using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Models;
using StockAPI.Services;

namespace StockAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;
    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();
        if (products == null)
        {
            return NotFound();
        }
        return Ok(products);
    }

    [HttpGet("{sku}")]
    public async Task<ActionResult<Product>> GetProductBySku(string sku)
    {
        var product = await _productService.GetProductBySku(sku);
        if (product == null)
        {
            return NotFound(new { message = "sku not found" });
        }
        return Ok(product);
    }
    [HttpPost]
    public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
    {
        var result = await _productService.GetProductBySku(product.Sku);
        if (result != null)
        {
            return Conflict(new { message = "sku has already been" });
        }
        await _productService.CreateProduct(product);
        return Ok(product);
    }
    [HttpPut("{sku}")]
    public async Task<ActionResult<Product>> UpdateProduct(string sku, [FromBody] Product product)
    {
        if (sku != product.Sku)
        {
            return BadRequest();
        }
        else
        {
            await _productService.UpdateProduct(product);
            return Ok(new { message = " product updated successfully" });
        }
    }
    [HttpDelete("{sku}")]
    public async Task<ActionResult> DeleteProduct(string sku)
    {
        var product = await _productService.GetProductBySku(sku);
        if (product == null)
        {
            return NotFound(new { message = "sku not found" });
        }
        await _productService.DeleteProduct(sku);
        return Ok(new { message = "sku " + sku + " has been deleted" });
    }
    [HttpPatch("{sku}/{quantity}/{updateBy}")]
    public async Task<ActionResult> AddQuantitySingleProduct(string sku, int quantity, string updateBy)
    {
        var product = await _productService.GetProductBySku(sku);
        if (product == null)
        {
            return NotFound(new { message = "sku not found" });
        }
        product.Quantity += quantity;
        product.UpdateDate = DateTime.Now.ToString("dd/MMMM/yyyy HH:mm:ss");
        product.UpdateBy = updateBy;
        await _productService.UpdateProduct(product);
        var result = await _productService.GetProductBySku(sku);
        return Ok(new
        {
            message = "add stock success",
            result
        });
    }
    [HttpPatch("{sku}/{quantity}/{updateBy}")]
    public async Task<ActionResult> CutQuantitySingleProduct(string sku, int quantity, string updateBy)
    {
        var product = await _productService.GetProductBySku(sku);
        if (product == null)
        {
            return NotFound(new { message = "sku not found" });
        }
        product.Quantity -= quantity;
        product.UpdateDate = DateTime.Now.ToString("dd/MMMM/yyyy HH:mm:ss");
        product.UpdateBy = updateBy;
        await _productService.UpdateProduct(product);
        var result = await _productService.GetProductBySku(sku);
        return Ok(new
        {
            message = "cut stock success",
            result
        });
    }
    [HttpPatch("{updateBy}")]
    public async Task<ActionResult<List<Product>>> AddQuantityMultiProduct(List<Product> products, string updateBy)
    {
        List<Object> productList = new List<Object>();
        foreach (var product in products)
        {
            var result = await _productService.GetProductBySku(product.Sku);
            if (result == null)
            {
                productList.Add(new
                {
                    sku = product.Sku,
                    message = "sku not found",
                });
            }
            else
            {
                product.UpdateDate = DateTime.Now.ToString("dd/MMMM/yyyy HH:mm:ss");
                product.UpdateBy = updateBy;
                product.Quantity += result.Quantity;
                // await _productService.UpdateProduct(product);
                productList.Add(new
                {
                    sku = result.Sku,
                    quantity = product.Quantity
                });
            }

        }
        return Ok(productList);
    }

}