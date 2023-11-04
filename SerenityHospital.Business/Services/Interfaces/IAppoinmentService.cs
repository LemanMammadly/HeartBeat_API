﻿using SerenityHospital.Business.Dtos.AppoinmentDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IAppoinmentService
{
    Task<ICollection<AppoinmentListItemDto>> GetAllAsync(bool takeAll);
    Task<AppoinmentDetailItemDto> GetByIdAsync(int id, bool takeAll);
    Task<ICollection<AppoinmentListItemDto>> GetByUsernameAsync(string userName);
    Task CreateAsync(AppoinmentCreateDto dto);
    Task UpdateAsync(int id,AppoinmentUpdateDto dto);
    Task DeleteAsync(int id);
    Task<int> Count();
    Task SoftDeleteAsync(int id);
    Task ReverteSoftDeleteAsync(int id);
    Task AcceptAppoinment(int id);
    Task RejectAppoinment(int id);
}

