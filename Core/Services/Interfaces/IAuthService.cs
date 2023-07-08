
using Core.Dtos;

namespace Core.Services.Interfaces;

public interface IAuthService {
    Task<UserDto> GetUserByToken(string token);
    Task<UserInfoDto> GetUserInfoByToken(string token);
    Task<UserDto> GetUserByEmail(string email);
    Task<string> Register(CreateUserDto user);
    Task<UserDto> Update(UserDto user);
    Task<string> Token(LoginUserDto user);
    bool Authenticate(HttpContext httpContext);
}