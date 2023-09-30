using AutoMapper;
using SerenityHospital.Business.Dtos.HospitalDtos;
using SerenityHospital.Business.Exceptions.Common;
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

    public async Task UpdateAsync(int id, HospitalUpdateDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Hospital>();
        var entity =await _repo.GetByIdAsync(id);
        if (entity == null) throw new NotFoundException<Hospital>();

        _mapper.Map(dto, entity);
        await _repo.SaveAsync();
    }
}

