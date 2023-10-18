using System;
using SerenityHospital.Business.Dtos.RecipeDtos;

namespace SerenityHospital.Business.Dtos.PatientHistoryDtos;

public record PatientHistoryListItemDto
{
    public int Id { get; set; }
    public string Information { get; set; }
    public DateTime Date { get; set; }
    public RecipeListItemDto Recipe { get; set; }
}

