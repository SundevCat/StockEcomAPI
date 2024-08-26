using System.IO.Pipelines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Services;

namespace StockAPI.Controllers;
[ApiController]
[Authorize(Roles = "plantoys")]
[Route("api/[controller]/[action]")]

public class FilesController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly FileService _fileService;
    public FilesController(FileService fileService, IWebHostEnvironment env)
    {
        _fileService = fileService;
        _env = env;
    }
    [HttpGet("{filename}")]
    public async Task<ActionResult> FileTemplateProduct(string filename)
    {

        var filePath = Path.Combine(_env.WebRootPath ?? Directory.GetCurrentDirectory(), "FileTemplate", filename);

        if (System.IO.File.Exists(filePath))
        {
            NotFound();
        }
        var memory = new MemoryStream();
        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        Console.WriteLine(filePath);
        return File(memory, _fileService.GetContentType(filePath), Path.GetFileName(filePath));
    }
}
