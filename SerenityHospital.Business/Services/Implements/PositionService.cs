using AutoMapper;
using SerenityHospital.Business.Dtos.PositionDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class PositionService : IPositionService
{
    readonly IPositionRepository _repo;
    readonly IMapper _mapper;

    public PositionService(IPositionRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task CreateAsync(PositionCreateDto dto)
    {
        if(await _repo.IsExistAsync(p=>p.Name==dto.Name)) throw new NameIsAlreadyExistException<Position>();

        var position = _mapper.Map<Position>(dto);
        await _repo.CreateAsync(position);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Position>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Position>();

        _repo.Delete(entity);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<PositionListItemDto>> GetAllAsync(bool takeAll)
    {
        if(takeAll)
        {
            return _mapper.Map<IEnumerable<PositionListItemDto>>(_repo.GetAll());
        }
        else
        {
            return _mapper.Map<IEnumerable<PositionListItemDto>>(_repo.FindAll(p => p.IsDeleted == false));
        }
    }

    public async Task<PositionDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Position>();

        Position? entity;

        if(takeAll)
        {
            entity = await _repo.GetByIdAsync(id);
            if (entity is null) throw new NotFoundException<Position>();
        }
        else
        {
            entity = await _repo.GetSingleAsync(p => p.IsDeleted == false);
            if (entity is null) throw new NotFoundException<Position>();
        }

        return _mapper.Map<PositionDetailItemDto>(entity);
    }

    public async Task ReverteSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Position>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Position>();

        _repo.RevertSoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Position>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Position>();

        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id, PositionUpdateDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Position>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Position>();

        if (await _repo.IsExistAsync(p => p.Name == dto.Name && p.Id != id)) throw new NameIsAlreadyExistException<Position>();

        _mapper.Map(dto, entity);
        await _repo.SaveAsync();
    }
}

