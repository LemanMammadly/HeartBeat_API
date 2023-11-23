using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Dtos.AppoinmentDtos;

public record AppoinmentPurchaseDto
{
    public string Nonce { get; set; }
    public decimal AppoinmentMoney { get; set; }
}


