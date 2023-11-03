using System.Numerics;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SerenityHospital.Business.Dtos.DoctorRoom;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Doctors;
using SerenityHospital.Business.Exceptions.PatientRooms;
using SerenityHospital.Business.Exceptions.Patients;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.Core.Enums;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class DoctorRoomService : IDoctorRoomService
{
    readonly IDoctorRoomRepository _repo;
    readonly UserManager<Doctor> _userManager;
    readonly IDepartmentRepository _departRepo;
    readonly IMapper _mapper;

    public DoctorRoomService(IDoctorRoomRepository repo, IMapper mapper, IDepartmentRepository departRepo, UserManager<Doctor> userManager)
    {
        _repo = repo;
        _mapper = mapper;
        _departRepo = departRepo;
        _userManager = userManager;
    }

    public async Task CreateAsync(DoctorRoomCreateDro dto)
    {
        if (await _repo.IsExistAsync(dr => dr.Number == dto.Number)) throw new ThisRoomNumberIsAlreadyExistException();
        var department = await _departRepo.GetByIdAsync(dto.DepartmentId);
        if (department is null) throw new NotFoundException<Department>();
        if (department.IsDeleted==true) throw new NotFoundException<Department>();
        var doctorRoom = _mapper.Map<DoctorRoom>(dto);
        doctorRoom.DoctorRoomStatus = DoctorRoomStatus.Available;
        await _repo.CreateAsync(doctorRoom);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<DoctorRoom>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<DoctorRoom>();

        if (entity.DoctorRoomStatus == DoctorRoomStatus.Occupied) throw new DoctorRoomIsNotEmptyException();

        _repo.Delete(entity);
        await _repo.SaveAsync();
    }

    public async Task<ICollection<DoctorRoomListItemDto>> GetAllAsync(bool takeAll)
    {
        if(takeAll)
        {
            return _mapper.Map<ICollection<DoctorRoomListItemDto>>(_repo.GetAll("Department","Doctor"));
        }
        else
        {
            return _mapper.Map<ICollection<DoctorRoomListItemDto>>(_repo.FindAll(dr => dr.IsDeleted == false, "Department", "Doctor"));
        }
    }

    public async Task<DoctorRoomDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<DoctorRoom>();
        DoctorRoom? entity;

        if(takeAll)
        {
            entity = await _repo.GetByIdAsync(id,"Department","Doctor");
            if (entity is null) throw new NotFoundException<DoctorRoom>();
        }
        else
        {
            entity = await _repo.GetSingleAsync(dr=>dr.IsDeleted==false && dr.Id==id,"Department","Doctor");
            if (entity is null) throw new NotFoundException<DoctorRoom>();
        }

        return _mapper.Map<DoctorRoomDetailItemDto>(entity);
    }

    public async Task RevertSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<DoctorRoom>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<DoctorRoom>();

        if (entity.DoctorRoomStatus == DoctorRoomStatus.Occupied) throw new DoctorRoomIsNotEmptyException();

        _repo.RevertSoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<DoctorRoom>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<DoctorRoom>();

        if (entity.DoctorRoomStatus == DoctorRoomStatus.Occupied) throw new DoctorRoomIsNotEmptyException();

        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id, DoctorRoomUpdateDto dto)
    {
        if (id <= 0) throw new NegativeIdException<DoctorRoom>();
        var entity = await _repo.GetByIdAsync(id, "Doctor");
        if (entity is null) throw new NotFoundException<DoctorRoom>();


        entity.Department = null;
        entity.Doctor = null;
        entity.DoctorRoomStatus = DoctorRoomStatus.Available;

        if (await _repo.IsExistAsync(dr => dr.Number == dto.Number && dr.Id != id)) throw new ThisRoomNumberIsAlreadyExistException();

        if (dto.DepartmentId != null)
        {
            var department = await _departRepo.GetSingleAsync(d => d.Id == dto.DepartmentId);
            if (department is null) throw new NotFoundException<Department>();
            if (department.IsDeleted == true) throw new NotFoundException<Department>();
            if(dto.DoctorId == null)
            {
             entity.DoctorRoomStatus = DoctorRoomStatus.Available;   
            }
            entity.Department = department;
            _mapper.Map(dto, entity);
            await _repo.SaveAsync();
        }

        if (dto.DoctorId != null)
        {
            var doctor = await _userManager.FindByIdAsync(dto.DoctorId);
            if (doctor is null) throw new NotFoundException<Doctor>();
            if (doctor.IsDeleted == true) throw new NotFoundException<Doctor>();
            if (dto.DepartmentId != doctor.DepartmentId) throw new DepartmentIdsDifferentException();
            if (doctor.DoctorRoomId != null) throw new NotFoundException<Doctor>("Doctor Have Already Room");
            entity.Doctor = doctor;
            entity.DoctorRoomStatus = DoctorRoomStatus.Occupied;
            _mapper.Map(dto, entity);
            await _repo.SaveAsync();
        }


        _mapper.Map(dto, entity);
        await _repo.SaveAsync();
    }


    public async Task<int> Count()
    {
        var rooms = _repo.GetAll();
        return rooms.Count();
    }
}

