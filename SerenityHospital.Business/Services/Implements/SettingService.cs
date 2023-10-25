using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.SettingDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Images;
using SerenityHospital.Business.Extensions;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class SettingService : ISettingService
{
    readonly IConfiguration _config;
    readonly ISettingRepository _repo;
    readonly IMapper _mapper;
    readonly IFileService _fileService;

    public SettingService(ISettingRepository repo, IMapper mapper, IFileService fileService, IConfiguration config)
    {
        _repo = repo;
        _mapper = mapper;
        _fileService = fileService;
        _config = config;
    }

    public async Task CreateAsync(SettingCreateDto dto)
    {
        var existSetting = await _repo.GetFirstAsync();

        if (existSetting != null) throw new SettingIsExistException();

        if (!dto.HeaderLogoFile.IsTypeValid("image") || !dto.FooterLogoFile.IsTypeValid("image")) throw new TypeNotValidException();
        if (!dto.FooterLogoFile.IsSizeValid(3) || !dto.HeaderLogoFile.IsSizeValid(3)) throw new SizeNotValidException();

        var setting = _mapper.Map<Setting>(dto);
        setting.HeaderLogoUrl = await _fileService.UploadAsync(dto.HeaderLogoFile, RootConstant.SettingImageRoot);
        setting.FooterLogoUrl = await _fileService.UploadAsync(dto.FooterLogoFile, RootConstant.SettingImageRoot);

        await _repo.CreateAsync(setting);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<SettingDetailItemDto>> GetAllAsync()
    {
        var settings = await _repo.GetAll().ToListAsync();
        var map = _mapper.Map<IEnumerable<SettingDetailItemDto>>(settings);

        foreach (var setting in settings)
        {
            foreach (var item in map)
            {
                item.HeaderLogoUrl = _config["Jwt:Issuer"] + "wwwroot/" + item.HeaderLogoUrl;
                item.FooterLogoUrl = _config["Jwt:Issuer"] + "wwwroot/" + item.FooterLogoUrl;
            }
        }
        return map;
    }

    public async Task UpdateAsync(int id, SettingUpdateDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Setting>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Setting>();

        if(dto.HeaderLogoFile != null)
        {
            _fileService.Delete(entity.HeaderLogoUrl);
            if (!dto.HeaderLogoFile.IsTypeValid("image")) throw new TypeNotValidException();
            if (!dto.HeaderLogoFile.IsSizeValid(3)) throw new SizeNotValidException();
            entity.HeaderLogoUrl = await _fileService.UploadAsync(dto.HeaderLogoFile, RootConstant.SettingImageRoot);
        }

        if (dto.FooterLogoFile != null)
        {
            _fileService.Delete(entity.FooterLogoUrl);
            if (!dto.FooterLogoFile.IsTypeValid("image")) throw new TypeNotValidException();
            if (!dto.FooterLogoFile.IsSizeValid(3)) throw new SizeNotValidException();
            entity.FooterLogoUrl = await _fileService.UploadAsync(dto.FooterLogoFile, RootConstant.SettingImageRoot);
        }

        _mapper.Map(dto, entity);
        await _repo.SaveAsync();
    }
}

