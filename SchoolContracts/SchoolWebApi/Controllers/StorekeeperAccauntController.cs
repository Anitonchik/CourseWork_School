using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApi.Models;
using SchoolWebApi.Services;
using Serilog;

namespace SchoolWebApi.Controllers;

public class StorekeeperAccauntController : ControllerBase
{

    private readonly JwtService _jwtService;
    public StorekeeperAccauntController(JwtService jwtService) => _jwtService = jwtService;

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
