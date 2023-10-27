using System.Numerics;
using AutoMapper;
using SerenityHospital.Business.Dtos.ServiceDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Departments;
using SerenityHospital.Business.Exceptions.Services;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class ServiceService : IServiceService
{
    readonly IServiceRepository _repo;
    readonly IDepartmentRepository _departRepo;
    readonly IMapper _mapper;

    public ServiceService(IServiceRepository repo, IMapper mapper, IDepartmentRepository departRepo)
    {
        _repo = repo;
        _mapper = mapper;
        _departRepo = departRepo;
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
        var entity = await _repo.GetByIdAsync(id,"Departments");
        if (entity is null) throw new NotFoundException<Service>();

        if (entity.Departments.Count() > 0) throw new ServiceIsNotEmptyException();

        _repo.Delete(entity);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<ServiceListItemDto>> GetAllAsync(bool takeAll)
    {
        if(takeAll)
        {
            var entity = _repo.GetAll("Departments");
            return _mapper.Map<IEnumerable<ServiceListItemDto>>(entity);
        }
        else
        {
            var entity = _repo.FindAll(s => s.IsDeleted == false);
            return _mapper.Map<IEnumerable<ServiceListItemDto>>(entity);
        }
    }

    public async Task<ServiceDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Service>();
        Service? entity;
        if(takeAll)
        {
            entity = await _repo.GetByIdAsync(id,"Departments");
            if (entity == null) throw new NotFoundException<Service>();
        }
        else
        {
            entity =await _repo.GetSingleAsync(s=>s.IsDeleted==false && s.Id==id,"Departments");
            if (entity == null) throw new NotFoundException<Service>();
        }

        return _mapper.Map<ServiceDetailItemDto>(entity);
    }

    public async Task ReverteSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Service>();
        var entity = await _repo.GetByIdAsync(id, "Departments");
        if (entity is null) throw new NotFoundException<Service>();


        if (entity.Departments.Count() > 0) throw new ServiceIsNotEmptyException();

        _repo.RevertSoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Service>();
        var entity = await _repo.GetByIdAsync(id, "Departments");
        if (entity is null) throw new NotFoundException<Service>();


        if (entity.Departments.Count() > 0) throw new ServiceIsNotEmptyException();

        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id, ServiceUpdateDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Service>();
        var entity = await _repo.GetByIdAsync(id,"Departments");
        if (entity is null) throw new NotFoundException<Service>();

        if (await _repo.IsExistAsync(s => s.Name == dto.Name && s.Id != id)) throw new ServiceNameIsExistException();

        entity.Departments?.Clear();
        if (dto.DepartmentIds != null)
        {
            foreach (var itemId in dto.DepartmentIds)
            {
                var item = await _departRepo.GetByIdAsync(itemId);
                if (item is null) throw new NotFoundException<Department>();
                if (item.IsDeleted == true) throw new NotFoundException<Department>();

                var isDepartmentInOtherService = await _repo.IsExistAsync(s => s.Id != id && s.Departments.Any(d => d.Id == item.Id));
                if (isDepartmentInOtherService) throw new DepartmentIsInOtherSereviceException();

                entity.Departments?.Add(item);
            }
        }

        _mapper.Map(dto,entity);
        await _repo.SaveAsync();
    }

    public async Task<int> Count()
    {
        var rooms = _repo.GetAll();
        return rooms.Count();
    }
}

