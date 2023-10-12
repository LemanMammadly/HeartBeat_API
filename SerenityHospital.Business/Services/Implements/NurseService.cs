using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Business.Dtos.NurseDtos;
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
using SerenityHospital.Core.Enums;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class NurseService : INurseService
{
    readonly UserManager<Nurse> _userManager;
    readonly UserManager<AppUser> _appUserManager;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly IDepartmentRepository _departmentRepository;
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly ITokenService _tokenService;
    readonly IHttpContextAccessor _context;
    readonly string? userId;
    readonly SignInManager<Nurse> _signInManager;

    public NurseService(UserManager<Nurse> userManager, UserManager<AppUser> appUserManager, IDepartmentRepository departmentRepository, IMapper mapper, IFileService fileService, ITokenService tokenService, RoleManager<IdentityRole> roleManager, IHttpContextAccessor context, SignInManager<Nurse> signInManager)
    {
        _userManager = userManager;
        _appUserManager = appUserManager;
        _departmentRepository = departmentRepository;
        _mapper = mapper;
        _fileService = fileService;
        _tokenService = tokenService;
        _roleManager = roleManager;
        _context = context;
        userId = _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _signInManager = signInManager;
    }

    public async Task AddRole(AddRoleDto dto)
    {
        if (!await _roleManager.RoleExistsAsync(dto.roleName)) throw new NotFoundException<IdentityRole>();

        var user = await _userManager.FindByNameAsync(dto.userName);
        if (user == null) throw new AppUserNotFoundException<Nurse>();

        var result = await _userManager.AddToRoleAsync(user, dto.roleName);

        if(!result.Succeeded)
        {
            string a = " ";
            foreach (var err in result.Errors)
            {
                a += err.Description + " ";
            }
            throw new RoleCreatedFailedException(a);
        }
    }

    public async Task CreateAsync(NurseCreateDto dto)
    {
        if(dto.ImageFile != null)
        {
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
        }

        if (await _userManager.Users.AnyAsync(u => u.UserName == dto.UserName || u.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Nurse>();
        if (await _appUserManager.Users.AnyAsync(a => a.UserName == dto.UserName || a.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Nurse>();

        var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
        if (department is null) throw new NotFoundException<Department>();
        if (department.IsDeleted==true) throw new NotFoundException<Department>();

        var nurse = _mapper.Map<Nurse>(dto);

        nurse.DepartmentId = department.Id;
        nurse.Status = WorkStatus.Active;

        if(dto.ImageFile != null)
        {
            nurse.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.NurseImageRoot);
        }

        var result = await _userManager.CreateAsync(nurse,dto.Password);
        if(!result.Succeeded)
        {
            string a = " ";
            foreach (var err in result.Errors)
            {
                a += err.Description + " ";
            }
            throw new RegisterFailedException<Nurse>();
        }
    }

    public async Task<ICollection<NurseListItemDto>> GetAllAsync(bool takeAll)
    {
        ICollection<NurseListItemDto> nurses = new List<NurseListItemDto>();

        if(takeAll)
        {
            foreach (var user in await _userManager.Users.Include(u=>u.Department).ToListAsync())
            {
                var userDto = new NurseListItemDto()
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Age = user.Age,
                    UserName = user.UserName,
                    Email = user.Email,
                    ImageUrl = user.ImageUrl,
                    StartWork = user.StartWork,
                    EndWork = user.EndWork,
                    IsDeleted = user.IsDeleted,
                    Department = _mapper.Map<DepartmentInfoDto>(user.Department),
                    Roles = await _userManager.GetRolesAsync(user)
                };
                nurses.Add(userDto);
            }
            return nurses;
        }
        else {
            foreach (var user in await _userManager.Users.Include(u=>u.Department).Where(u=>u.IsDeleted==false).ToListAsync())
            {
                var userDto = new NurseListItemDto()
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Age = user.Age,
                    UserName = user.UserName,
                    Email = user.Email,
                    ImageUrl = user.ImageUrl,
                    StartWork = user.StartWork,
                    EndWork = user.EndWork,
                    IsDeleted = user.IsDeleted,
                    Department = _mapper.Map<DepartmentInfoDto>(user.Department),
                    Roles=await _userManager.GetRolesAsync(user)
                };
                nurses.Add(userDto);
            }
            return nurses;
        }
    }

    public async Task<NurseDetailItemDto> GetById(bool takeAll, string id)
    {
        if(takeAll)
        {
            var user = await _userManager.Users.Include(u=>u.Department).FirstOrDefaultAsync(u=>u.Id==id);
            if (user is null) throw new AppUserNotFoundException<Nurse>();
            var userDto = new NurseDetailItemDto()
            {
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age,
                UserName = user.UserName,
                Email = user.Email,
                ImageUrl = user.ImageUrl,
                StartWork = user.StartWork,
                EndWork = user.EndWork,
                IsDeleted = user.IsDeleted,
                Department = _mapper.Map<DepartmentInfoDto>(user.Department),
                Roles = await _userManager.GetRolesAsync(user)
            };
            return userDto;
        }

        else {
            var user = await _userManager.Users.Include(u => u.Department).Where(u=>u.IsDeleted==false).FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) throw new AppUserNotFoundException<Nurse>();
            var userDto = new NurseDetailItemDto()
            {
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age,
                UserName = user.UserName,
                Email = user.Email,
                ImageUrl = user.ImageUrl,
                StartWork = user.StartWork,
                EndWork = user.EndWork,
                IsDeleted = user.IsDeleted,
                Department = _mapper.Map<DepartmentInfoDto>(user.Department),
                Roles = await _userManager.GetRolesAsync(user)
            };
            return userDto;
        }
    }

    public async Task<TokenResponseDto> LoginAsync(NurseLoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user is null) throw new LoginFailedException<Nurse>("Username or password is wrong");

        var result = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!result) throw new LoginFailedException<Nurse>("Username or password is wrong");

        return _tokenService.CreateNurseToken(user);
    }

    public async Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user is null) throw new NotFoundException<Nurse>();
        if (user.RefreshTokenExpiresDate < DateTime.UtcNow.AddHours(4)) throw new RefreshTokenExpiresIsOldException();
        return _tokenService.CreateNurseToken(user);
    }

    public async Task RemoveRole(RemoveRoleDto dto)
    {
        if (!await _roleManager.RoleExistsAsync(dto.roleName)) throw new NotFoundException<IdentityRole>();

        var user = await _userManager.FindByNameAsync(dto.userName);
        if (user is null) throw new NotFoundException<Nurse>();

        var result = await _userManager.RemoveFromRoleAsync(user, dto.roleName);

        if(!result.Succeeded)
        {
            string a = " ";
            foreach (var err in result.Errors)
            {
                a += err.Description + " ";
            }
            throw new RoleRemoveFailedException();
        }
    }

    public async Task SoftDeleteAsync(string id)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentIsNullException();
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) throw new AppUserNotFoundException<Nurse>();

        user.IsDeleted = true;
        user.Status = WorkStatus.leave;
        user.EndDate = DateTime.UtcNow.AddHours(4);

        var result = await _userManager.UpdateAsync(user);
        if(!result.Succeeded)
        {
            string a = "";
            foreach (var err in result.Errors)
            {
                a += err.Description + " ";
            }
            throw new AppUserDeleteFailedException<Nurse>();
        }
    }

    public async Task RevertSoftDeleteAsync(string id)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentIsNullException();
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) throw new AppUserNotFoundException<Nurse>();

        user.IsDeleted = false;
        user.Status = WorkStatus.Active;
        user.StartDate = DateTime.UtcNow.AddHours(4);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            string a = "";
            foreach (var err in result.Errors)
            {
                a += err.Description + " ";
            }
            throw new AppUserUpdateFailedException<Nurse>();
        }
    }

    public async Task UpdateAsync(NurseUpdateDto dto)
    {
        if (string.IsNullOrEmpty(userId)) throw new AppUserNotFoundException<Nurse>();
        if (!await _userManager.Users.AnyAsync(u => u.Id == userId)) throw new AppUserNotFoundException<Nurse>();

        var user = await _userManager.FindByIdAsync(userId);

        if (await _userManager.Users.AnyAsync(u => (u.UserName == dto.UserName && u.Id != userId) || (u.Email == dto.Email && u.Id != userId))) throw new AppUserIsAlreadyExistException<Nurse>();
        if (await _appUserManager.Users.AnyAsync(u => (u.UserName == dto.UserName && u.Id != userId) || (u.Email == dto.Email && u.Id != userId))) throw new AppUserIsAlreadyExistException<Nurse>();

        if(dto.ImageFile != null)
        {
            if(user.ImageUrl !=null)
            {
                _fileService.Delete(user.ImageUrl);
            }
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
            user.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.NurseImageRoot);
        }

        var newUser = _mapper.Map(dto, user);
        var result = await _userManager.UpdateAsync(newUser);

        if(!result.Succeeded)
        {
            string a = "";
            foreach (var err in result.Errors)
            {
                a += err.Description + " ";
            }
            throw new AppUserUpdateFailedException<Nurse>();
        }

    }

    public async Task UpdateByAdminAsync(string id,NurseUpdateByAdminDto dto)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentIsNullException();
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) throw new AppUserNotFoundException<Nurse>();

        if (await _userManager.Users.AnyAsync(u => (u.UserName == dto.UserName && u.Id != id) || (u.Email == dto.Email && u.Id != id))) throw new AppUserIsAlreadyExistException<Nurse>();
        if (await _appUserManager.Users.AnyAsync(u => (u.UserName == dto.UserName && u.Id != id) || (u.Email == dto.Email && u.Id != id))) throw new AppUserIsAlreadyExistException<Nurse>();

        if (dto.ImageFile != null)
        {
            if (user.ImageUrl != null)
            {
                _fileService.Delete(user.ImageUrl);
            }
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
            user.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.NurseImageRoot);
        }

        var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
        if (department == null) throw new NotFoundException<Department>();
        if (department.IsDeleted == true) throw new NotFoundException<Department>();

        if(dto.Status==WorkStatus.leave)
        {
            await SoftDeleteAsync(id);
        }

        if(dto.Status==WorkStatus.Active)
        {
            await RevertSoftDeleteAsync(id);
        }

        var newUser = _mapper.Map(dto, user);

        var result = await _userManager.UpdateAsync(newUser);

        if (!result.Succeeded)
        {
            string a = "";
            foreach (var err in result.Errors)
            {
                a += err.Description + " ";
            }
            throw new AppUserUpdateFailedException<Nurse>();
        }
    }

    public async Task DeleteAsync(string id)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentIsNullException();
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) throw new AppUserNotFoundException<Nurse>();

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserDeleteFailedException<Nurse>(a);
        }
    }

    public async Task Logout()
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new AppUserNotFoundException<Nurse>();
        await _signInManager.SignOutAsync();
        user.RefreshToken = null;
        user.RefreshTokenExpiresDate = null;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new LogoutFaileException<Nurse>();
    }
}

