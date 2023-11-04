using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Dtos.DoctorRoom;
using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Business.Dtos.PatientRoomDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Images;
using SerenityHospital.Business.Exceptions.PatientRooms;
using SerenityHospital.Business.Exceptions.Patients;
using SerenityHospital.Business.Extensions;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.Core.Enums;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class PatientRoomService : IPatientRoomService
{
    readonly IPatientRoomRepository _repo;
    readonly IDepartmentRepository _depRepo;
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly UserManager<Patient> _patientUserManager;
    readonly IConfiguration _config;

    public PatientRoomService(IPatientRoomRepository repo, IMapper mapper, IDepartmentRepository depRepo, IFileService fileService, UserManager<Patient> patientUserManager, IConfiguration config)
    {
        _repo = repo;
        _mapper = mapper;
        _depRepo = depRepo;
        _fileService = fileService;
        _patientUserManager = patientUserManager;
        _config = config;
    }

    public async Task CreateAsync(PatientRoomCreateDto dto)
    {
        if (await _repo.IsExistAsync(pr => pr.Number == dto.Number)) throw new ThisRoomNumberIsAlreadyExistException();

        if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
        if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();

        var department =await _depRepo.GetByIdAsync(dto.DepartmentId);
        if (department is null) throw new NotFoundException<Department>();
        if (department.IsDeleted==true) throw new NotFoundException<Department>();

        var patientRoom = _mapper.Map<PatientRoom>(dto);
        patientRoom.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.PatientRoomtImageRoot);
        patientRoom.Status = PatientRoomStatus.Available;

        await _repo.CreateAsync(patientRoom);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<PatientRoom>();
        var entity = await _repo.GetByIdAsync(id,"Patients");
        if (entity is null) throw new NotFoundException<PatientRoom>();

        if (entity.Patients.Count() > 0) throw new PatientRoomNotEmptyException();

        _repo.Delete(entity);
        _fileService.Delete(entity.ImageUrl);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<PatientRoomListItemDto>> GetAllAsync(bool takeAll)
    {
        ICollection<PatientRoomListItemDto> patientRooms = new List<PatientRoomListItemDto>();

        if (takeAll)
        {
            foreach (var room in await _repo.GetAll("Patients").ToListAsync())
            {
                var patientRoom = new PatientRoomListItemDto
                {
                    Id = room.Id,
                    Number=room.Number,
                    Type=room.Type,
                    Status=room.Status,
                    Capacity=room.Capacity,
                    Price=room.Price,
                    ImageUrl = _config["Jwt:Issuer"] + "wwwroot/" + room.ImageUrl,
                    DepartmentId=room.DepartmentId,
                    IsDeleted=room.IsDeleted,
                    Patients=_mapper.Map<ICollection<PatientListItemDto>>(room.Patients)
                };
                patientRooms.Add(patientRoom);
            }
            return patientRooms;
        }
        else
        {
            foreach (var room in await _repo.FindAll(pr => pr.IsDeleted == false, "Patients").ToListAsync())
            {
                var patientRoom = new PatientRoomListItemDto
                {
                    Id = room.Id,
                    Number = room.Number,
                    Type = room.Type,
                    Status = room.Status,
                    Capacity = room.Capacity,
                    Price = room.Price,
                    ImageUrl = _config["Jwt:Issuer"] + "wwwroot/" + room.ImageUrl,
                    DepartmentId = room.DepartmentId,
                    IsDeleted = room.IsDeleted,
                    Patients = _mapper.Map<ICollection<PatientListItemDto>>(room.Patients)
                };
                patientRooms.Add(patientRoom);
            }
            return patientRooms;
        }
    }

    public async Task<PatientRoomDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<PatientRoom>();

        PatientRoom? entity;

        if(takeAll)
        {
            entity =await _repo.GetByIdAsync(id,"Patients");
            if (entity is null) throw new NotFoundException<PatientRoom>();
        }
        else
        {
            entity = await _repo.GetSingleAsync(pr => pr.IsDeleted == false && pr.Id==id, "Patients");
            if (entity is null) throw new NotFoundException<PatientRoom>();
        }

        var map = _mapper.Map<PatientRoomDetailItemDto>(entity);

        map.ImageUrl = _config["Jwt:Issuer"] + "wwwroot/" + map.ImageUrl;

        return map;

    }

    public async Task RevertSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<PatientRoom>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<PatientRoom>();

        _repo.RevertSoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<PatientRoom>();
        var entity = await _repo.GetByIdAsync(id,"Patients");
        if (entity is null) throw new NotFoundException<PatientRoom>();

        if (entity.Patients.Count() > 0) throw new PatientRoomNotEmptyException();

        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id, PatientRoomUpdateDto dto)
    {
        if (id <= 0) throw new NegativeIdException<PatientRoom>();
        var entity = await _repo.GetByIdAsync(id,"Patients");
        if (entity is null) throw new NotFoundException<PatientRoom>();

        if (await _repo.IsExistAsync(pr => pr.Number == dto.Number && pr.Id != id)) throw new ThisRoomNumberIsAlreadyExistException();

        if (dto.ImageFile!=null)
        {
            _fileService.Delete(entity.ImageUrl);

            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
            entity.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.PatientRoomtImageRoot);
        }

        if (dto.DepartmentId != null)
        {
            var department = await _depRepo.GetSingleAsync(d => d.Id == dto.DepartmentId);
            if (department is null) throw new NotFoundException<Department>();
            if (department.IsDeleted==true) throw new NotFoundException<Department>();
        }

        entity.Patients?.Clear();

        if (dto.Patientids != null)
        {
            if (entity.IsDeleted == true) throw new NotFoundException<PatientRoom>();
            foreach (var patientId in dto.Patientids)
            {
                var patient = await _patientUserManager.FindByIdAsync(patientId);
                if (patient is null) throw new NotFoundException<Patient>();

                var patientInOtherPatientRoom = await _repo.IsExistAsync(p => p.Id != id && p.Patients.Any(p => p.Id == patient.Id));
                if (patientInOtherPatientRoom) throw new PatientInOtherPatientRoomException();

                entity.Patients?.Add(patient);
            }
            if (dto.Patientids.Count() > dto.Capacity) throw new PatientRoomCapacityIsFullException();
            if (dto.Patientids.Count() == dto.Capacity)
            {
                entity.Status = PatientRoomStatus.Occupied;
            }

            if (dto.Patientids.Count() < dto.Capacity)
            {
                entity.Status = PatientRoomStatus.Available;
            }

        }
        else
        {
           entity.Status = PatientRoomStatus.Available;
        }
        entity.DepartmentId = dto.DepartmentId;

        _mapper.Map(dto, entity);
        await _repo.SaveAsync();
    }

    public async Task<int> Count()
    {
        var rooms =  _repo.GetAll();
        return rooms.Count();
    }
}

