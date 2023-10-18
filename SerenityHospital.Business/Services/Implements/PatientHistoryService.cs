using AutoMapper;
using SerenityHospital.Business.Dtos.PatientDtos;
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


    public PatientHistoryService(IPatientHistoryRepository repo, IMapper mapper,
        IAppoinmentRepository appoinmentRepo)
    {
        _repo = repo;
        _mapper = mapper;
        _appoinmentRepo = appoinmentRepo;
    }


    public async Task CreateAsync(PatientHistory patientHistory)
    {
        if (patientHistory == null)
            throw new ArgumentNullException();

        await _repo.CreateAsync(patientHistory);
        await _repo.SaveAsync();
    }

    public async Task<ICollection<PatientListItemDto>> GetAllAsync()
    {
        return _mapper.Map<ICollection<PatientListItemDto>>(_repo.GetAll("Recipe", "Patient","Doctor"));
    }

    public async Task<PatientDetailItemDto> GetByIdAsync(int id)
    {
        if (id <= 0) throw new NotFoundException<PatientHistory>();
        PatientHistory? entity;

        entity = await _repo.GetByIdAsync(id, "Recipe", "Patient", "Doctor");
        return _mapper.Map<PatientDetailItemDto>(entity);
    }
}


