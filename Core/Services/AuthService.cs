using Core.Dtos;
using Core.Exceptions;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Core.Services;

public class AuthService: IAuthService {
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtGeneratorService;
    private readonly ServiceBusService _serviceBusService;

    public AuthService(
        IUserRepository userRepository,
        IJwtService jwtGeneratorService,
        ServiceBusService serviceBusService
    ) {
        _userRepository = userRepository;
        _jwtGeneratorService = jwtGeneratorService;
        _serviceBusService = serviceBusService;
    }
    
    public async Task<UserDto> GetUserByToken(string token) {
        var user = _jwtGeneratorService.ValidateToken(token);
        return await GetUserByEmail(user!.Email);
    }

    public async Task<UserInfoDto> GetUserInfoByToken(string token) {
        var user = _jwtGeneratorService.ValidateToken(token);
        var userDto = await GetUserByEmail(user!.Email);
        return new UserInfoDto {
            Email = userDto!.Email,
            PhoneNumber = userDto!.PhoneNumber,
            IsPhoneVerified = userDto!.IsPhoneVerified,
        };
    }

    public async Task<UserDto> GetUserByEmail(string email) {
        var user = await _userRepository.GetUserByEmail(email);
        return new UserDto {
            Email = user!.Email,
            PhoneNumber = user!.PhoneNumber,
            IsPhoneVerified = user!.IsPhoneVerified,
            VerificationCode = user!.VerificationCode
        };
    }

    public async Task<string> Register(CreateUserDto userDto) {
        var newUser = new User {
            Email = userDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            PhoneNumber = userDto.PhoneNumber,
            IsPhoneVerified = false,
            VerificationCode = userDto.VerificationCode
        };

        var existingUser = await _userRepository.GetUserByEmail(newUser.Email);

        if (existingUser != null) {
            throw new BusinessException("User already exists");
        }

        var createdUser = await _userRepository.Create(newUser);

        await _serviceBusService.SendMessage("user-created", userDto.ToJson());
        return createdUser.Id;
    }

    public async Task<string> Token(LoginUserDto userDto) {
        var user = await _userRepository.GetUserByEmail(userDto.Email) ?? throw new BusinessException("User does not exist");

        if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password)) {
            throw new BusinessException("User does not exist or password is incorrect");
        }

        return _jwtGeneratorService.GenerateToken(user);
    }

    public bool Authenticate(HttpContext httpContext) {
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
            return false;

        var user = _jwtGeneratorService.ValidateToken(token);
        return user != null;
    }

    public async Task<UserDto> Update(UserDto userDto) {
        var user = await _userRepository.GetUserByEmail(userDto.Email) ?? throw new BusinessException("User does not exist");

        user.PhoneNumber = userDto.PhoneNumber;
        user.IsPhoneVerified = userDto.IsPhoneVerified;
        user.VerificationCode = userDto.VerificationCode;

        await _userRepository.Update(user);

        return new UserDto {
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsPhoneVerified = user.IsPhoneVerified,
        };
    }
}