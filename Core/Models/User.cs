
using Newtonsoft.Json;

namespace Core.Models;

public class User
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;
    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; } = string.Empty;
    [JsonProperty(PropertyName = "phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;
    [JsonProperty(PropertyName = "is_phone_verified")]
    public bool IsPhoneVerified { get; set; } = false;
    [JsonProperty(PropertyName = "verification_code")]
    public int VerificationCode { get; set; } = 0;
    [JsonProperty(PropertyName = "password")]
    public string Password { get; set; } = string.Empty;
}
