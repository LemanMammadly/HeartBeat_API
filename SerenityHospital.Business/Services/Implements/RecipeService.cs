using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Recipes;
using SerenityHospital.Business.Exceptions.Services;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class RecipeService : IRecipeService
{
    readonly IRecipeRepository _repo;
    readonly IAppoinmentRepository _appoinmentRepo;
    readonly UserManager<Doctor> _docManager;
    readonly UserManager<Patient> _patManager;
    readonly IHttpContextAccessor _context;
    readonly string? userId;
    readonly IMapper _mapper;

    public RecipeService(IRecipeRepository repo, IMapper mapper, IAppoinmentRepository appoinmentRepo, UserManager<Doctor> docManager, UserManager<Patient> patManager, IHttpContextAccessor context)
    {
        _repo = repo;
        _mapper = mapper;
        _appoinmentRepo = appoinmentRepo;
        _docManager = docManager;
        _patManager = patManager;
        _context = context;
        userId = _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task CreateAsync(RecipeCreateDto dto)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentIsNullException();

        if (!await _docManager.Users.AnyAsync(d => d.Id == userId)) throw new AppUserNotFoundException<Doctor>();

        var doctorProf = await _docManager.FindByIdAsync(userId);

        if (await _repo.IsExistAsync(a => a.AppoinmentId == dto.AppoinmentId)) throw new ThisAppoinmentRecipeHasAlreadyExistException();

        var doctor = await _docManager.FindByIdAsync(dto.DoctorId);
        if (doctor is null || doctor.IsDeleted == true) throw new AppUserNotFoundException<Doctor>();

        if(dto.PatientId !=null)
        {
            var patient = await _patManager.FindByIdAsync(dto.PatientId);
            if (patient is null || doctor.IsDeleted == true) throw new AppUserNotFoundException<Patient>();
        }

        var recipe = _mapper.Map<Recipe>(dto);
        await _repo.CreateAsync(recipe);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Recipe>();
        var recipe = await _repo.GetByIdAsync(id);
        if (recipe is null) throw new NotFoundException<Recipe>();


        _repo.Delete(recipe);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<RecipeListItemDto>> GetAllAsync(bool takeAll)
    {
        if(takeAll)
        {
            return _mapper.Map<IEnumerable<RecipeListItemDto>>(_repo.GetAll("Appoinment","Doctor","Patient"));
        }
        else
        {
            return _mapper.Map<IEnumerable<RecipeListItemDto>>(_repo.FindAll(r=>r.IsDeleted==false, "Appoinment", "Doctor", "Patient"));
        }
    }

    public async Task<RecipeDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Recipe>();
        Recipe? entity;

        if(takeAll)
        {
            entity = await _repo.GetByIdAsync(id, "Appoinment", "Doctor", "Patient");
            if (entity is null) throw new NotFoundException<Recipe>();
        }
        else
        {
            entity = await _repo.GetSingleAsync(r=>r.IsDeleted==false && r.Id==id, "Appoinment", "Doctor", "Patient");
            if (entity is null) throw new NotFoundException<Recipe>();
        }

        return _mapper.Map<RecipeDetailItemDto>(entity);
    }

    public async Task ReverteSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Recipe>();
        var recipe = await _repo.GetByIdAsync(id);
        if (recipe is null) throw new NotFoundException<Recipe>();


        _repo.RevertSoftDelete(recipe);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Recipe>();
        var recipe = await _repo.GetByIdAsync(id);
        if (recipe is null) throw new NotFoundException<Recipe>();


        _repo.SoftDelete(recipe);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id, RecipeUpdateDto dto)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentIsNullException();

        if (!await _docManager.Users.AnyAsync(d => d.Id == userId)) throw new AppUserNotFoundException<Doctor>();

        var doctorProf = await _docManager.FindByIdAsync(userId);

        if (id <= 0) throw new NegativeIdException<Recipe>();
        var recipe = await _repo.GetByIdAsync(id);
        if (recipe is null) throw new NotFoundException<Recipe>();

        _mapper.Map(dto, recipe);
        await _repo.SaveAsync();
    }
}

