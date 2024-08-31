
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StockAPI.Models;
using StockAPI.Services;

namespace StockAPI.Controllers;
[AllowAnonymous]
[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : Controller
{
    private readonly UserService _userservice;
    private readonly HashService _hashservice;
    private readonly TokenServices _tokenservice;

    public AuthController(UserService userService, HashService hashService, TokenServices tokenServices)
    {
        _userservice = userService;
        _hashservice = hashService;
        _tokenservice = tokenServices;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Login([FromBody] User user)
    {
        var checkUser = await _userservice.GetUserByEmail(user.Email);
        if (checkUser != null)
        {
            var checkpassword = _hashservice.Verifypassword(checkUser.Password, user.Password);
            if (checkpassword)
            {
                var tokenString = _tokenservice.CreateToken(checkUser.Id);
                return Ok(tokenString);
            }
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<User>> Register([FromBody] User user)
    {
        var checkEmail = await _userservice.GetUserByEmail(user.Email);
        if (checkEmail != null)
        {
            return Conflict(new { message = "email alerady exits" });
        }
        user.Id = Guid.NewGuid().ToString();
        user.Password = _hashservice.HashPassword(user.Password);
        await _userservice.CreateUser(user);
        return Ok(new { message = " created success" });
    }
}