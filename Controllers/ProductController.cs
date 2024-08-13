using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Models;
using StockAPI.Services;

namespace StockAPI.Controllers;
[ApiController]
[Authorize(Roles = "plantoys")]
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
    [HttpPatch("{updateBy}")]
    public async Task<ActionResult> AddQuantitySingleProduct([FromBody] Product product, string updateBy)
    {
        var products = await _productService.GetProductBySku(product.Sku);
        if (products == null)
        {
            return NotFound(new { message = "sku not found" });
        }
        products.Quantity += product.Quantity;
        products.UpdateDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
        products.UpdateBy = updateBy;
        await _productService.UpdateProduct(products);
        var result = await _productService.GetProductBySku(product.Sku);
        return Ok(new
        {
            message = "add stock success",
            result
        });
    }
    [HttpPatch("{updateBy}")]
    public async Task<ActionResult> CutQuantitySingleProduct([FromBody] Product product, string updateBy)
    {
        var products = await _productService.GetProductBySku(product.Sku);
        if (products == null)
        {
            return NotFound(new { message = "sku not found" });
        }
        products.Quantity -= product.Quantity;
        products.UpdateDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
        products.UpdateBy = updateBy;
        await _productService.UpdateProduct(products);
        var result = await _productService.GetProductBySku(product.Sku);
        return Ok(new
        {
            message = "cut stock success",
            result
        });
    }
    [HttpPatch("{updateBy}")]
    public async Task<ActionResult<List<Product>>> AddQuantityMultiProduct(List<Product> products, string updateBy)
    {
        if (products.Any())
        {
            List<Object> productList = new List<Object>();
            foreach (var update in products)
            {
                var product = await _productService.GetProductBySku(update.Sku);
                if (product != null)
                {
                    product.Quantity += update.Quantity;
                    product.UpdateBy = updateBy;
                    product.UpdateDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
                    productList.Add(new
                    {
                        sku = update.Sku,
                        quantity = product.Quantity

                    });
                    await _productService.UpdateProduct(product);
                }
                else
                {
                    productList.Add(new
                    {
                        sku = update.Sku,
                        quantity = "sku not found"

                    });
                }
            }
            productList.Add(new
            {
                updateBy = updateBy
            });
            return Ok(productList);
        }
        return BadRequest();
    }
    [HttpPatch("{updateBy}")]
    public async Task<ActionResult<List<Product>>> CutQuantityMultiProduct(List<Product> products, string updateBy)
    {
        if (products.Any())
        {
            List<Object> productList = new List<Object>();
            foreach (var update in products)
            {
                var product = await _productService.GetProductBySku(update.Sku);
                if (product != null)
                {
                    product.Quantity -= update.Quantity;
                    product.UpdateBy = updateBy;
                    product.UpdateDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
                    productList.Add(new
                    {
                        sku = update.Sku,
                        quantity = product.Quantity

                    });
                    await _productService.UpdateProduct(product);
                }
                else
                {
                    productList.Add(new
                    {
                        sku = update.Sku,
                        quantity = "sku not found"

                    });
                }
            }
            productList.Add(new
            {
                updateBy = updateBy
            });
            return Ok(productList);
        }
        return BadRequest();
    }

}