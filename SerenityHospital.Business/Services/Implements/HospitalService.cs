using AutoMapper;
using SerenityHospital.Business.Dtos.HospitalDtos;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class HospitalService : IHospitalService
{
    readonly IHospitalRepository _repo;
    readonly IMapper _mapper;

    public HospitalService(IHospitalRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HospitalDetailItemDto>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<HospitalDetailItemDto>>(_repo.GetAll());
    }

    public Task UpdateAsync(int id, HospitalUpdateDto dto)
    {
        throw new NotImplementedException();
    }
}

