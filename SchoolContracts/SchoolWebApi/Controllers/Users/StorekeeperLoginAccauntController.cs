using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApi.Models;
using SchoolWebApi.Services;

namespace SchoolWebApi.Controllers.Users;

public class StorekeeperLoginAccauntController : ControllerBase
{

    private readonly JwtService _jwtService;
    public StorekeeperLoginAccauntController(JwtService jwtService) => _jwtService = jwtService;

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel request)
    {
        var result = await _jwtService.Authenticate(request);

        if (result is null)
            return Unauthorized();

        return result;
    }

}
