using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.API;
using SerenityHospital.Business.Services.Implements;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;

public class Startup
{
    public IConfiguration Configuration { get; }
    public UserManager<Doctor> _userManager { get; set; }

    public Startup(IConfiguration configuration, UserManager<Doctor> userManager)
    {
        Configuration = configuration;
        _userManager = userManager;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
    }

    public async Task Configure(IApplicationBuilder app)
    {
        var doctors = await _userManager.Users.ToListAsync();

        foreach (var item in doctors)
        {
            RecurringJob.AddOrUpdate<IDoctorService>(x => x.DoctorStatusUpdater(item.Id), Cron.MinuteInterval(1));


            BackgroundJob.Enqueue<DoctorService>(x => x.DoctorStatusUpdater(item.Id));
        }
    }
}
