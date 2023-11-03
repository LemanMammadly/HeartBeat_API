﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Business.Dtos.AppoinmentDtos;
using SerenityHospital.Business.Exceptions.Appoinments;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.Core.Enums;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class AppoinmentService : IAppoinmentService
{
    readonly UserManager<Patient> _patUserManager;
    readonly UserManager<Doctor> _docUserManager;
    readonly IDepartmentRepository _deprepo;
    readonly IPatientHistoryRepository _patientHistoryRepository;
    readonly IHttpContextAccessor _context;
    readonly IRecipeRepository _recipRepo;
    readonly string? userId;
    readonly IAppoinmentRepository _repo;
    readonly IMapper _mapper;

    public AppoinmentService(IHttpContextAccessor context, UserManager<Patient> userManager, IAppoinmentRepository repo, UserManager<Doctor> docUserManager, IMapper mapper, IPatientHistoryRepository patientHistoryRepository, IRecipeRepository recipRepo, IDepartmentRepository deprepo)
    {
        _repo = repo;
        _context = context;
        userId = _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _patUserManager = userManager;
        _docUserManager = docUserManager;
        _mapper = mapper;
        _patientHistoryRepository = patientHistoryRepository;
        _recipRepo = recipRepo;
        _deprepo = deprepo;
    }

    public async Task CreateAsync(AppoinmentCreateDto dto)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentIsNullException();

        if (!await _patUserManager.Users.AnyAsync(d => d.Id == userId) &&
            !await _docUserManager.Users.AnyAsync(d => d.Id == userId))
            throw new AppUserNotFoundException<Patient>();

        var patient = await _patUserManager.FindByIdAsync(userId);
        var doctor = await _docUserManager.FindByIdAsync(userId);

        if (doctor != null && dto.DoctorId == doctor.Id)
            throw new DoctorCannotAppoinmentThemselves();

        if(dto.DepartmentId != null)
        {
            var department = await _deprepo.GetSingleAsync(d=>d.Id== dto.DepartmentId);
            if (department is null || department.IsDeleted == true) throw new NotFoundException<Department>();
        }

        var targetDoctor = await _docUserManager.FindByIdAsync(dto.DoctorId);
        if (targetDoctor == null || targetDoctor.IsDeleted)
            throw new AppUserNotFoundException<Nurse>();

        var appoinmentStart = dto.AppoinmentDate;
        var appoinmentEnd = dto.AppoinmentDate.AddMinutes(20);

        var conflictDoc = await _repo.IsExistAsync(a => a.AppoinmentAsDoctorId == dto.DoctorId &&
        (dto.AppoinmentDate <= a.AppoinmentDate.AddMinutes(a.Duration) &&
            dto.AppoinmentDate >= a.AppoinmentDate));

        if (conflictDoc)
            throw new ConflictingAppointmentException();

        var conflict = await _repo.IsExistAsync(a => a.DoctorId == dto.DoctorId &&
            (dto.AppoinmentDate <= a.AppoinmentDate.AddMinutes(a.Duration) &&
            dto.AppoinmentDate >= a.AppoinmentDate));

        if (conflict)
            throw new ConflictingAppointmentException();

       if(patient!=null)
       {
            var conflictPatient = await _repo.IsExistAsync(a => a.DoctorId != dto.DoctorId
           && a.PatientId == userId &&
           (dto.AppoinmentDate <= a.AppoinmentDate.AddMinutes(a.Duration)
           && dto.AppoinmentDate >= a.AppoinmentDate));
            if (conflictPatient)
                throw new ConflictingAppointmentException();
       }


       if(doctor!=null)
        {
            var conflictDoctor = await _repo.IsExistAsync(a => a.DoctorId != dto.DoctorId
            && a.AppoinmentAsDoctorId == userId &&
            (dto.AppoinmentDate <= a.AppoinmentDate.AddMinutes(a.Duration)
            && dto.AppoinmentDate >= a.AppoinmentDate));
            if (conflictDoctor)
                throw new ConflictingAppointmentException();
        }

        var appoinment = _mapper.Map<Appoinment>(dto);

        if(doctor!=null)
        {
            appoinment.AppoinmentAsDoctorId = doctor.Id;
        }

        if(patient!=null)
        {
            appoinment.PatientId = patient.Id;
        }

        appoinment.Doctor = targetDoctor;


        await _repo.CreateAsync(appoinment);
        await _repo.SaveAsync();
    }


    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Appoinment>();
        var appoinment = await _repo.GetByIdAsync(id, "Recipe");
        if (appoinment is null) throw new NotFoundException<Appoinment>();

        if (appoinment.Recipe != null) throw new AppoinmentCouldntBeDeleteException("Appoinment have recipe.Couldnt delete");

        var now = DateTime.Now;

        if (now >= appoinment.AppoinmentDate && now <= appoinment.AppoinmentDate.AddMinutes(appoinment.Duration)) throw new AppoinmentCouldntBeDeleteException("Now appoinment date.Couldnt delete");

        _repo.Delete(appoinment);
        await _repo.SaveAsync();
    }

    public async Task<ICollection<AppoinmentListItemDto>> GetAllAsync(bool takeAll)
    {
       if(takeAll)
       {
            return _mapper.Map<ICollection<AppoinmentListItemDto>>(_repo.GetAll("Doctor","Patient","Recipe",
                "AppoinmentAsDoctor", "Doctor.Position", "Doctor.Department"));
       }
       else
       {
            return _mapper.Map<ICollection<AppoinmentListItemDto>>(_repo.FindAll(a => a.IsDeleted == false,"Doctor", "Recipe",
                "Patient", "AppoinmentAsDoctor", "Doctor.Position", "Doctor.Department"));
       }
    }

    public async Task<AppoinmentDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Appoinment>();
        Appoinment? entity;
        if(takeAll)
        {
            entity = await _repo.GetByIdAsync(id, "Doctor", "Patient", "Recipe",
                "AppoinmentAsDoctor", "Doctor.Position", "Doctor.Department");
            if (entity is null) throw new NotFoundException<Appoinment>();
        }
        else
        {
            entity = await _repo.GetSingleAsync(a => a.IsDeleted == false && a.Id == id, "Doctor", "Patient", "Recipe",
                "AppoinmentAsDoctor", "Doctor.Position", "Doctor.Department");
            if (entity is null) throw new NotFoundException<Appoinment>();
        }
        return _mapper.Map<AppoinmentDetailItemDto>(entity);
    }

    public async Task ReverteSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Appoinment>();
        var appoinment = await _repo.GetByIdAsync(id);
        if (appoinment is null) throw new NotFoundException<Appoinment>();

        _repo.RevertSoftDelete(appoinment);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Appoinment>();
        var appoinment = await _repo.GetByIdAsync(id, "Recipe");
        if (appoinment is null) throw new NotFoundException<Appoinment>();

        if (appoinment.Recipe != null) throw new AppoinmentCouldntBeDeleteException("Appoinment have recipe.Couldnt delete");

        var now = DateTime.Now;

        if (now >= appoinment.AppoinmentDate && now <= appoinment.AppoinmentDate.AddMinutes(appoinment.Duration))
            throw new AppoinmentCouldntBeDeleteException();

        _repo.SoftDelete(appoinment);
        await _repo.SaveAsync();
    }


    public async Task UpdateAsync(int id, AppoinmentUpdateDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Appoinment>();
        var appoinment = await _repo.GetByIdAsync(id);
        if (appoinment is null) throw new NotFoundException<Appoinment>();
        if (appoinment.IsDeleted == true) throw new NotFoundException<Appoinment>();

        var doctor = await _docUserManager.FindByIdAsync(dto.DoctorId);
        if (doctor is null) throw new NotFoundException<Doctor>();
        if (doctor.IsDeleted == true) throw new NotFoundException<Doctor>("hekim hekim");


        if (dto.PatientId != null)
        {
            var patient = await _patUserManager.FindByIdAsync(dto.PatientId);
            if (patient is null) throw new NotFoundException<Patient>();
        }

        if (dto.AppoinmentAsDoctorId != null)
        {
            var patientAsDoctor = await _docUserManager.FindByIdAsync(dto.AppoinmentAsDoctorId);
            if (patientAsDoctor is null) throw new NotFoundException<Doctor>();
        }

        if (dto.DepartmentId != null)
        {
            var department = await _deprepo.GetSingleAsync(d => d.Id == dto.DepartmentId);
            if (department is null || department.IsDeleted == true) throw new NotFoundException<Department>();
        }

        if (dto.AppoinmentAsDoctorId != null && dto.AppoinmentAsDoctorId == doctor.Id)
            throw new DoctorCannotAppoinmentThemselves();

        var appoinmentStart = dto.AppoinmentDate;
        var appoinmentEnd = dto.AppoinmentDate.AddMinutes(dto.Duration);

        if (dto.PatientId != null)
        {
            var conflictPatient = await _repo.IsExistAsync(a => a.DoctorId != dto.DoctorId && a.PatientId == dto.PatientId && ((dto.AppoinmentDate >= a.AppoinmentDate && dto.AppoinmentDate <= a.AppoinmentDate.AddMinutes(a.Duration))  && a.Id != id));
            if (conflictPatient) throw new ConflictingAppointmentException();
        }

        if (dto.AppoinmentAsDoctorId != null)
        {
            var conflictAsDoctorPatient = await _repo.IsExistAsync(a => a.DoctorId != dto.DoctorId && a.AppoinmentAsDoctorId == dto.AppoinmentAsDoctorId && (((dto.AppoinmentDate >= a.AppoinmentDate && dto.AppoinmentDate <= a.AppoinmentDate.AddMinutes(a.Duration)) && a.Id != id)));
            if (conflictAsDoctorPatient) throw new ConflictingAppointmentException();
        }

        var conflictDoc = await _repo.IsExistAsync(a => a.AppoinmentAsDoctorId == dto.DoctorId &&
        (dto.AppoinmentDate <= a.AppoinmentDate.AddMinutes(a.Duration) &&
            dto.AppoinmentDate >= a.AppoinmentDate));

        if (conflictDoc)
            throw new ConflictingAppointmentException();

        var conflict = await _repo.IsExistAsync
            (a => a.DoctorId == dto.DoctorId &&
            ((dto.AppoinmentDate>=a.AppoinmentDate  && dto.AppoinmentDate <= a.AppoinmentDate.AddMinutes(a.Duration) && a.Id != id)));

        if (conflict) throw new ConflictingAppointmentException();

        _mapper.Map(dto, appoinment);
        await _repo.SaveAsync();
    }

    public async Task<int> Count()
    {
        var rooms = _repo.GetAll();
        return rooms.Count();
    }
}

