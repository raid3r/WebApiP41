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
        // перетворюємо DTO в модель користувача
        var newUser = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };

        try
        {
            var registeredUser = await userService.RegisterUserAsync(newUser);

            // перетворюємо модель користувача в DTO для відповіді
            return Ok(new AuthResponse
            {
                Name = registeredUser.Name,
                Token = registeredUser.Token
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

        // перетворюємо модель користувача в DTO для відповіді
        return Ok(new AuthResponse
        {
            Name = authenticatedUser.Name,
            Token = authenticatedUser.Token
        }); // 200 OK
    }



    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>A collection of all users</returns>
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();

        // перетворюємо колекцію моделей користувачів в колекцію DTO для відповіді
        var userViewModels = users.Select(u => new UserViewModel
        {
            Name = u.Name,
            Email = u.Email
        });
        return Ok(userViewModels);
    }

    
    /// <summary>
    /// Get a user by their email
    /// </summary>
    /// <param name="email">The email of the user to retrieve</param>
    /// <returns>The user with the specified email, or a 404 Not Found if not found</returns>
    [HttpGet("{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await userService.GetUserByEmailIdAsync(email);
        if (user == null)
        {
            return NotFound(); // 404 Not Found
        }
        // перетворюємо модель користувача в DTO для відповіді
        var userViewModel = new UserViewModel
        {
            Name = user.Name,
            Email = user.Email
        };
        return Ok(userViewModel); // 200 OK
    }


}
