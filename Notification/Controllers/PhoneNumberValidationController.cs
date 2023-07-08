using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Notification.Dtos;

namespace Notification.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PhoneNumberValidationController : ControllerBase
{
    private readonly ILogger<PhoneNumberValidationController> _logger;

    public PhoneNumberValidationController(ILogger<PhoneNumberValidationController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "PostPhoneNumberValidation")]
    public IActionResult Post(
        [FromBody] VerificationCodeDto code,
        [FromHeader(Name = "Authorization")] string bearerToken
    )
    {
        if (string.IsNullOrEmpty(bearerToken)) return Unauthorized();

        _logger.LogInformation("Received code: {code}", code.Code);

        using var channel = GrpcChannel.ForAddress("https://localhost:5001");
        var client = new PhoneNumberValidation.PhoneNumberValidationClient(channel);
        var response = client.Validate(new ValidationRequest { 
            VerificationCode = code.Code.ToString(),
            Token = bearerToken.Split(" ").Last()
        });

        return Ok(new {
            valid = response.Valid,
            error = response.Error
        });
    }
}
