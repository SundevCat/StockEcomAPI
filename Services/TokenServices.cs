using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;

namespace StockAPI.Services;
public class TokenServices
{
    private readonly IConfiguration _configuration;
    public TokenServices(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private SigningCredentials GetsignigCredentaials()
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["secret:key"]));
        var signigCredentaials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        return signigCredentaials;
    }

    private List<Claim> GetClaims(string id)
    {
        var claims = new List<Claim>{
            new Claim("id",id),
            new Claim(ClaimTypes.Role,"plantoys")
        };
        return claims;
    }

    public string CreateToken(string id)
    {
        var tokenOption = new JwtSecurityToken(
            issuer: _configuration["secret:issuer"],
            audience: _configuration["secret:audience"],
            claims: GetClaims(id),
            signingCredentials: GetsignigCredentaials()
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOption);
        return tokenString;
    }
}