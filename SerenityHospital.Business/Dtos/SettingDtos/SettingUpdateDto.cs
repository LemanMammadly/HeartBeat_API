using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Validators;

namespace SerenityHospital.Business.Dtos.SettingDtos;

public record SettingUpdateDto
{
    public IFormFile? HeaderLogoFile { get; set; }
    public IFormFile? FooterLogoFile { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}

public class SettingUpdateDtoValidator:AbstractValidator<SettingUpdateDto>
{
    public SettingUpdateDtoValidator()
    {
        RuleFor(s => s.HeaderLogoFile)
            .SetValidator(new FileValidator());
        RuleFor(s => s.FooterLogoFile)
            .SetValidator(new FileValidator());
        RuleFor(s => s.Address)
            .NotEmpty()
                .WithMessage("Address dont be empty")
            .NotNull()
                .WithMessage("Address dont be null");
        RuleFor(s => s.Email)
            .NotEmpty()
                .WithMessage("Email dont be empty")
            .NotNull()
                .WithMessage("Email dont be null")
            .Must(s =>
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                var result = regex.Match(s);
                return result.Success;
            })
                 .WithMessage("Please enter valid email");
        RuleFor(s => s.Phone)
            .NotEmpty()
                .WithMessage("Phone number dont be empty")
            .NotNull()
                .WithMessage("Phone number dont be null")
            .Must(s =>
            {
                Regex regex = new Regex(@"^(\+994|0)(50|51|55|70|77|99)[1-9]\d{6}$");
                var result = regex.Match(s);
                return result.Success;
            })
                .WithMessage("Please enter valid phone number");

    }
}