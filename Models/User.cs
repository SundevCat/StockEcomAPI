using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockAPI.Models;

[Table("Users")]
public class User
{
    public string Id { get; set; }
    [Required]

    public string Name { get; set; }
    public string Email { get; set; }
}

