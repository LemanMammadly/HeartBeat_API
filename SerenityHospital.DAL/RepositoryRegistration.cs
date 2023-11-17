using Microsoft.Extensions.DependencyInjection;
using SerenityHospital.DAL.Repositories.Implements;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.DAL;

public static class RepositoryRegistration
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IHospitalRepository, HospitalRepository>();
        services.AddScoped<ISettingRepository, SettingRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<IPatientRoomRepository, PatientRoomRepository>();
        services.AddScoped<IDoctorRoomRepository, DoctorRoomRepository>();
        services.AddScoped<IAppoinmentRepository, AppoinmentRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IPatientHistoryRepository, PatientHistoryRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
    }
}

