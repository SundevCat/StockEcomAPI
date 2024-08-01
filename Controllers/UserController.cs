using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Models;
using StockAPI.Services;

namespace StockAPI.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly UserService _userservice;
    public UserController(UserService userService)
    {
        _userservice = userService;
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
    [HttpPost]
    public async Task<ActionResult<User>> AddUser([FromBody] User user)
    {
        user.Id = Guid.NewGuid().ToString();
        var result = await _userservice.GetUserById(user.Id);
        if (result != null)
        {
            return Conflict(new { message = "user already exists" });
        }
        await _userservice.CreateUser(user);
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
