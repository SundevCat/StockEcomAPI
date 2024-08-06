using Microsoft.EntityFrameworkCore;
using StockAPI.context;
using StockAPI.Models;

namespace StockAPI.Services;
public class LogService
{
    private readonly DatabaseContext _context;
    public LogService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Log>> GetAllLogs() => await _context.Logs.ToListAsync();
}