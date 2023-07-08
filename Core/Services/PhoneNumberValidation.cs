using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Grpc.Core;

namespace Core.Services;

public class PhoneNumberValidationService : PhoneNumberValidation.PhoneNumberValidationBase {

    private readonly ILogger<PhoneNumberValidationService> _logger;
    private readonly IAuthService _authService;

    public PhoneNumberValidationService(ILogger<PhoneNumberValidationService> logger, IAuthService authService) {
        _logger = logger;
        _authService = authService;
    }

    public override Task<ValidationResult> Validate(ValidationRequest request, ServerCallContext context) {
        _logger.LogInformation("Validating VerificationCode {PhoneNumber}", request.VerificationCode);
        var user = _authService.GetUserByToken(request.Token).Result ?? throw new RpcException(new Status(StatusCode.Unauthenticated, "User not found"));
        _logger.LogInformation("User found {User}", user.Email);
        _logger.LogInformation("User verification code {VerificationCode}", user.VerificationCode);

        if (user.VerificationCode.ToString() != request.VerificationCode) {
            return Task.FromResult(new ValidationResult {
                Valid = false,
                Error = "Invalid verification code"
            });
        }

        user.IsPhoneVerified = true;
        _authService.Update(user);
        
        return Task.FromResult(new ValidationResult {
            Valid = user.IsPhoneVerified,
            Error = string.Empty
        });
    }
}