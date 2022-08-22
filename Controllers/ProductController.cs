using JordanGardenStockWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace JordanGardenStockWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly ProductService _service;

    public ProductController(ILogger<ProductController> logger, ProductService service)
    {
        _logger = logger;
        _service = service;
    }
}