using System.Text.RegularExpressions;
using FluentValidation;

namespace SerenityHospital.Business.Dtos.ContactDtos;

public record CreateContactDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Message { get; set; }
}

public class CreateContactDtoValidator:AbstractValidator<CreateContactDto>
{
    public CreateContactDtoValidator()
    {
       RuleFor(c=>c.Name)
            .NotEmpty()
                .WithMessage("Name dont be empty")
            .NotNull()
                .WithMessage("Name dont be null");
        RuleFor(c => c.Email)
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
        RuleFor(c => c.Phone)
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
        RuleFor(c => c.Address)
            .NotEmpty()
                .WithMessage("Address dont be empty")
            .NotNull()
                .WithMessage("Address dont be null");
        RuleFor(c => c.Message)
            .MinimumLength(10)
                .WithMessage("Please write the message clearly.")
            .NotEmpty()
                .WithMessage("Message dont be empty")
            .NotNull()
                .WithMessage("Message dont be null");

    }
}