using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SerenityHospital.Business.ExternalServices.Implements;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Implements;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.Business.Application;
using SerenityHospital.Business.Contracts;

namespace SerenityHospital.Business;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IHospitalService, HospitalService>();
        services.AddScoped<ISettingService, SettingService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<IPatientRoomService, PatientRoomService>();
        services.AddScoped<IAdminstratorService, AdminstratorService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<INurseService, NurseService>();
        services.AddScoped<IDoctorRoomService, DoctorRoomService>();
        services.AddScoped<IAppoinmentService, AppoinmentService>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IPatientHistoryService, PatientHistoryService>();
        services.AddSingleton<IEmailSenderService, EmailSenderService>();
    }
}

