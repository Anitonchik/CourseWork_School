using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SchoolContracts.AdapterContracts;
using SchoolWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SchoolWebApi.Services;

public class JwtService
{
    public readonly IStorekeeperAdapter _adapter;

    private readonly IConfiguration _configuration;

    public JwtService(IStorekeeperAdapter adapter, IConfiguration configuration)
    {
        _adapter = adapter;
        _configuration = configuration;
    }

    public async Task<LoginResponseModel> Authenticate(LoginRequestModel request)
    {
        if (string.IsNullOrWhiteSpace(request.UserLogin) || string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        var userAccaunt = _adapter.GetUserByLogin(request.UserLogin);
        // добавить проверку на верность пароля из request
        if (userAccaunt is null || request.Password != userAccaunt)
            return null;

        var issuer = _configuration["JwtConfig:Issuer"];
        var audience = _configuration["JwtConfig:Audience"];
        var key = _configuration["JwtConfig:Key"];
        var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                // данные внутри токена, вписываем login и роль пользователя
                new Claim(JwtRegisteredClaimNames.Actort, request.Role.ToString()),
                //new Claim(JwtRegisteredClaimNames.Name, request.UserLogin)
            }),
            Expires = tokenExpiryTimeStamp,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            SecurityAlgorithms.HmacSha256Signature),
        };

        // генерация токена
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return new LoginResponseModel
        {
            AccessToken = accessToken,
            UserLogin = request.UserLogin,
            ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
        };
    }
}
