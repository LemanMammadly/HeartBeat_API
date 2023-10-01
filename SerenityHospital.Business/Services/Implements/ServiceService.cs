using AutoMapper;
using SerenityHospital.Business.Dtos.ServiceDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Services;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class ServiceService : IServiceService
{
    readonly IServiceRepository _repo;
    readonly IMapper _mapper;

    public ServiceService(IServiceRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task CreateAsync(ServiceCreateDto dto)
    {
        if (await _repo.IsExistAsync(s => s.Name == dto.Name)) throw new ServiceNameIsExistException();
        var service = _mapper.Map<Service>(dto);
        await _repo.CreateAsync(service);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Service>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Service>();

        _repo.Delete(entity);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<ServiceListItemDto>> GetAllAsync(bool takeAll)
    {
        if(takeAll)
        {
            return _mapper.Map<IEnumerable<ServiceListItemDto>>(_repo.GetAll());
        }
        else
        {
            return _mapper.Map<IEnumerable<ServiceListItemDto>>(_repo.FindAll(s=>s.IsDeleted==false));
        }
    }

    public async Task<ServiceDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Service>();
        Service? entity;
        if(takeAll)
        {
            entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException<Service>();
        }
        else
        {
            entity =await _repo.GetSingleAsync(s=>s.IsDeleted==false && s.Id==id);
            if (entity == null) throw new NotFoundException<Service>();
        }

        return _mapper.Map<ServiceDetailItemDto>(entity);
    }

    public async Task ReverteSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Service>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Service>();

        _repo.RevertSoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Service>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Service>();

        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id, ServiceUpdateDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Service>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Service>();

        if (await _repo.IsExistAsync(s => s.Name == dto.Name && s.Id != id)) throw new ServiceNameIsExistException();
        _mapper.Map(dto,entity);
        await _repo.SaveAsync();
    }
}

