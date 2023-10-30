using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.AdminstratorDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Confirms;
using SerenityHospital.Business.Exceptions.Images;
using SerenityHospital.Business.Exceptions.Roles;
using SerenityHospital.Business.Exceptions.Tokens;
using SerenityHospital.Business.Extensions;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.Core.Enums;
using SerenityHospital.DAL.Contexts;
using SerenityHospital.DAL.Repositories.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Policy;
using System;

namespace SerenityHospital.Business.Services.Implements;

public class AdminstratorService : IAdminstratorService
{
    readonly UserManager<Adminstrator> userManager;
    readonly UserManager<AppUser> _appUserManager;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly IHttpContextAccessor _context;
    readonly string? userId;
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly IHospitalRepository _hospitalRepository;
    readonly ITokenService _tokenService;
    readonly SignInManager<Adminstrator> _signInManager;
    readonly AppDbContext _appDbContext;
    readonly IConfiguration _config;
    readonly IEmailServiceSender _emailService;

    public AdminstratorService(UserManager<Adminstrator> userManager, IMapper mapper, IFileService fileService, IHospitalRepository hospitalRepository, ITokenService tokenService, RoleManager<IdentityRole> roleManager, IHttpContextAccessor context, UserManager<AppUser> appUserManager, SignInManager<Adminstrator> signInManager, AppDbContext appDbContext, IConfiguration config, IEmailServiceSender emailService)
    {
        this.userManager = userManager;
        _context = context;
        userId = _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _mapper = mapper;
        _fileService = fileService;
        _hospitalRepository = hospitalRepository;
        _tokenService = tokenService;
        _roleManager = roleManager;
        _appUserManager = appUserManager;
        _signInManager = signInManager;
        _appDbContext = appDbContext;
        _config = config;
        _emailService = emailService;
    }

    public async Task CreateAsync(CreateAdminstratorDto dto)
    {
        var existAdminstrator = await userManager.Users.FirstOrDefaultAsync(a => a.IsDeleted == false);

        if (existAdminstrator != null) throw new AppUserIsAlreadyExistException<Adminstrator>();

        if (dto.ImageFile != null)
        {
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
        }

        var adminstrator = _mapper.Map<Adminstrator>(dto);

        if(dto.ImageFile != null)
        {
            adminstrator.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.AdminstratortImageRoot);
        }

        adminstrator.Status = WorkStatus.Active;

        if (await userManager.Users.AnyAsync(a => a.UserName == dto.UserName || a.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Adminstrator>();
        if (await _appUserManager.Users.AnyAsync(d => d.UserName == dto.UserName || d.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Adminstrator>();


        var hospital =await _hospitalRepository.GetFirstAsync();

        adminstrator.HospitalId = hospital.Id;

        var result = await userManager.CreateAsync(adminstrator, dto.Password);

        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new RegisterFailedException<Adminstrator>(a);
        }
    }

    public async Task SoftDeleteAsync(string id)
    {
        var adminstrator = await userManager.Users.FirstOrDefaultAsync(a => a.Id == id);
        if (adminstrator == null) throw new AppUserNotFoundException<Adminstrator>();
        adminstrator.IsDeleted = true;
        adminstrator.HospitalId = null;
       
        var result = await userManager.UpdateAsync(adminstrator);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserUpdateFailedException<Nurse>(a);
        }
    }

    public async Task RevertSoftDeleteAsync(string id)
    {
        var adminstrator = await userManager.Users.FirstOrDefaultAsync(a => a.Id == id);
        if (adminstrator == null) throw new AppUserNotFoundException<Adminstrator>();

        var existAdminstrator= await userManager.Users.FirstOrDefaultAsync(a => a.IsDeleted == false);
        if(existAdminstrator != null) throw new AppUserIsAlreadyExistException<Adminstrator>();

        var hospital = await _hospitalRepository.GetFirstAsync();
        adminstrator.IsDeleted = false;
        adminstrator.HospitalId = hospital.Id;

        var result = await userManager.UpdateAsync(adminstrator);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserUpdateFailedException<Nurse>(a);
        }
    }

    public async Task<TokenResponseDto> LoginAsync(LoginAdminstratorDto dto)
    {
        var adminstrator = await userManager.FindByNameAsync(dto.UserName);

        //if (adminstrator.IsDeleted)
        //{
        //    throw new LoginFailedException<Adminstrator>("This user is delete");
        //};

        if (adminstrator == null) throw new LoginFailedException<Adminstrator>("Username or password is wrong");

        var result = await userManager.CheckPasswordAsync(adminstrator, dto.Password);
        if (!result) throw new LoginFailedException<Adminstrator>("Username or password is wrong");

        return _tokenService.CreateAdminstratorToken(adminstrator);
    }

    public async Task AddRoleAsync(AddRoleDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.userName);
        if (user == null) throw new NotFoundException<Adminstrator>();

        if (!await _roleManager.RoleExistsAsync(dto.roleName)) throw new NotFoundException<IdentityRole>();

        var result = await userManager.AddToRoleAsync(user, dto.roleName);

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

    public async Task RemoveRoleAsync(RemoveRoleDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.userName);
        if (user == null) throw new NotFoundException<Adminstrator>();

        if (!await _roleManager.RoleExistsAsync(dto.roleName)) throw new NotFoundException<IdentityRole>();

        var result = await userManager.RemoveFromRoleAsync(user, dto.roleName);

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

    public async Task<ICollection<AdminstratorListItemDto>> GetAllAsync(bool takeAll)
    {
        ICollection<AdminstratorListItemDto> users = new List<AdminstratorListItemDto>();

        if(takeAll)
        {
            foreach (var user in await userManager.Users.ToListAsync())
            {
                var userDto = new AdminstratorListItemDto
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    ImageUrl = user.ImageUrl,
                    UserName = user.UserName,
                    Roles = await userManager.GetRolesAsync(user),
                    IsDeleted=user.IsDeleted
                };
                users.Add(userDto);
            }
            return users;
        }
        else
        {
            foreach (var user in await userManager.Users.Where(a=>a.IsDeleted==false).ToListAsync())
            {
                var userDto = new AdminstratorListItemDto
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    ImageUrl = user.ImageUrl,
                    UserName = user.UserName,
                    Roles = await userManager.GetRolesAsync(user),
                    IsDeleted = user.IsDeleted
                };
                users.Add(userDto);
            }
            return users;
        }
    }

    public async Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentIsNullException();
        var user = await userManager.Users.SingleOrDefaultAsync(x => x.RefreshToken == refreshToken);
        if (user == null) throw new NotFoundException<Adminstrator>();
        if (user.RefreshTokenExpiresDate < DateTime.UtcNow.AddHours(4)) throw new RefreshTokenExpiresIsOldException();
        return _tokenService.CreateAdminstratorToken(user);
    }

    public async Task UpdateAsync(AdminstratorUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentIsNullException();
        if (!await userManager.Users.AnyAsync(u => u.Id == userId)) throw new AppUserNotFoundException<Adminstrator>();

        var user = await userManager.FindByIdAsync(userId);

        if (await userManager.Users.AnyAsync(a=>(a.UserName==dto.UserName && a.Id != userId) || (a.Email==dto.Email && a.Id !=userId))) throw new AppUserIsAlreadyExistException<Adminstrator>();

        if (dto.ImageFile != null)
        {
            if(user.ImageUrl != null)
            {
                _fileService.Delete(user.ImageUrl);
            }
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
            user.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.AdminstratortImageRoot);
        }

        var newUser = _mapper.Map(dto, user);
        var result =await userManager.UpdateAsync(newUser);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserUpdateFailedException<Adminstrator>(a);
        }
    }

    public async Task UpdateByAdminAsync(string id, AdminstratorUpdateByAdminDto dto)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentIsNullException();
        var user = await userManager.FindByIdAsync(id);
        if (user == null) throw new AppUserNotFoundException<Adminstrator>();

        if (await userManager.Users.AnyAsync(a => (a.UserName == dto.UserName && a.Id != id) || (a.Email == dto.Email && a.Id != id))) throw new AppUserIsAlreadyExistException<Adminstrator>();

        if (dto.ImageFile != null)
        {
            if (user.ImageUrl != null)
            {
                _fileService.Delete(user.ImageUrl);
            }
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
            user.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.AdminstratortImageRoot);
        }

        if(dto.Status==WorkStatus.leave)
        {
            user.EndDate = DateTime.UtcNow.AddHours(4);
            await SoftDeleteAsync(id);
        }

        var hospital = await _hospitalRepository.GetFirstAsync();

        if (dto.Status == WorkStatus.Active)
        {
            user.StartDate = DateTime.UtcNow.AddHours(4);
            user.EndDate = null;
            await RevertSoftDeleteAsync(id);
        }

        var newUser = _mapper.Map(dto, user);
        var result = await userManager.UpdateAsync(newUser);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserUpdateFailedException<Adminstrator>(a);
        }
    }

    public async Task DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentIsNullException();
        var user = await userManager.FindByIdAsync(id);
        if (user == null) throw new AppUserNotFoundException<Adminstrator>();

        if (user.ImageUrl != null)
        {
            _fileService.Delete(user.ImageUrl);
        }

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserDeleteFailedException<Adminstrator>(a);
        }
    }

    public async Task Logout()
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) throw new AppUserNotFoundException<Adminstrator>();
        await _signInManager.SignOutAsync();
        user.RefreshToken = null;
        user.RefreshTokenExpiresDate = null;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new LogoutFaileException<Adminstrator>();
    }

    public async Task<AdminstratorDetailItemDto> GetById(string id, bool takeAll)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentIsNullException();
        if (takeAll)
        {
            var user=await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) throw new AppUserNotFoundException<Adminstrator>();
            var userDto = new AdminstratorDetailItemDto()
            {
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                ImageUrl = user.ImageUrl,
                Roles = await userManager.GetRolesAsync(user)
            };
            return userDto;
        }
        else
        {
            var user = await userManager.Users.Where(u=>u.IsDeleted==false).FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) throw new AppUserNotFoundException<Adminstrator>();
            var userDto = new AdminstratorDetailItemDto()
            {
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                ImageUrl = user.ImageUrl,
                Roles = await userManager.GetRolesAsync(user)
            };
            return userDto;
        }
    }

    //public async Task ConfirmEmail(string token, string email)
    //{
    //    var user =await  userManager.FindByEmailAsync(email);
    //    if(user is not null)
    //    {
    //        var result = await userManager.ConfirmEmailAsync(user, token);
    //        if (!result.Succeeded) throw new EmailConfirmationFailedException();
    //    }
    //    throw new NotFoundException<Adminstrator>();
    //}
}

