using SerenityHospital.Business.Dtos.ContactDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IContactService
{
    Task<IEnumerable<ContactListItemDto>> GetAllAsync();
    Task<IEnumerable<ContactListItemDto>> GetAllAsyncReadUnread();
    Task<ContactDetailItemDto> GetByIdAsync(int id);
    Task CreateAsync(CreateContactDto dto);
    Task DeleteAsync(int id);
    Task<int> Count();
}

