﻿using System.Numerics;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.AdminstratorDtos;
using SerenityHospital.Business.Dtos.AppoinmentDtos;
using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Dtos.DoctorRoom;
using SerenityHospital.Business.Dtos.PositionDtos;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Doctors;
using SerenityHospital.Business.Exceptions.Images;
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

public class DoctorService : IDoctorService
{
    readonly UserManager<Doctor> _userManager;
    readonly UserManager<AppUser> _appUserManager;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly IHttpContextAccessor _context;
    readonly string? userId;
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly IDepartmentRepository _departmentRepository;
    readonly IPositionRepository _positionRepository;
    readonly ITokenService _tokenService;
    readonly SignInManager<Doctor> _signInManager;
    readonly IDoctorRoomRepository _doctorRoomRepository;
    readonly IConfiguration _config;

    public DoctorService(UserManager<Doctor> userManager, IMapper mapper, IFileService fileService, IDepartmentRepository departmentRepository, IPositionRepository positionRepository, UserManager<AppUser> appUserManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager, IHttpContextAccessor context, SignInManager<Doctor> signInManager, IDoctorRoomRepository doctorRoomRepository, IConfiguration config)
    {
        _userManager = userManager;
        _mapper = mapper;
        _fileService = fileService;
        _departmentRepository = departmentRepository;
        _positionRepository = positionRepository;
        _appUserManager = appUserManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
        _context = context;
        userId = _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _signInManager = signInManager;
        _doctorRoomRepository = doctorRoomRepository;
        _config = config;
    }

    public async Task AddRole(AddRoleDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.userName);
        if (user == null) throw new NotFoundException<Doctor>();

        if (!await _roleManager.RoleExistsAsync(dto.roleName)) throw new NotFoundException<IdentityRole>();

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

    public async Task CreateAsync(DoctorCreateDto dto)
    {
        if(dto.ImageFile!=null)
        {
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
        }

        if (await _userManager.Users.AnyAsync(d => d.UserName == dto.UserName || d.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Doctor>();
        if (await _appUserManager.Users.AnyAsync(d => d.UserName == dto.UserName || d.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Doctor>();


        var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
        if (department == null) throw new NotFoundException<Department>();
        if (department.IsDeleted==true) throw new NotFoundException<Department>();

        var position = await _positionRepository.GetByIdAsync(dto.PositionId);
        if (position == null) throw new NotFoundException<Position>();
        if (position.IsDeleted==true) throw new NotFoundException<Position>();


        var doctor = _mapper.Map<Doctor>(dto);

        doctor.Status = WorkStatus.Active;

        if (dto.ImageFile != null)
        {
            doctor.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.DoctorImageRoot);
        }

        var result = await _userManager.CreateAsync(doctor, dto.Password);

        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new RegisterFailedException<Doctor>(a);
        }
    }

    public async Task<ICollection<DoctorListItemDto>> GetAllAsync(bool takeAll)
    {
        ICollection<DoctorListItemDto> users = new List<DoctorListItemDto>();
        if (takeAll)
        {
            foreach (var user in await _userManager.Users.Include(d=>d.Department).Include(d=>d.AppointmentsAsPatient).Include(d=>d.Position).Include(d=>d.Recipes).Include(d=>d.DoctorRoom).Include(d=>d.Appoinments).ThenInclude(a=>a.Patient).ToListAsync())
            {
                var userDto = new DoctorListItemDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    ImageUrl =  _config["Jwt:Issuer"] + "wwwroot/" + user.ImageUrl,
                    Age=user.Age,
                    Gender=user.Gender,
                    StartDate=user.StartDate,
                    EndDate=user.EndDate,
                    UserName = user.UserName,
                    IsDeleted = user.IsDeleted,
                    Status = user.Status,
                    Email=user.Email,
                    AvailabilityStatus = user.AvailabilityStatus,
                    Roles = await _userManager.GetRolesAsync(user),
                    DoctorRoom = _mapper.Map<DoctorRoomDetailItemDto>(user.DoctorRoom),
                    Department = _mapper.Map<DepartmentInfoDto>(user.Department),
                    Position = _mapper.Map<PositionInfoDto>(user.Position),
                    Appoinments = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.Appoinments),
                    AppointmentsAsPatient = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.AppointmentsAsPatient),
                    Recipes=_mapper.Map<ICollection<RecipeListItemDto>>(user.Recipes)
                };
                users.Add(userDto);
            }
            return users;
        }
        else
        {
            foreach (var user in await _userManager.Users.Include(d => d.Department).Include(d => d.AppointmentsAsPatient).Include(d => d.Position).Include(d => d.DoctorRoom).Include(d=>d.Appoinments).Where(d=>d.IsDeleted==false).ToListAsync())
            {
                var userDto = new DoctorListItemDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    ImageUrl = _config["Jwt:Issuer"] + "wwwroot/" + user.ImageUrl,
                    Age = user.Age,
                    Gender = user.Gender,
                    StartDate = user.StartDate,
                    EndDate = user.EndDate,
                    UserName = user.UserName,
                    Status = user.Status,
                    Email = user.Email,
                    AvailabilityStatus = user.AvailabilityStatus,
                    Roles = await _userManager.GetRolesAsync(user),
                    Department = _mapper.Map<DepartmentInfoDto>(user.Department),
                    Position = _mapper.Map<PositionInfoDto>(user.Position),
                    Appoinments = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.Appoinments),
                    AppointmentsAsPatient = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.AppointmentsAsPatient),
                    Recipes = _mapper.Map<ICollection<RecipeListItemDto>>(user.Recipes)
                };
                users.Add(userDto);
            }
            return users;
        }
    }

    public async Task<TokenResponseDto> LoginAsync(DoctorLoginDto dto)
    {
        var doctor = await _userManager.FindByNameAsync(dto.UserName);

        if (doctor.IsDeleted)
        {
            throw new LoginFailedException<Doctor>("This user is delete");
        };

        if (doctor == null) throw new LoginFailedException<Doctor>("Username or password is wrong");

        var result = await _userManager.CheckPasswordAsync(doctor, dto.Password);
        if (!result) throw new LoginFailedException<Doctor>("Username or password is wrong");

        return _tokenService.CreateDoctorToken(doctor);
    }

    public async Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.SingleOrDefaultAsync(d => d.RefreshToken == refreshToken);
        if (user is null) throw new NotFoundException<Doctor>();
        if (user.RefreshTokenExpiresDate < DateTime.UtcNow.AddHours(4)) throw new RefreshTokenExpiresIsOldException();
        return _tokenService.CreateDoctorToken(user);
    }

    public async Task RemoveRole(RemoveRoleDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.userName);
        if (user == null) throw new NotFoundException<Doctor>();

        if (!await _roleManager.RoleExistsAsync(dto.roleName)) throw new NotFoundException<IdentityRole>();

        var result = await _userManager.RemoveFromRoleAsync(user,dto.roleName);

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

    public async Task SoftDeleteAsync(string id)
    {
        var doctor = await _userManager.Users.Include(d => d.Appoinments).Include(d=>d.Recipes).FirstOrDefaultAsync(d => d.Id == id);
        if (doctor is null) throw new NotFoundException<Doctor>();
        doctor.IsDeleted = true;
        doctor.EndDate = DateTime.UtcNow.AddHours(4);
        doctor.Status = WorkStatus.leave;

        if(doctor.Appoinments.Count()>0)
        {
            foreach (var app in doctor.Appoinments)
            {
                if (app.AppoinmentDate >= DateTime.Now) throw new DoctorHasAppoinmentException();
            }
        }

        if (doctor.Recipes.Count() > 0) throw new DoctorHasAppoinmentException();

        var result = await _userManager.UpdateAsync(doctor);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserUpdateFailedException<Doctor>(a);
        }
    }

    public async Task ReverteSoftDeleteAsync(string id)
    {
        var doctor = await _userManager.Users.FirstOrDefaultAsync(d => d.Id == id);
        if (doctor is null) throw new NotFoundException<Doctor>();
        doctor.IsDeleted = false;
        doctor.StartDate = DateTime.UtcNow.AddHours(4);
        doctor.EndDate = null;
        doctor.Status = WorkStatus.Active;

        var result = await _userManager.UpdateAsync(doctor);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserUpdateFailedException<Doctor>(a);
        }
    }

    public async Task UpdateAsync(DoctorUpdateDto dto)
    {
        if (string.IsNullOrEmpty(userId)) throw new ArgumentIsNullException();
        if (!await _userManager.Users.AnyAsync(d => d.Id == userId)) throw new NotFoundException<Doctor>();

        var user = await _userManager.FindByIdAsync(userId);

        if (await _appUserManager.Users.AnyAsync(d => (d.UserName == dto.UserName && d.Id != userId) || (d.Email == dto.Email && d.Id != userId))) throw new AppUserIsAlreadyExistException<Doctor>();
        if (await _userManager.Users.AnyAsync(d => (d.UserName == dto.UserName && d.Id != userId) || (d.Email == dto.Email && d.Id != userId))) throw new AppUserIsAlreadyExistException<Doctor>();

        if (dto.ImageFile != null)
        {
            if (user.ImageUrl != null)
            {
                _fileService.Delete(user.ImageUrl);
            }
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
            user.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.DoctorImageRoot);
        }

        var newUser = _mapper.Map(dto, user);
        var result = await _userManager.UpdateAsync(newUser);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserUpdateFailedException<Doctor>(a);
        }
    }

    public async Task UpdateByAdminAsync(string id,DoctorUpdateByAdminDto dto)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.FirstOrDefaultAsync(d=>d.Id==id);
        if (user == null) throw new AppUserNotFoundException<Doctor>();


        if (await _userManager.Users.AnyAsync(d => (d.UserName == dto.UserName && d.Id != id) ||
        (d.Email == dto.Email && d.Id != id))) throw new AppUserIsAlreadyExistException<Doctor>();

        if (await _appUserManager.Users.AnyAsync(d => (d.UserName == dto.UserName && d.Id != id) ||
        (d.Email == dto.Email && d.Id != id))) throw new AppUserIsAlreadyExistException<Doctor>();

        if (dto.ImageFile != null)
        {
            if (user.ImageUrl != null)
            {
                _fileService.Delete(user.ImageUrl);
            }
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
            user.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.DoctorImageRoot);
        }

        if(dto.Status==WorkStatus.leave)
        {
            await SoftDeleteAsync(id);
        }

        if(dto.Status==WorkStatus.Active)
        {
            await ReverteSoftDeleteAsync(id);
        }

        if(dto.PositionId != null)
        {
            var position = await _positionRepository.GetSingleAsync(p=>p.Id==dto.PositionId);
            if (position == null) throw new NotFoundException<Position>();
            if (position.IsDeleted==true) throw new NotFoundException<Position>();
        }

        if(dto.DepartmentId != null)
        {
            var department = await _departmentRepository.GetSingleAsync(d => d.Id == dto.DepartmentId);
            if (department == null) throw new NotFoundException<Department>();
            if (department.IsDeleted==true) throw new NotFoundException<Department>();
        }

        var newUser = _mapper.Map(dto, user);
        var result = await _userManager.UpdateAsync(newUser);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserUpdateFailedException<Doctor>(a);
        }
    }

    public async Task DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.Include(d=>d.Appoinments).Include(d=>d.Recipes).SingleOrDefaultAsync(u => u.Id == id);
        if (user is null) throw new NotFoundException<Doctor>();

        if (user.ImageUrl != null)
        {
            _fileService.Delete(user.ImageUrl);
        }

        if (user.Appoinments.Count() > 0) throw new DoctorHasAppoinmentException();
        if (user.Recipes.Count() > 0) throw new DoctorHasAppoinmentException();

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserDeleteFailedException<Doctor>(a);
        }
    }

    public async Task Logout()
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new AppUserNotFoundException<Doctor>();
        await _signInManager.SignOutAsync();
        user.RefreshToken = null;
        user.RefreshTokenExpiresDate = null;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new LogoutFaileException<Doctor>();
    }

    public async Task AddDoctorRoom(AddDoctorRoomDto dto)
    {
        var room = await _doctorRoomRepository.GetByIdAsync(dto.RoomId);
        if (room is null) throw new NotFoundException<DoctorRoom>();
        if (room.IsDeleted==true) throw new NotFoundException<DoctorRoom>();
        if (room.DoctorRoomStatus==DoctorRoomStatus.Occupied || room.DoctorRoomStatus==DoctorRoomStatus.OutOfService)
            throw new DoctorRoomIsNotAvailableException();

        var user = await _userManager.FindByIdAsync(dto.Id);
        if (user is null) throw new AppUserNotFoundException<Patient>();


        if (user.DoctorRoomId != null) throw new DoctorHavAlreadyRoomException();

        if (user.DepartmentId != room.DepartmentId) throw new DepartmentIdsDifferentException();

        room.Doctor = user;

        room.DoctorRoomStatus = DoctorRoomStatus.Occupied;

        await _doctorRoomRepository.SaveAsync();
    }

    public async Task<DoctorDetailItemDto> GetById(string id, bool takeAll)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentIsNullException();
        if (takeAll)
        {
            var user = await _userManager.Users.Include(d => d.Department).Include(d => d.AppointmentsAsPatient).Include(d => d.Position).Include(d => d.DoctorRoom).Include(d=>d.Appoinments).ThenInclude(a=>a.Patient).SingleOrDefaultAsync(d => d.Id == id);
            if (user is null) throw new AppUserNotFoundException<Doctor>();
            var userDto = new DoctorDetailItemDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                ImageUrl = _config["Jwt:Issuer"] + "wwwroot/" + user.ImageUrl,
                UserName = user.UserName,
                IsDeleted = user.IsDeleted,
                Email = user.Email,
                Password = user.PasswordHash,
                Description = user.Description,
                Status = user.Status,
                Salary = user.Salary,
                Age = user.Age,
                Gender = user.Gender,
                StartDate = user.StartDate,
                AvailabilityStatus = user.AvailabilityStatus,
                Roles = await _userManager.GetRolesAsync(user),
                DoctorRoom = _mapper.Map<DoctorRoomDetailItemDto>(user.DoctorRoom),
                Department = _mapper.Map<DepartmentInfoDto>(user.Department),
                Position = _mapper.Map<PositionInfoDto>(user.Position),
                Appoinments = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.Appoinments),
                AppointmentsAsPatient = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.AppointmentsAsPatient)
            };
            return userDto;
        }
        else
        {
            var user = await _userManager.Users.Include(d => d.Department).Include(d => d.AppointmentsAsPatient).Include(d => d.Position).Include(d => d.DoctorRoom).Include(d => d.Appoinments).ThenInclude(a => a.Patient).Where(d=>d.IsDeleted==false).SingleOrDefaultAsync(d => d.Id == id);
            if (user is null) throw new AppUserNotFoundException<Doctor>();
            var userDto = new DoctorDetailItemDto
            {
                Id=user.Id,
                Name = user.Name,
                Surname = user.Surname,
                ImageUrl = _config["Jwt:Issuer"] + "wwwroot/" + user.ImageUrl,
                UserName = user.UserName,
                IsDeleted = user.IsDeleted,
                Email = user.Email,
                Password=user.PasswordHash,
                Status=user.Status,
                Description=user.Description,
                Salary=user.Salary,
                Age=user.Age,
                Gender=user.Gender,
                StartDate=user.StartDate,
                AvailabilityStatus = user.AvailabilityStatus,
                Roles = await _userManager.GetRolesAsync(user),
                DoctorRoom = _mapper.Map<DoctorRoomDetailItemDto>(user.DoctorRoom),
                Department = _mapper.Map<DepartmentInfoDto>(user.Department),
                Position = _mapper.Map<PositionInfoDto>(user.Position),
                Appoinments = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.Appoinments),
                AppointmentsAsPatient = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.AppointmentsAsPatient)
            };
            return userDto;
        }
    }

    public async Task DoctorStatusUpdater(string id)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentIsNullException();
        var doctor = await _userManager.FindByIdAsync(id);
        if (doctor is null) throw new NotFoundException<Doctor>();

        var currentDate = DateTime.Now;
        var newStatus = DoctorAvailabilityStatus.Available;

        foreach (var app in doctor.Appoinments)
        {
            if(currentDate>=app.AppoinmentDate && currentDate<=app.AppoinmentDate.Add(TimeSpan.FromMinutes(app.Duration)))
            {
                //BackgroundJob.
                doctor.AvailabilityStatus= DoctorAvailabilityStatus.Busy;
                break;
            }
        }

        doctor.AvailabilityStatus = newStatus;

        var result = await _userManager.UpdateAsync(doctor);

        if (!result.Succeeded)
        {
            throw new AppUserUpdateFailedException<Doctor>();
        }
    }

    public async Task<int> Count()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Count();
    }

    public async Task<DoctorDetailItemDto> GetByUsername(string userName, bool takeAll)
    {
        if (string.IsNullOrEmpty(userName)) throw new ArgumentIsNullException();
        if (takeAll)
        {
            var user = await _userManager.Users.Include(d => d.Department).Include(d => d.AppointmentsAsPatient).Include(d => d.Position).Include(d => d.DoctorRoom).Include(d => d.Appoinments).ThenInclude(a => a.Patient).SingleOrDefaultAsync(d => d.UserName == userName);
            if (user is null) throw new AppUserNotFoundException<Doctor>();
            var userDto = new DoctorDetailItemDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                ImageUrl = _config["Jwt:Issuer"] + "wwwroot/" + user.ImageUrl,
                UserName = user.UserName,
                IsDeleted = user.IsDeleted,
                Email = user.Email,
                Password = user.PasswordHash,
                Description = user.Description,
                Status = user.Status,
                Salary = user.Salary,
                Age = user.Age,
                Gender = user.Gender,
                StartDate = user.StartDate,
                AvailabilityStatus = user.AvailabilityStatus,
                Roles = await _userManager.GetRolesAsync(user),
                DoctorRoom = _mapper.Map<DoctorRoomDetailItemDto>(user.DoctorRoom),
                Department = _mapper.Map<DepartmentInfoDto>(user.Department),
                Position = _mapper.Map<PositionInfoDto>(user.Position),
                Appoinments = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.Appoinments),
                AppointmentsAsPatient = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.AppointmentsAsPatient)
            };
            return userDto;
        }
        else
        {
            var user = await _userManager.Users.Include(d => d.Department).Include(d => d.AppointmentsAsPatient).Include(d => d.Position).Include(d => d.DoctorRoom).Include(d => d.Appoinments).ThenInclude(a => a.Patient).Where(d => d.IsDeleted == false).SingleOrDefaultAsync(d => d.Id == userName);
            if (user is null) throw new AppUserNotFoundException<Doctor>();
            var userDto = new DoctorDetailItemDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                ImageUrl = _config["Jwt:Issuer"] + "wwwroot/" + user.ImageUrl,
                UserName = user.UserName,
                IsDeleted = user.IsDeleted,
                Email = user.Email,
                Password = user.PasswordHash,
                Status = user.Status,
                Description = user.Description,
                Salary = user.Salary,
                Age = user.Age,
                Gender = user.Gender,
                StartDate = user.StartDate,
                AvailabilityStatus = user.AvailabilityStatus,
                Roles = await _userManager.GetRolesAsync(user),
                DoctorRoom = _mapper.Map<DoctorRoomDetailItemDto>(user.DoctorRoom),
                Department = _mapper.Map<DepartmentInfoDto>(user.Department),
                Position = _mapper.Map<PositionInfoDto>(user.Position),
                Appoinments = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.Appoinments),
                AppointmentsAsPatient = _mapper.Map<ICollection<AppoinmentListItemDto>>(user.AppointmentsAsPatient)
            };
            return userDto;
        }
    }



}

