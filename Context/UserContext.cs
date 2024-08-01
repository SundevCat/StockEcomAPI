using Microsoft.EntityFrameworkCore;
using StockAPI.Models;

namespace StockAPI.context;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {

    }
    public DbSet<User> User { get; private set; }
}