using Core.Models;

namespace Core.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    User? ValidateToken(string token);
}