using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Pictogen.API.Controllers;

[ApiController]
public sealed class AuthController(IConfiguration configuration) : ControllerBase {
    [HttpPost("api/auth/login")]
    public IActionResult Login(string userName, string password) {
        if (userName != "testUser" || password != "testPassword") return BadRequest("Invalid username or password");
        
        string token = GenerateJwtToken(userName);
        return Ok(new { Token = token });
    }
    
    private string GenerateJwtToken(string userName) {
        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub, userName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, userName)
        ];
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
