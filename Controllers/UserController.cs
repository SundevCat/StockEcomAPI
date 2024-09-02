using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StockAPI.Models;
using StockAPI.Services;

namespace StockAPI.Controllers;
[ApiController]
[Authorize(Roles = "plantoys")]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly UserService _userservice;
    private readonly HashService _hashService;

    public UserController(UserService userService, HashService hashService)
    {
        _userservice = userService;
        _hashService = hashService;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var users = await _userservice.GetAllUsers();
        if (users == null)
        {
            return NotFound();
        }
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        var user = await _userservice.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        var user = await _userservice.GetUserByEmail(email);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<User>> UpdateUser(String id, [FromBody] User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }
        else
        {
            await _userservice.UpdateUser(user);
            return Ok(new { message = "update complate" });
        }
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<User>> ChangePassword(string id, User user)
    {
        var userCheck = await _userservice.GetUserById(id);
        if (userCheck == null)
        {
            return BadRequest();
        }
        userCheck.Password = _hashService.HashPassword(user.Password);
        await _userservice.UpdateUser(userCheck);
        return Ok(new { message = "password changed" });
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(String id)
    {
        var user = await _userservice.GetUserById(id);
        if (user != null)
        {
            await _userservice.DeleteUser(id);
            return Ok(new { message = "Delete complete" });

        }
        else
        {
            return NotFound(new { message = "id not found" });
        }
    }
}
