using System.Security.Claims;
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
    readonly IHttpContextAccessor _context;
    readonly string? userId;
    readonly IAppoinmentRepository _repo;
    readonly IMapper _mapper;

    public AppoinmentService(IHttpContextAccessor context, UserManager<Patient> userManager, IAppoinmentRepository repo, UserManager<Doctor> docUserManager, IMapper mapper)
    {
        _repo = repo;
        _context = context;
        userId = _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _patUserManager = userManager;
        _docUserManager = docUserManager;
        _mapper = mapper;
    }

    public async Task CreateAsync(AppoinmentCreateDto dto)
    {
        if (string.IsNullOrEmpty(userId)) throw new ArgumentIsNullException();
        if (!await _patUserManager.Users.AnyAsync(d => d.Id == userId)) throw new AppUserNotFoundException<Patient>();

        var user = await _patUserManager.FindByIdAsync(userId);

        var doctor = await _docUserManager.Users.Include(d => d.Appoinments).FirstOrDefaultAsync(d => d.Id == dto.DoctorId);

        if (doctor == null) throw new AppUserNotFoundException<Doctor>();
        if (doctor.IsDeleted==true) throw new AppUserNotFoundException<Doctor>();

        var appoinmentStart = dto.AppoinmentDate;
        var appoinmentEnd = dto.AppoinmentDate.AddMinutes(20);

        var conflict = await _repo.IsExistAsync(a=>a.DoctorId==dto.DoctorId && (dto.AppoinmentDate<= a.AppoinmentDate.AddMinutes(20) && dto.AppoinmentDate >=a.AppoinmentDate ));
        if (conflict) throw new ConflictingAppointmentException();

        var conflictPatient = await _repo.IsExistAsync(a => a.DoctorId != dto.DoctorId && a.PatientId==userId && (dto.AppoinmentDate <= a.AppoinmentDate.AddMinutes(20) && dto.AppoinmentDate >= a.AppoinmentDate));
        if (conflictPatient) throw new ConflictingAppointmentException();

        var appoinment = _mapper.Map<Appoinment>(dto);

        appoinment.PatientId = userId;
        appoinment.Doctor = doctor;
        appoinment.Status = AppoinmentStatus.Pending;
        appoinment.Doctor.AvailabilityStatus = DoctorAvailabilityStatus.Busy;

        await _repo.CreateAsync(appoinment);
        await _repo.SaveAsync();
    }

    public async Task<ICollection<AppoinmentListItemDto>> GetAllAsync(bool takeAll)
    {
       if(takeAll)
       {
            return _mapper.Map<ICollection<AppoinmentListItemDto>>(_repo.GetAll("Doctor","Patient", "Doctor.Position", "Doctor.Department"));
       }
       else
       {
            return _mapper.Map<ICollection<AppoinmentListItemDto>>(_repo.FindAll(a => a.IsDeleted == false,"Doctor", "Patient", "Doctor.Position", "Doctor.Department"));
       }
    }
}

