using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Business.Dtos.ContactDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class ContactService : IContactService
{
    readonly IContactRepository _repo;
    readonly IMapper _mapper;

    public ContactService(IContactRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<int> Count()
    {
        var contacts = await _repo.GetAll().ToListAsync();
        return contacts.Count();
    }

    public async Task CreateAsync(CreateContactDto dto)
    {
        var contact = _mapper.Map<Contact>(dto);
        await _repo.CreateAsync(contact);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Contact>();
        var contact = await _repo.GetByIdAsync(id);
        if (contact == null) throw new NotFoundException<Contact>();
        _repo.Delete(contact);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<ContactListItemDto>> GetAllAsync()
    {
        var messages = _mapper.Map<IEnumerable<ContactListItemDto>>(await _repo.GetAll().ToListAsync());
        foreach (var message in messages)
        {
            message.IsRead = true;
        }
        await _repo.SaveAsync();
        return messages;
    }

    public async Task<IEnumerable<ContactListItemDto>> GetAllAsyncReadUnread()
    {
        var allContacts = await _repo.GetAll().ToListAsync();
        var allContactDtos = _mapper.Map<IEnumerable<ContactListItemDto>>(allContacts);

        var seenContactIds = new HashSet<int>();

        foreach (var contact in allContacts)
        {
            if (!seenContactIds.Contains(contact.Id))
            {
                contact.IsRead = true;
                seenContactIds.Add(contact.Id);
            }
        }

        await _repo.SaveAsync();

        return allContactDtos;
    }


    public async Task<ContactDetailItemDto> GetByIdAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Contact>();
        var contact = await _repo.GetByIdAsync(id);
        if (contact == null) throw new NotFoundException<Contact>();
        contact.IsRead = true;
        return _mapper.Map<ContactDetailItemDto>(contact);
    }
}

