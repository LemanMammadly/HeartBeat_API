using Microsoft.EntityFrameworkCore;
using SerenityHospital.DAL.Contexts;
using SerenityHospital.DAL;
using SerenityHospital.Business.Profiles;
using SerenityHospital.API.Helpers;
using SerenityHospital.Business;
using FluentValidation.AspNetCore;
using SerenityHospital.Business.Services.Implements;
using SerenityHospital.Business.Constants;
using Microsoft.AspNetCore.Identity;
using System.Numerics;
using SerenityHospital.Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using SerenityHospital.Business.ExternalServices.Implements;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using System.Security.Principal;
using SerenityHospital.API;
using Microsoft.Extensions.Options;
using Stripe;
using Hangfire;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddPersistenceServices(builder.Configuration);

//confirm email
builder.Services.AddTransient<UserManager<AppUser>>();
builder.Services.AddTransient<UserManager<Doctor>>();
builder.Services.AddTransient<UserManager<Patient>>();
builder.Services.AddTransient<UserManager<Adminstrator>>();
builder.Services.AddTransient<UserManager<Nurse>>();


builder.Services.AddFluentValidation(opt =>
{
    opt.RegisterValidatorsFromAssemblyContaining<HospitalService>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


//builder.Services.AddHangfire(configuration => configuration
//    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
//    .UseSimpleAssemblyNameTypeSerializer()
//    .UseRecommendedSerializerSettings()
//    .UseSqlServerStorage(builder.Configuration.GetConnectionString("Default")));

//builder.Services.AddHangfireServer();


//stripe
builder.Services.AddStripeInfrastructure(builder.Configuration);


//add email config
var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IEmailServiceSender, EmailServiceSender>();

//builder.Services.AddMailKit(optionBuilder =>
//{
//    optionBuilder.UseMailKit(new MailKitOptions
//    {
//        Server = "smtp.gmail.com",
//        Port = 587,
//        SenderName = "Your Name",
//        SenderEmail = "leman.mammadly23@gmail.com",
//        Account = "leman.mammadly23@gmail.com",
//        Password = "naehtslelvpxpaox",
//        Security = true // Use SSL/TLS
//    });
//});



//StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("StripeSettings:SecretKey");

//var options = new PaymentIntentCreateOptions
//{
//    Amount = 500,
//    Currency = "gbp",
//    PaymentMethod = "pm_card_visa",
//};
//var service = new PaymentIntentService();
//service.Create(options);


builder.Services.AddAutoMapper(typeof(HospitalMappingProfile).Assembly);

builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        LifetimeValidator = (_, expires, token, _) => token != null ? DateTime.UtcNow.AddHours(4) < expires : false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"]))
    };
}).AddIdentityCookies();
builder.Services.AddAuthorization();


builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    });
}

app.UseHttpsRedirection();

//app.UseHangfireDashboard();
//app.MapHangfireDashboard();

app.UseAuthentication();
app.UseAuthorization();

//app.UseCustomExceptionHandler();

app.MapControllers();

RootConstant.Root = builder.Environment.WebRootPath;

app.Run();

