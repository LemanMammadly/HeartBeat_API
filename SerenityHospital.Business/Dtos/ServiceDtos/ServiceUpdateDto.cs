﻿using FluentValidation;
using SerenityHospital.Business.Dtos.DepartmentDtos;

namespace SerenityHospital.Business.Dtos.ServiceDtos;

public record ServiceUpdateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ServiceBeginning { get; set; }
    public DateTime ServiceEnding { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public IEnumerable<int>? DepartmentIds { get; set; }
}

public class ServiceUpdateDtoValidator:AbstractValidator<ServiceUpdateDto>
{
    public ServiceUpdateDtoValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty()
                .WithMessage("Name dont be empty")
            .NotNull()
                .WithMessage("Name dont be null");
        RuleFor(s => s.Description)
            .NotEmpty()
                .WithMessage("Description dont be empty")
            .NotNull()
                .WithMessage("Description dont be null");
        RuleFor(s => s.ServiceBeginning)
            .NotEmpty()
                .WithMessage("ServiceBeginning dont be empty")
            .NotNull()
                .WithMessage("ServiceBeginning dont be null");
        RuleFor(s => s.ServiceEnding)
            .NotEmpty()
                .WithMessage("ServiceEnding dont be empty")
            .NotNull()
                .WithMessage("ServiceEnding dont be null")
            .GreaterThan(s => s.ServiceBeginning)
                .WithMessage("ServiceEnding must be greater than ServiceBeginning");
        RuleFor(s => s.MinPrice)
            .NotEmpty()
                .WithMessage("MinPrice dont be empty")
            .NotNull()
                .WithMessage("MinPrice dont be null")
            .GreaterThan(0)
                .WithMessage("MinPrice greater than 0");
        RuleFor(s => s.MaxPrice)
             .NotEmpty()
                .WithMessage("MaxPrice dont be empty")
            .NotNull()
                .WithMessage("MaxPrice dont be null")
            .GreaterThan(s => s.MinPrice)
                .WithMessage("MaxPrice must be greater than MinPrice");
        RuleFor(s => s.DepartmentIds)
            .Must(s=>IsDistinct(s))
                .WithMessage("You cannot add a department with the same id");
    }

    private bool IsDistinct(IEnumerable<int> ids)
    {
        var encounteredIds = new HashSet<int>();

        if(ids != null)
        {
            foreach (var id in ids)
            {
                if (!encounteredIds.Contains(id))
                {
                    encounteredIds.Add(id);
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }
}

