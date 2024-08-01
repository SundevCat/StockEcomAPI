using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.context;
using StockAPI.Models;

namespace StockAPI.Services;
public class UserService
{
    private readonly UserContext _context;

    public UserService(UserContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers() => await _context.User.ToListAsync();
    public async Task<User> GetUserById(string id) => await _context.User.FirstOrDefaultAsync(user => user.Id == id);
    public async Task CreateUser(User user)
    {
        _context.User.Add(user);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateUser(User user)
    {
        _context.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteUser(string id)
    {
        var user = await _context.User.FindAsync(id);
        if (user != null)
        {
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

}