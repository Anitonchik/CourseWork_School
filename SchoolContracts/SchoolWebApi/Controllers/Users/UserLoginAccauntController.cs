using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.BindingModels;
using SchoolWebApi.Models;
using SchoolWebApi.Services;

namespace SchoolWebApi.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UserLoginAccauntController : ControllerBase
{

    private readonly JwtService _jwtService;
    public UserLoginAccauntController(JwtService jwtService) => _jwtService = jwtService;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseModel>> Login([FromBody]  LoginRequestModel request)
    {
        var result = await _jwtService.Authenticate(request);

        if (result is null)
            return Unauthorized();

        return result;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserBindingModel model)
    {
        var result = await _jwtService.Register(model);

        return Ok(new
        {
            User = result.UserLogin,
            AccessToken = result.AccessToken
        });
    }
}
