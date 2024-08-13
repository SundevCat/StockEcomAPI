using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockAPI.Models;
[Table("Products")]
public class Product
{
    public string Sku { get; set; }
    [Required]
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; }
    public string UpdateDate { get; set; }
    public string UpdateBy { get; set; }
    public string Barcode { get; set; }

}