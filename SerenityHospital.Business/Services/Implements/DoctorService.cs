using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Images;
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
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly IDepartmentRepository _departmentRepository;
    readonly IPositionRepository _positionRepository;

    public DoctorService(UserManager<Doctor> userManager, IMapper mapper, IFileService fileService, IDepartmentRepository departmentRepository, IPositionRepository positionRepository, UserManager<AppUser> appUserManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _fileService = fileService;
        _departmentRepository = departmentRepository;
        _positionRepository = positionRepository;
        _appUserManager = appUserManager;
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
}

