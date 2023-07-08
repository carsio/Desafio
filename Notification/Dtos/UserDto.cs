using System.Text.Json;

namespace Core.Dtos;

public class UserDto {
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int VerificationCode  { get; set; }


    public string ToJson() {
        return JsonSerializer.Serialize(this);
    }

    public static UserDto FromJson(string json) {
        return JsonSerializer.Deserialize<UserDto>(json) ?? new UserDto();
    }
}