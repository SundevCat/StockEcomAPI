using Microsoft.EntityFrameworkCore;
using StockAPI.context;
using StockAPI.Models;
using System.Linq;

namespace StockAPI.Services;

public class ProductService
{
    private readonly DatabaseContext _context;
    public ProductService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProducts() => await _context.Products.OrderByDescending(e => e.UpdateDate).ToListAsync();
    public async Task<List<Product>> GetAllProductsActives() => await _context.Products.OrderByDescending(e => e.UpdateDate).Where(item => item.Status == "1").ToListAsync();
    public async Task<Product> GetProductBySku(string sku) => await _context.Products.FirstOrDefaultAsync(product => product.Sku == sku);
    public async Task CreateProduct(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }
    public async Task CreateMultiProduct(List<Product> products)
    {
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMultiProducts(List<Product> product)
    {
        _context.Products.UpdateRange(product);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteProduct(string sku)
    {
        var product = await _context.Products.FindAsync(sku);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}