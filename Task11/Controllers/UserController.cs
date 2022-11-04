using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Task11.Dtos;
using Task11.Services;

namespace Task11.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _service;

    public UserController(
        ILogger<UserController> logger,
        IUserService userService)
    {
        _logger = logger;
        _service = userService;
    }

    [HttpPost("Register")]
    public IActionResult Register(RegisterDto dtoModel)
    {
        if (!ModelState.IsValid) return BadRequest("Model state is invalid");

        var insertResult = _service.InsertUser(dtoModel.Login, dtoModel.Password);

        if (!insertResult) return BadRequest("Registration failed.");
        return Ok("User successfully added to database.");
    }

    [HttpPost("Login")]
    public IActionResult Login(LoginDto dtoModel)
    {
        if (!ModelState.IsValid) return BadRequest("Model state is invalid");

        var existingUser = _service.RetrieveUserByLogin(dtoModel.Login);
        if (existingUser is null) return NotFound("The user with given login not found");

        if (existingUser.Password != dtoModel.Password) return BadRequest("Password is invalid");

        Response.Headers.Add(HeaderNames.Authorization, existingUser.Key.ToString());

        return Ok("Login successfully completed.");
    }
}
