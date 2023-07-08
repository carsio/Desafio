using System.Text.Json;

namespace Core.Dtos;

public class UserInfoDto { 
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsPhoneVerified { get; set; } = false;
}

public class UserDto : UserInfoDto {
    public int VerificationCode { get; set; } = new Random().Next(100000, 999999);
}

public class CreateUserDto : UserDto{
    public string Password { get; set; } = string.Empty;

    public UserDto CloneWithoutPassword() {
        return new UserDto {
            Email = Email,
            PhoneNumber = PhoneNumber,
            VerificationCode = VerificationCode,
            IsPhoneVerified = IsPhoneVerified
        };
    }

    public string ToJson() {
        return JsonSerializer.Serialize(CloneWithoutPassword());
    }
}

public class LoginUserDto {
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}