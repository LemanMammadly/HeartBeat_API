using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.AdminDtos;
using SerenityHospital.Business.Dtos.AppoinmentDtos;
using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Business.Dtos.PatientHistoryDtos;
using SerenityHospital.Business.Dtos.PatientRoomDtos;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Images;
using SerenityHospital.Business.Exceptions.Roles;
using SerenityHospital.Business.Exceptions.Tokens;
using SerenityHospital.Business.Extensions;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Services.Implements;

public class AdminService : IAdminService
{
    readonly UserManager<Admin> _userManager;
    readonly UserManager<AppUser> _allUserManager;
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly IHttpContextAccessor _httpContext;
    readonly string? userId;
    readonly ITokenService _tokenService;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly SignInManager<Admin> _signInManager;

    public AdminService(UserManager<Admin> userManager, UserManager<AppUser> allUserManager, IMapper mapper, IFileService fileService, IHttpContextAccessor httpContext, ITokenService tokenService, RoleManager<IdentityRole> roleManager, SignInManager<Admin> signInManager)
    {
        _userManager = userManager;
        _allUserManager = allUserManager;
        _mapper = mapper;
        _fileService = fileService;
        _httpContext = httpContext;
        userId = _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _tokenService = tokenService;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    public async Task AddRoleAsync(AddRoleDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.userName);
        if (user is null) throw new NotFoundException<Admin>();

        var role = await _roleManager.RoleExistsAsync(dto.roleName);
        if (!role) throw new NotFoundException<IdentityRole>();

        var result = await _userManager.AddToRoleAsync(user, dto.roleName);

        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new RoleCreatedFailedException(a);
        }
    }

    public async Task CreateAsync(AdminCreateDto dto)
    {
        if(dto.ImageFile != null)
        {
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
        }

        if (await _userManager.Users.AnyAsync(a => a.UserName == dto.UserName || a.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Admin>();
        if (await _allUserManager.Users.AnyAsync(a => a.UserName == dto.UserName || a.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Admin>();

        var admin = _mapper.Map<Admin>(dto);

        if(dto.ImageFile!=null)
        {
            admin.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.AdminImageRoot);
        }

        var result = await _userManager.CreateAsync(admin, dto.Password);

        if(!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new RegisterFailedException<Admin>(a);
        }
    }

    public async Task DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentIsNullException();
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) throw new NotFoundException<Admin>();
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserDeleteFailedException<Admin>(a);
        }
    }

    public async Task<ICollection<AdminListItemDto>> GetAllAsync()
    {
        ICollection<AdminListItemDto> admins = new List<AdminListItemDto>();
        foreach (var admin in await _userManager.Users.ToListAsync())
        {
            var adminDto = new AdminListItemDto
            {
                Name = admin.Name,
                Surname = admin.Surname,
                ImageUrl = admin.ImageUrl,
                Email = admin.Email,
                Age = admin.Age,
                Roles = await _userManager.GetRolesAsync(admin),
            };
            admins.Add(adminDto);
        }
        return admins;
    }

    public async Task<AdminDetailItemDto> GetById(string id)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentIsNullException();
        var user=await _userManager.Users.SingleOrDefaultAsync(a => a.Id == id);
        if (user is null) throw new AppUserNotFoundException<Patient>();
        var adminDto = new AdminDetailItemDto
        {
            Name = user.Name,
            Surname = user.Surname,
            ImageUrl = user.ImageUrl,
            Email=user.Email,
            Age = user.Age,
            Roles = await _userManager.GetRolesAsync(user),
        };
        return adminDto;
    }

    public async Task<TokenResponseDto> LoginAsync(AdminLoginDto dto)
    {
        var admin = await _userManager.FindByNameAsync(dto.UserName);

        if (admin is null) throw new LoginFailedException<Admin>();

        var result = await _userManager.CheckPasswordAsync(admin, dto.Password);
        if (!result) throw new LoginFailedException<Admin>();

        return _tokenService.CreateAdminToken(admin);
    }

    public async Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.SingleOrDefaultAsync(s => s.RefreshToken == refreshToken);
        if (user is null) throw new ArgumentIsNullException();
        if (user.RefreshTokenExpiresDate < DateTime.UtcNow.AddHours(4)) throw new RefreshTokenExpiresIsOldException();
        return _tokenService.CreateAdminToken(user);
    }

    public async Task Logout()
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new AppUserNotFoundException<Admin>();
        await _signInManager.SignOutAsync();
        user.RefreshToken = null;
        user.RefreshTokenExpiresDate = null;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new LogoutFaileException<Admin>();
    }

    public async Task RemoveRoleAsync(RemoveRoleDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.userName);
        if (user is null) throw new NotFoundException<Admin>();

        var role = await _roleManager.RoleExistsAsync(dto.roleName);
        if (!role) throw new NotFoundException<IdentityRole>();

        var result = await _userManager.RemoveFromRoleAsync(user, dto.roleName);

        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new RoleRemoveFailedException(a);
        }
    }

    public async Task UpdateAsync(AdminUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentIsNullException();
        if (!await _userManager.Users.AnyAsync(u => u.Id == userId)) throw new NotFoundException<Admin>();

        var user = await _userManager.FindByIdAsync(userId);

        if (await _userManager.Users.AnyAsync(a => (a.Email == dto.Email && a.Id != userId) && (a.UserName == dto.UserName && a.Id != userId))) throw new AppUserIsAlreadyExistException<Admin>();
        if (await _allUserManager.Users.AnyAsync(a => (a.Email == dto.Email && a.Id != userId) && (a.UserName == dto.UserName && a.Id != userId))) throw new AppUserIsAlreadyExistException<Admin>();

        if(dto.ImageFile != null)
        {
            if(user.ImageUrl !=null)
            {
                _fileService.Delete(user.ImageUrl);
            }
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
            user.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.AdminImageRoot);
        }

        var newuser = _mapper.Map(dto, user);
        var result = await _userManager.UpdateAsync(newuser);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserUpdateFailedException<Admin>(a);
        }
    }

    public async Task UpdateByAdminAsync(string id,AdminUpdateBySuperAdminDto dto)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentIsNullException();
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) throw new NotFoundException<Admin>();

        if (await _userManager.Users.AnyAsync(a => (a.Email == dto.Email && a.Id != id) && (a.UserName == dto.UserName && a.Id != id))) throw new AppUserIsAlreadyExistException<Admin>();
        if (await _allUserManager.Users.AnyAsync(a => (a.Email == dto.Email && a.Id != id) && (a.UserName == dto.UserName && a.Id != id))) throw new AppUserIsAlreadyExistException<Admin>();


        if (dto.ImageFile != null)
        {
            if (user.ImageUrl != null)
            {
                _fileService.Delete(user.ImageUrl);
            }
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
            user.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.AdminImageRoot);
        }

        var newuser = _mapper.Map(dto, user);
        var result = await _userManager.UpdateAsync(newuser);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserUpdateFailedException<Admin>(a);
        }
    }
}

