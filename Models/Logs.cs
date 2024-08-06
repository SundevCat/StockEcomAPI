using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockAPI.Models;
[Table("logs")]
public class Log
{
    public string Id { get; set; }
    [Required]
    public DateTime Timestamp { get; set; }
    public string UpdateBy { get; set; }
    public string Descripttion { get; set; }
    public string logsSku { get; set; }
}