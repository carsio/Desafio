using Core.Dtos;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) {
        _authService = authService;
    }

    [HttpGet("user")]
    public async Task<UserInfoDto> User(
        [FromHeader(Name = "Authorization")] string bearerToken
    ) {
        var token = bearerToken.Split(" ").Last();        
        return await _authService.GetUserInfoByToken(token);
    }

    [HttpPost("register")]
    async public Task<IActionResult> Register(
        [FromBody] CreateUserDto userDto
    ) {
        var result = await _authService.Register(userDto);
        return new OkObjectResult(result);
    }
    
    [HttpPost("token")]
    public async Task<TokenDto> Token([FromBody] LoginUserDto userDto) {
        var token = await _authService.Token(userDto);
        return new TokenDto { Token = token };
    }

    [HttpGet("error")]
    public IActionResult Error() {
        throw new Exception("This is an exception");
    }
}