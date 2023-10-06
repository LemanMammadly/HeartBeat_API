using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.AdminstratorDtos;
using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Dtos.PositionDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Images;
using SerenityHospital.Business.Exceptions.Roles;
using SerenityHospital.Business.Extensions;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.Core.Enums;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class DoctorService : IDoctorService
{
    readonly UserManager<Doctor> _userManager;
    readonly UserManager<AppUser> _appUserManager;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly IDepartmentRepository _departmentRepository;
    readonly IPositionRepository _positionRepository;
    readonly ITokenService _tokenService;

    public DoctorService(UserManager<Doctor> userManager, IMapper mapper, IFileService fileService, IDepartmentRepository departmentRepository, IPositionRepository positionRepository, UserManager<AppUser> appUserManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _fileService = fileService;
        _departmentRepository = departmentRepository;
        _positionRepository = positionRepository;
        _appUserManager = appUserManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
    }

    public async Task AddRole(AddRoleDto dto)
    {
        var user =await _userManager.FindByNameAsync(dto.userName);
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

        var position = await _positionRepository.GetByIdAsync(dto.PositionId);
        if (position == null) throw new NotFoundException<Position>();


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
            foreach (var user in await _userManager.Users.Include(d=>d.Department).Include(d=>d.Position).ToListAsync())
            {
                var userDto = new DoctorListItemDto
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    ImageUrl = user.ImageUrl,
                    UserName = user.UserName,
                    Roles = await _userManager.GetRolesAsync(user),
                    Department=_mapper.Map<DepartmentInfoDto>(user.Department),
                    Position=_mapper.Map<PositionInfoDto>(user.Position)
                };
                users.Add(userDto);
            }
            return users;
        }
        else
        {
            foreach (var user in await _userManager.Users.Include(d => d.Department).Include(d => d.Position).Where(d=>d.IsDeleted==false).ToListAsync())
            {
                var userDto = new DoctorListItemDto
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    ImageUrl = user.ImageUrl,
                    UserName = user.UserName,
                    Roles = await _userManager.GetRolesAsync(user),
                    Department = _mapper.Map<DepartmentInfoDto>(user.Department),
                    Position = _mapper.Map<PositionInfoDto>(user.Position)
                };
                users.Add(userDto);
            }
            return users;
        }
    }

    public async Task<TokenResponseDto> LoginAsync(DoctorLoginDto dto)
    {
        var doctor = await _userManager.FindByNameAsync(dto.UserName);
        if (doctor == null) throw new AppUserNotFoundException<Doctor>("Username or password is wrong");

        var result = await _userManager.CheckPasswordAsync(doctor, dto.Password);
        if (!result) throw new LoginFailedException<Doctor>("Username or password is wrong");

        return _tokenService.CreateDoctorToken(doctor);
    }
}

