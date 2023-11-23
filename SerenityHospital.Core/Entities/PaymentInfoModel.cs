namespace SerenityHospital.Core.Entities;


public class PaymentInfoModel
{
    public string CreditCardNumber { get; set; }
    public string ExpiryMonth { get; set; }
    public string ExpiryYear { get; set; }
    public string CVV { get; set; }
    public decimal AppoinmentMoney { get; set; }
}

