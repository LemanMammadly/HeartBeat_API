using Braintree;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.AppoinmentDtos;
using SerenityHospital.Business.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class BraintreeController : ControllerBase
{
    private readonly IBraintreeService _braintreeService;

    public BraintreeController(IBraintreeService braintreeService)
    {
        _braintreeService = braintreeService;
    }

    [HttpGet("[action]")]
    public IActionResult GetClientToken()
    {

            var gateway = _braintreeService.GetGateway();
            return Ok(gateway.ClientToken.Generate());
    }

    [HttpPost("[action]")]
    public IActionResult Create(string amount)
    {

            var gateway = _braintreeService.GetGateway();
            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(amount),
                PaymentMethodNonce = "",
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            return Ok(result.IsSuccess());
    }
}

