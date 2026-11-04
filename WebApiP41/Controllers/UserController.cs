using Microsoft.AspNetCore.Mvc;
using WebApiP41.Services;
using WebApiP41.Models;
using WebApiP41.Models.DTO;


namespace WebApiP41.Controllers;

[ApiController]
[Route("api/users")] // api/users
public class UserController(UserService userService): ControllerBase
{

    // реєстрація нового користувача POST /api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
    {
        var newUser = new User
        {
            Name = request.
            Email = request.Email,
            Password = request.Password
        };

        try
        {
            var registeredUser = await userService.RegisterUserAsync(newUser);
            return Ok(new AuthResponse
            {
                Name = newUser.Name,
                Token = newUser.Token
            });
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }        
    }



    // перевірка логіна та пароля користувача POST /api/users/login?login=...&password=...
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
    {
        var authenticatedUser = await userService.AuthenticateUserAsync(request.Email, request.Password);
        if (authenticatedUser == null)
        {
            return Unauthorized(); // 401 Unauthorized
        }
        return Ok(new AuthResponse
        {
            Name = authenticatedUser.Name,
            Token = authenticatedUser.Token
        }); // 200 OK
    }




}
