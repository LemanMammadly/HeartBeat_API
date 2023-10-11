using AutoMapper;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Departments;
using SerenityHospital.Business.Exceptions.Images;
using SerenityHospital.Business.Exceptions.PatientRooms;
using SerenityHospital.Business.Extensions;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class DepartmentService : IDepartmentService
{
    readonly IDepartmentRepository _repo;
    readonly IPatientRoomRepository _patientRepo;
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly IServiceRepository _serviceRepo;

    public DepartmentService(IDepartmentRepository repo, IMapper mapper, IFileService fileService, IServiceRepository serviceRepo, IPatientRoomRepository patientRepo)
    {
        _repo = repo;
        _mapper = mapper;
        _fileService = fileService;
        _serviceRepo = serviceRepo;
        _patientRepo = patientRepo;
    }

    public async Task CreateAsync(DepartmentCreateDto dto)
    {
        if (await _repo.IsExistAsync(d => d.Name == dto.Name)) throw new DepartmentNameIsExistException();

        if (!dto.IconFile.IsSizeValid(3)) throw new SizeNotValidException();
        if (!dto.IconFile.IsTypeValid("image")) throw new TypeNotValidException();

        var service = await _serviceRepo.GetByIdAsync(dto.ServiceId);
        if (service == null) throw new NotFoundException<Service>();

        var department = _mapper.Map<Department>(dto);
        department.IconUrl = await _fileService.UploadAsync(dto.IconFile, RootConstant.DepartmentImageRoot);
        await _repo.CreateAsync(department);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Department>();
        var entity = await _repo.GetByIdAsync(id,"PatientRooms","Doctors");
        if (entity is null) throw new NotFoundException<Department>();


        if (entity.PatientRooms.Count() > 0) throw new DepartmentIsNotEmptyException();
        if (entity.Doctors.Count() > 0) throw new DepartmentIsNotEmptyException();

        _repo.Delete(entity);
        _fileService.Delete(entity.IconUrl);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<DepartmentListItemDto>> GetAllAsync(bool takeAll)
    {
        if(takeAll)
        {
            return _mapper.Map<IEnumerable<DepartmentListItemDto>>(_repo.GetAll("PatientRooms","Doctors","Doctors.Position"));
        }
        else
        {
            return _mapper.Map<IEnumerable<DepartmentListItemDto>>(_repo.FindAll(d => d.IsDeleted == false,"PatientRooms","Doctors","Doctors.Position"));
        }
    }

    public async Task<DepartmentDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Department>();
        Department? entity;
        if(takeAll)
        {
            entity = await _repo.GetByIdAsync(id,"PatientRooms","Doctors","Doctors.Position");
            if (entity is null) throw new NotFoundException<Department>();
        }
        else
        {
            entity = await _repo.GetSingleAsync(d => d.Id == id && d.IsDeleted == false, "PatientRooms", "Doctors", "Doctors.Position");
            if (entity is null) throw new NotFoundException<Department>();
        }

        return _mapper.Map<DepartmentDetailItemDto>(entity);
    }

    public async Task ReverteSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Department>();
        var entity = await _repo.GetByIdAsync(id,"PatientRooms");
        if (entity is null) throw new NotFoundException<Department>();

        if (entity.PatientRooms.Count() > 0) throw new DepartmentIsNotEmptyException();

        _repo.RevertSoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Department>();
        var entity = await _repo.GetByIdAsync(id,"PatientRooms","Doctors");
        if (entity is null) throw new NotFoundException<Department>();

        if (entity.PatientRooms.Count() > 0) throw new DepartmentIsNotEmptyException();
        if (entity.Doctors.Count() > 0) throw new DepartmentIsNotEmptyException();

        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id, DepartmentUpdateDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Department>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Department>();

        if (await _repo.IsExistAsync(d => d.Name == dto.Name && d.Id != id)) throw new DepartmentNameIsExistException();
        if(dto.IconFile!=null)
        {
            _fileService.Delete(entity.IconUrl);
            if (!dto.IconFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.IconFile.IsTypeValid("image")) throw new TypeNotValidException();
            entity.IconUrl = await _fileService.UploadAsync(dto.IconFile, RootConstant.DepartmentImageRoot);
        }

        entity.PatientRooms?.Clear();
        if(dto.PatientRoomIds !=null)
        {
            foreach (var itemId in dto.PatientRoomIds)
            {
                var patientRoom = await _patientRepo.GetByIdAsync(itemId);
                if (patientRoom is null) throw new NotFoundException<PatientRoom>();
                var isPatientRoomInOtherDepartment = await _repo.IsExistAsync(d => d.Id != id && d.PatientRooms.Any(d => d.Id == patientRoom.Id));
                if (isPatientRoomInOtherDepartment) throw new PatientRoomInOtherDepartmentException();
                entity.PatientRooms?.Add(patientRoom);
            }
        }

        var service = await _serviceRepo.GetByIdAsync(dto.ServiceId);
        if (service == null) throw new NotFoundException<Service>();

        entity.ServiceId = dto.ServiceId;

        _mapper.Map(dto, entity);
        await _repo.SaveAsync();
    }
}

