using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.PatientDtos;
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

    public PatientService(UserManager<Patient> userManager, UserManager<AppUser> appUserManager, IPatientRoomRepository patientRoomRepository, IFileService fileService, IMapper mapper, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _AppUserManager = appUserManager;
        _patientRoomRepository = patientRoomRepository;
        _fileService = fileService;
        _mapper = mapper;
        _tokenService = tokenService;
        _roleManager = roleManager;
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

        if (await _userManager.Users.AnyAsync(p => p.UserName == dto.UserName || p.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Patient>();
        if (await _AppUserManager.Users.AnyAsync(a => a.UserName == dto.UserName || a.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Patient>();

        if (dto.PatientRoomId != null)
        {
            var IsExistPatientRoom = await _patientRoomRepository.GetSingleAsync(p => p.Id == dto.PatientRoomId);
            if (IsExistPatientRoom == null) throw new NotFoundException<PatientRoom>();
        }

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
    }

    public Task<ICollection<PatientListItemDto>> GetAllAsync(bool takeAll)
    {
        throw new NotImplementedException();
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
}

