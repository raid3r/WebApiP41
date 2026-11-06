using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using WebApiP41.Services;
using WebApiP41.Models;
using WebApiP41.Models.DTO;


namespace WebApiP41.Controllers;

[ApiController]
[Route("api/users")] // api/users
[Tags("User Management")]
//[Produces("application/json")]
//[Consumes("application/json")]
public class UserController(UserService userService): ControllerBase
{

    // реєстрація нового користувача POST /api/users/register
    [HttpPost("register")]
    [EndpointName("RegisterUser")]
    [EndpointDescription("Register a new user")]
    [EndpointSummary("This endpoint allows a new user to register by providing their name, email, and password.")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody, Description("Register request")] RegisterRequest request)
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



    /// <summary>
    /// Authenticate a user with their email and password
    /// </summary>
    /// <param name="request">The login request containing email and password</param>
    /// <returns>The authentication response with user name and token</returns>
    /// <response code="200">Returns the authenticated user with a token</response>
    /// <response code="401">If the email or password is incorrect</response>
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody, Description("Login request containing email and password")] LoginRequest request)
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
