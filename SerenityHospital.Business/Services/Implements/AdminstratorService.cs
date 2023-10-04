using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SerenityHospital.Business.Constants;
using SerenityHospital.Business.Dtos.AdminstratorDtos;
using SerenityHospital.Business.Dtos.TokenDtos;
using SerenityHospital.Business.Exceptions.Common;
using SerenityHospital.Business.Exceptions.Images;
using SerenityHospital.Business.Extensions;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;
using SerenityHospital.Core.Enums;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class AdminstratorService : IAdminstratorService
{
    readonly UserManager<Adminstrator> userManager;
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly IHospitalRepository _hospitalRepository;
    readonly ITokenService _tokenService;

    public AdminstratorService(UserManager<Adminstrator> userManager, IMapper mapper, IFileService fileService, IHospitalRepository hospitalRepository, ITokenService tokenService)
    {
        this.userManager = userManager;
        _mapper = mapper;
        _fileService = fileService;
        _hospitalRepository = hospitalRepository;
        _tokenService = tokenService;
    }

    public async Task CreateAsync(CreateAdminstratorDto dto)
    {

        var existAdminstrator = await userManager.Users.FirstOrDefaultAsync(a => a.IsDeleted == false);

        if (existAdminstrator != null) throw new AppUserIsAlreadyExistException<Adminstrator>();

        if (dto.ImageFile != null)
        {
            if (!dto.ImageFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.ImageFile.IsTypeValid("image")) throw new TypeNotValidException();
        }

        var adminstrator = _mapper.Map<Adminstrator>(dto);

        if(dto.ImageFile != null)
        {
            adminstrator.ImageUrl = await _fileService.UploadAsync(dto.ImageFile, RootConstant.AdminstratortImageRoot);
        }

        adminstrator.Status = WorkStatus.Active;

        if (await userManager.Users.AnyAsync(a => a.UserName == dto.UserName || a.Email == dto.Email)) throw new AppUserIsAlreadyExistException<Adminstrator>();

        var hospital =await _hospitalRepository.GetFirstAsync();

        adminstrator.HospitalId = hospital.Id;


        var result = await userManager.CreateAsync(adminstrator, dto.Password);

        if(!result.Succeeded)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in result.Errors)
            {
                sb.Append(item.Description + " ");
            }
            throw new RegisterFailedException<Adminstrator>();
        }
    }

    public async Task SoftDeleteAsync(string id)
    {
        var adminstrator = await userManager.Users.FirstOrDefaultAsync(a => a.Id == id);
        if (adminstrator == null) throw new AppUserNotFoundException<Adminstrator>();
        adminstrator.IsDeleted = true;
        adminstrator.HospitalId = null;

        await userManager.UpdateAsync(adminstrator);
    }

    public async Task RevertSoftDeleteAsync(string id)
    {
        var adminstrator = await userManager.Users.FirstOrDefaultAsync(a => a.Id == id);
        if (adminstrator == null) throw new AppUserNotFoundException<Adminstrator>();

        var existAdminstrator= await userManager.Users.FirstOrDefaultAsync(a => a.IsDeleted == false);
        if(existAdminstrator != null) throw new AppUserIsAlreadyExistException<Adminstrator>();

        var hospital = await _hospitalRepository.GetFirstAsync();
        adminstrator.IsDeleted = false;
        adminstrator.HospitalId = hospital.Id;

        await userManager.UpdateAsync(adminstrator);
    }

    public async Task<TokenResponseDto> LoginAsync(LoginAdminstratorDto dto)
    {
        var adminstrator = await userManager.FindByNameAsync(dto.UserName);
        if (adminstrator == null) throw new LoginFailedException<Adminstrator>("Username or password is wrong");

        var result = await userManager.CheckPasswordAsync(adminstrator, dto.Password);
        if (!result) throw new LoginFailedException<Adminstrator>("Username or password is wrong");

        return _tokenService.CreateToken(adminstrator);
    }
}

