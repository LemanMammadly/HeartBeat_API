using Microsoft.Extensions.DependencyInjection;
using SerenityHospital.Business.Services.Implements;
using SerenityHospital.Business.Services.Interfaces;

namespace SerenityHospital.Business;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IHospitalService, HospitalService>();
    }
}

