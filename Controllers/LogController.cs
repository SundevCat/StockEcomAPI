using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Models;
using StockAPI.Services;

namespace StockAPI.Controllers;
[ApiController]
[Authorize(Roles = "plantoys")]
[Route("api/[controller]/[action]")]
public class LogController : Controller
{
    private readonly LogService _logService;

    public LogController(LogService logService)
    {
        _logService = logService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Log>>> getLogs()
    {
        var logs = await _logService.GetAllLogs();
        if (logs == null)
        {
            return BadRequest();
        }
        return Ok(logs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Log>> GetLogsById(string id)
    {
        var log = await _logService.GetLogByid(id);
        if (log == null)
        {
            return NotFound();
        }
        return Ok(log);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLog(string id)
    {
        var log = await _logService.GetLogByid(id);
        if (log == null)
        {
            return NotFound();
        }
        await _logService.DeleteLogs(id);
        return Ok(new { message = "Delete successfully" });
    }
}