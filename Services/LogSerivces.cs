using Microsoft.AspNetCore.Http.Features;
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
    public async Task<Log> GetLogByid(string Id) => await _context.Logs.FirstOrDefaultAsync(item => item.Id == Id);

    public async Task DeleteLogs(string Id)
    {
        var log = await _context.Logs.FirstOrDefaultAsync(item => item.Id == Id);
        if (log != null)
        {
            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();
        }
    }
    public async Task Createlogs(Log logs)
    {
        await _context.Logs.AddAsync(logs);
        await _context.SaveChangesAsync();
    }

}