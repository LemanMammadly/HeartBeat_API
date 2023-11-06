using System.Numerics;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.AppoinmentDtos;
using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Business.Dtos.PatientHistoryDtos;
using SerenityHospital.Business.Dtos.PatientRoomDtos;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Images;
using SerenityHospital.Business.Exceptions.PatientRooms;
using SerenityHospital.Business.Exceptions.Patients;
using SerenityHospital.Business.Exceptions.Roles;
using SerenityHospital.Business.Exceptions.Tokens;
using SerenityHospital.Business.Extensions;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.Core.Enums;
using SerenityHospital.DAL.Repositories.Implements;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class PatientService : IPatientService
{
    readonly UserManager<Patient> _userManager;
    readonly UserManager<AppUser> _AppUserManager;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly IPatientRoomRepository _patientRoomRepository;
    readonly IFileService _fileService;
    readonly IMapper _mapper;
    readonly ITokenService _tokenService;
    readonly IHttpContextAccessor _httpContext;
    readonly string? userId;
    readonly SignInManager<Patient> _signInManager;
    readonly IConfiguration _config;

    public PatientService(UserManager<Patient> userManager, UserManager<AppUser> appUserManager, IPatientRoomRepository patientRoomRepository, IFileService fileService, IMapper mapper, ITokenService tokenService, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContext, SignInManager<Patient> signInManager, IConfiguration config)
    {
        _userManager = userManager;
        _AppUserManager = appUserManager;
        _patientRoomRepository = patientRoomRepository;
        _fileService = fileService;
        _mapper = mapper;
        _tokenService = tokenService;
        _roleManager = roleManager;
        _httpContext = httpContext;
        userId = _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _signInManager = signInManager;
        _config = config;
    }

    public async Task AddPatientRoom(AddPatientRoomDto dto)
    {
        var room = await _patientRoomRepository.GetByIdAsync(dto.RoomId,"Patients");
        if (room is null) throw new NotFoundException<PatientRoom>();
        if (room.IsDeleted==true) throw new NotFoundException<PatientRoom>();
        if (room.Status == PatientRoomStatus.Occupied || room.Status == PatientRoomStatus.OutOfService) throw new PatientRoomIsNotAvailableException();

        var user = await _userManager.FindByIdAsync(dto.Id);
        if (user is null) throw new AppUserNotFoundException<Patient>();


        if (user.PatientRoomId != null) throw new PatientHavAlreadyRoomException();

        room.Patients.Add(user);

        if (room.Patients.Count() == room.Capacity)
        {
            room.Status = PatientRoomStatus.Occupied;
        }

        if (room.Patients.Count() > room.Capacity) throw new PatientRoomCapacityIsFullException();

        await _patientRoomRepository.SaveAsync();
    }

    public async Task AddRole(AddRoleDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.userName);
        if (user is null) throw new NotFoundException<Patient>();

        if(!await _roleManager.RoleExistsAsync(dto.roleName)) throw new NotFoundException<IdentityRole>();

        var result = await _userManager.AddToRoleAsync(user, dto.roleName);

        if(!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " "; 
            }
            throw new RoleCreatedFailedException();
        }
    }

    public async Task CreateAsync(PatientCreateDto dto)
    {
        if(dto.ImageFile!=null)
        {
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
        }

        if (await _userManager.Users.AnyAsync(p => p.UserName == dto.UserName || p.Email == dto.Email || p.PhoneNumber ==dto.PhoneNumber)) throw new AppUserIsAlreadyExistException<Patient>();
        if (await _AppUserManager.Users.AnyAsync(a => a.UserName == dto.UserName || a.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Patient>();

        var patient = _mapper.Map<Patient>(dto);

        if (dto.ImageFile != null) 
        {
            patient.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.PatientImageRoot);
        }

        var result = await _userManager.CreateAsync(patient, dto.Password);

        if(!result.Succeeded)
        {
            string a = " ";

            foreach (var err in result.Errors)
            {
                a += err.Description + " ";
            }
            throw new RegisterFailedException<Patient>();
        }

        await _userManager.AddToRoleAsync(patient, "Patient");
    }

    public async Task DeleteAsync(string id)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.Include(p=>p.Appoinments).Include(p=>p.PatientRoom).Include(p=>p.Recipes).SingleOrDefaultAsync(p=>p.Id==id);
        if (user is null) throw new AppUserNotFoundException<Patient>();
        if(user.ImageUrl != null)
        {
            _fileService.Delete(user.ImageUrl);
        }

        if (user.Appoinments.Count() > 0) throw new PatientHasAppoinmentException();
        if (user.Recipes.Count() > 0) throw new PatientHasAppoinmentException();

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserDeleteFailedException<Patient>(a);
        }
    }

    public async Task<ICollection<PatientListItemDto>> GetAllAsync()
    {
        ICollection<PatientListItemDto> patients = new List<PatientListItemDto>();

            foreach (var patient in await _userManager.Users.Include(p=>p.PatientRoom).Include(p=>p.PatientHistories).Include(p => p.Recipes).Include(p=>p.Appoinments).ThenInclude(a=>a.Doctor).ToListAsync())
            {
                var patientDto = new PatientListItemDto
                {
                    Id=patient.Id,
                    Name = patient.Name,
                    Surname = patient.Surname,
                    UserName = patient.UserName,
                    ImageUrl = _config["Jwt:Issuer"] + "wwwroot/" + patient.ImageUrl,
                    PhoneNumber =patient.PhoneNumber,
                    Email = patient.Email,
                    Age = patient.Age,
                    Address = patient.Address,
                    Gender = patient.Gender,
                    BloodType = patient.BloodType,
                    Roles = await _userManager.GetRolesAsync(patient),
                    PatientRoom = _mapper.Map<PatientRoomInfoDto>(patient.PatientRoom),
                    Appoinments=_mapper.Map<ICollection<AppoinmentInfoDto>>(patient.Appoinments),
                    Recipes=_mapper.Map<ICollection<RecipeListItemDto>>(patient.Recipes),
                    PatientHistories=_mapper.Map<ICollection<PatientHistoryListItemDto>>(patient.PatientHistories)
                };
                patients.Add(patientDto);
            }
            return patients;
    }

    public async Task<PatientDetailItemDto> GetById(string id)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.Include(p => p.PatientRoom).Include(p => p.PatientHistories).Include(p=>p.Recipes).Include(p=>p.Appoinments).ThenInclude(a=>a.Doctor).SingleOrDefaultAsync(p => p.Id == id);
        if (user is null) throw new AppUserNotFoundException<Patient>();
        var userDto = new PatientDetailItemDto
        {
            Id=user.Id,
            Name = user.Name,
            Surname = user.Surname,
            UserName = user.UserName,
            ImageUrl = _config["Jwt:Issuer"] + "wwwroot/" + user.ImageUrl,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Age = user.Age,
            Address = user.Address,
            Gender = user.Gender,
            BloodType = user.BloodType,
            Roles = await _userManager.GetRolesAsync(user),
            PatientRoom = _mapper.Map<PatientRoomInfoDto>(user.PatientRoom),
            Appoinments = _mapper.Map<ICollection<AppoinmentInfoDto>>(user.Appoinments),
            Recipes = _mapper.Map<ICollection<RecipeListItemDto>>(user.Recipes),
            PatientHistories=_mapper.Map<ICollection<PatientHistoryListItemDto>>(user.PatientHistories)
        };
        return userDto;
    }

    public async Task<TokenResponseDto> LoginAsync(PatientLoginDto dto)
    {
        var patient = await _userManager.FindByNameAsync(dto.UserName);

        if (patient == null) throw new LoginFailedException<Patient>("Username or password is wrong");

        var result = await _userManager.CheckPasswordAsync(patient, dto.Password);
        if (!result) throw new LoginFailedException<Patient>("Username or password is wrong");

        return _tokenService.CreatePatientToken(patient);
    }

    public async Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.SingleOrDefaultAsync(p => p.RefreshToken == refreshToken);
        if (user is null) throw new NotFoundException<Patient>();
        if (user.RefreshTokenExpiresDate < DateTime.UtcNow.AddHours(4)) throw new RefreshTokenExpiresIsOldException();
        return _tokenService.CreatePatientToken(user);
    }

    public async Task Logout()
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new AppUserNotFoundException<Patient>();
        await _signInManager.SignOutAsync();
        user.RefreshToken = null;
        user.RefreshTokenExpiresDate = null;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new LogoutFaileException<Patient>();
    }

    public async Task RemoveRole(RemoveRoleDto dto)
    {
        if (!await _roleManager.RoleExistsAsync(dto.roleName)) throw new NotFoundException<IdentityRole>();
        var user = await _userManager.FindByNameAsync(dto.userName);
        if (user is null) throw new NotFoundException<Patient>();
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

    public async Task UpdateAsync(PatientUpdateDto dto)
    {
        if (string.IsNullOrEmpty(userId)) throw new ArgumentIsNullException();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new NotFoundException<Patient>();

        if (await _userManager.Users.AnyAsync(p => p.UserName == dto.UserName && p.Id != userId || p.Email == dto.Email && p.Id != userId || p.PhoneNumber == dto.PhoneNumber && p.Id != userId)) throw new AppUserIsAlreadyExistException<Patient>(); 
        if (await _AppUserManager.Users.AnyAsync(a => a.UserName == dto.UserName && a.Id != userId || a.Email == dto.Email && a.Id != userId || a.PhoneNumber == dto.PhoneNumber && a.Id != userId)) throw new AppUserIsAlreadyExistException<Patient>();

        if(dto.ImageFile !=null)
        {
            if(user.ImageUrl != null)
            {
                _fileService.Delete(user.ImageUrl);
            }
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
            user.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.PatientImageRoot);
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
            throw new AppUserUpdateFailedException<Patient>();
        }
    }

    public async Task UpdateByAdminAsync(string id, PatientUpdateByAdminDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) throw new AppUserNotFoundException<Patient>();

        if (await _userManager.Users.AnyAsync(p => p.UserName == dto.UserName && p.Id != id ||
        p.Email == dto.Email && p.Id != id || p.PhoneNumber == dto.PhoneNumber && p.Id != id))
            throw new AppUserIsAlreadyExistException<Patient>();

        if (await _AppUserManager.Users.AnyAsync(a => a.UserName == dto.UserName && a.Id != id ||
        a.Email == dto.Email && a.Id != id || a.PhoneNumber == dto.PhoneNumber && a.Id != id))
            throw new AppUserIsAlreadyExistException<Patient>();

        if (dto.ImageFile != null)
        {
            if (user.ImageUrl != null)
            {
                _fileService.Delete(user.ImageUrl);
            }
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
            user.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.PatientImageRoot);
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
            throw new AppUserUpdateFailedException<Patient>();
        }
    }

    public async Task<int> Count()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Count();
    }

    public async Task<PatientDetailItemDto> GetByName(string userName)
    {
        if (string.IsNullOrEmpty(userName)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.Include(p => p.PatientRoom).Include(p => p.PatientHistories).Include(p => p.Recipes).Include(p => p.Appoinments).ThenInclude(a => a.Doctor).SingleOrDefaultAsync(p => p.UserName == userName);
        if (user is null) throw new AppUserNotFoundException<Patient>();
        var userDto = new PatientDetailItemDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            UserName = user.UserName,
            ImageUrl = _config["Jwt:Issuer"] + "wwwroot/" + user.ImageUrl,
            PhoneNumber = user.PhoneNumber,
            Email=user.Email,
            Age = user.Age,
            Address = user.Address,
            Gender = user.Gender,
            BloodType = user.BloodType,
            Roles = await _userManager.GetRolesAsync(user),
            PatientRoom = _mapper.Map<PatientRoomInfoDto>(user.PatientRoom),
            Appoinments = _mapper.Map<ICollection<AppoinmentInfoDto>>(user.Appoinments),
            Recipes = _mapper.Map<ICollection<RecipeListItemDto>>(user.Recipes),
            PatientHistories = _mapper.Map<ICollection<PatientHistoryListItemDto>>(user.PatientHistories)
        };
        return userDto;
    }
}

