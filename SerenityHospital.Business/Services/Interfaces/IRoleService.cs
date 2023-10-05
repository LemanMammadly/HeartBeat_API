using Microsoft.AspNetCore.Identity;
using SerenityHospital.Business.Dtos.RoleDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<IdentityRole>> GetAllAsync();
    Task<string> GetByIdAsync(string id);
    Task CreateAsync(string name);
    Task UpdateAsync(string id, string name);
    Task RemoveAsync(string id);
}

