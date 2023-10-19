using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Business.Dtos.PatientHistoryDtos;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class PatientHistoryService : IPatientHistoryService
{
    readonly IPatientHistoryRepository _repo;
    readonly IMapper _mapper;
    readonly IAppoinmentRepository _appoinmentRepo;
    readonly IRecipeRepository _recipRepo;


    public PatientHistoryService(IPatientHistoryRepository repo, IMapper mapper,
        IAppoinmentRepository appoinmentRepo, IRecipeRepository recipRepo)
    {
        _repo = repo;
        _mapper = mapper;
        _appoinmentRepo = appoinmentRepo;
        _recipRepo = recipRepo;
    }


    public async Task CreateAsync(PatientHistory patientHistory)
    {
        if (patientHistory == null)
            throw new ArgumentNullException();

        await _repo.CreateAsync(patientHistory);
        await _repo.SaveAsync();
    }

    public async Task<ICollection<PatientHistoryListItemDto>> GetAllAsync()
    {
        return _mapper.Map<ICollection<PatientHistoryListItemDto>>(_repo.GetAll("Recipe","Recipe.Doctor","Recipe.Appoinment","Recipe.Patient"));
    }

    public async Task<PatientHistoryDetailtemDto> GetByIdAsync(int id)
    {
        if (id <= 0) throw new NotFoundException<PatientHistory>();

        PatientHistory? entity;

        entity = await _repo.GetByIdAsync(id, "Recipe", "Recipe.Doctor", "Recipe.Appoinment", "Recipe.Patient");
        if (entity is null) throw new NotFoundException<PatientHistory>();
        return _mapper.Map<PatientHistoryDetailtemDto>(entity);
    }

    public async Task UpdateAsync(PatientHistory patientHistory)
    {
        if (patientHistory == null)
            throw new ArgumentNullException();

        var existpatientHistory = await _repo.GetByIdAsync(patientHistory.Id);

        if(existpatientHistory.Recipe!=null && patientHistory.Recipe!=null)
        {
            existpatientHistory.Recipe.RecipeDesc = patientHistory.Recipe.RecipeDesc;
        }
        await _repo.SaveAsync();
    }
}


