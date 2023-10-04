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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddFluentValidation(opt =>
{
    opt.RegisterValidatorsFromAssemblyContaining<HospitalService>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
});


builder.Services.AddIdentity<Adminstrator, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddAutoMapper(typeof(HospitalMappingProfile).Assembly);

builder.Services.AddRepositories();
builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseCustomExceptionHandler();

app.MapControllers();

RootConstant.Root = builder.Environment.WebRootPath;

app.Run();

