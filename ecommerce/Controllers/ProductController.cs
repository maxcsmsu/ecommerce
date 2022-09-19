using ecommerce.Cache;
using ecommerce.Data;
using ecommerce.Models;
using ecommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace ecommerce.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _repo;

    private static bool IsNotNull([NotNullWhen(true)] object? obj) => obj != null;
    private ICacheService _cacheService;  //Caching in .NET  => https://docs.microsoft.com/en-us/dotnet/core/extensions/caching
    private readonly ILogger<ProductController> _logger;
    private readonly IConfiguration _configuration;

    private double initExpirationTime = 1;
    public ProductController(IConfiguration configuration, ILogger<ProductController> logger, ICacheService cacheService, IProductService repo)
    {
        _configuration = configuration;
        _repo = repo;
        _cacheService = cacheService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]ProductRequestDto productRequest)
    {
        try
        {
            //get product from cache
            var cacheData = _cacheService.GetData<List<Product>>("product");
            if (cacheData != null)
            {
                if (!IsNotNull(productRequest.Name))
                     return Ok(cacheData);
                else
                {
                    return Ok(cacheData.Where(x => x.Name.IndexOf(productRequest.Name) > -1).ToList());
                }
            }

            try
            {
                initExpirationTime = Convert.ToDouble(_configuration.GetSection("Configuration:ExpirationTime").Value);
            }
            catch { }

            //get product from database 
            cacheData = await _repo.GetAll();

            //set  product to cache
            var expirationTime = DateTimeOffset.Now.AddMinutes(initExpirationTime);
            _cacheService.SetData<List<Product>>("product", cacheData, expirationTime);

            return Ok(productRequest.Name != null ? cacheData.Where(x => x.Name.IndexOf(productRequest.Name) > -1).ToList():cacheData);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            //get data from cache
            var cacheData = _cacheService.GetData<List<Product>>("product");
            if (cacheData != null)
            {
                //find data  in cache
                var productcache = cacheData.Where(x => x.Id == id).FirstOrDefault();
                if (productcache != null)
                    return Ok(productcache);
            }

            //find data  from web api
            var product = await _repo.GetById(id);
            if (product == null)
                return NotFound(new { status = false, message = "Product not found" });
            return Ok(product);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { status = false, message = "Product not found" });
        }

    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateUpdateProductRequest product)
    {
        try
        {
            //create product in database
            await _repo.Create(product);
            _cacheService.RemoveData("product");
            return Ok(new { status = true, message = "Product created" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, CreateUpdateProductRequest productData)
    {
        try
        {
            //check request
            if (productData == null || id == 0)
                return BadRequest();

            //check product id from database
            var product = await _repo.GetById(id);
            if (product == null)
                return NotFound(new { status = false, message = "Product not found" });


            //update product in database
            await _repo.Update(id, productData);
            _cacheService.RemoveData("product");
            return Ok(new { status = true, message = "Product updated" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {   
            //check product id from database
            var product = await _repo.GetById(id);
            if (product == null) return NotFound(new { status = false, message = "Product not found" });

            //delete product
            await _repo.Delete(id);
            _cacheService.RemoveData("product");
            return Ok(new { status = true, message = "Product deleted" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
        }

    }
}
