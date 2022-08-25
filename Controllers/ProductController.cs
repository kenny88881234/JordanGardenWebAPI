using JordanGardenStockWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using JordanGardenStockWebAPI.Models;

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

    /// <summary>
    /// 依 ID 取得商品
    /// </summary>
    /// <param name="CompanyId">公司 ID</param>
    /// <param name="TillandsiaId">空氣鳳梨 ID</param>
    /// <returns>商品</returns>
    [HttpGet("{Id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<APIResult<Product>> GetById(int CompanyId, int TillandsiaId)
    {
        APIResult<Product> apiResult = new APIResult<Product>();

        //檢查商品是否存在
        if (!_service.IsExist(CompanyId, TillandsiaId))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The product is not exist";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        //取得資料
        Product? product = _service.GetProduct(CompanyId, TillandsiaId);

        apiResult.Succ = true;
        apiResult.Message = "Success";
        apiResult.Data = product;

        _logger.LogInformation(apiResult.Message);
        return apiResult;
    }

    /// <summary>
    /// 依條件取得商品
    /// </summary>
    /// <param name="SearchString">欲搜尋字串</param>
    /// <param name="CompanyId">公司 ID</param>
    /// <param name="Page">頁數，為空時取得所有商品</param>
    /// <returns>當前頁商品</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<APIResult<List<Product>>> GetByCondition(string? SearchString, int CompanyId, int Page = 0)
    {
        APIResult<List<Product>> apiResult = new APIResult<List<Product>>();

        //取得資料
        string searchString = SearchString ?? string.Empty;
        List<Product> products = _service.GetProducts(Page, CompanyId, searchString);

        //檢查是否有資料
        if (products.Count is 0)
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The page is null";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        apiResult.Succ = true;
        apiResult.Message = "Success";
        apiResult.Data = products;

        _logger.LogInformation(apiResult.Message);
        return apiResult;
    }

    /// <summary>
    /// 取得頁數資訊
    /// </summary>
    /// <param name="SearchString">欲搜尋字串</param>
    /// <param name="CompanyId">公司 ID</param>
    /// <returns>頁數資訊</returns>
    [HttpGet("PageInfo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<APIResult<PageInfo>> PageInfo(string? SearchString, int CompanyId)
    {
        string searchString = SearchString ?? string.Empty;
        _logger.LogInformation("Success");
        return new APIResult<PageInfo>()
        {
            Succ = true,
            Message = "Success",
            Data = _service.GetPageInfo(CompanyId, searchString)
        };
    }

    /// <summary>
    /// 新增商品
    /// </summary>
    /// <param name="CompanyId">公司 ID</param>
    /// <param name="TillandsiaId">空氣鳳梨 ID</param>
    /// <param name="Product">商品資料</param>
    /// <returns>新增是否成功</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResult<Tillandsia>>> Add(int CompanyId, int TillandsiaId, [FromBody] Product Product)
    {
        APIResult<Tillandsia> apiResult = new APIResult<Tillandsia>();

        //檢查數據是否合法
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //檢查名稱是否存在
        if (_service.IsExist(CompanyId, TillandsiaId))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The product is alredy exist";

            _logger.LogInformation(apiResult.Message);
            return BadRequest(apiResult);
        }

        //新增資料
        await _service.AddProductAsync(Product);

        _logger.LogInformation("Success");
        return CreatedAtAction(nameof(GetById), new { Id = Product.Id }, Product);
    }

    /// <summary>
    /// 更新商品
    /// </summary>
    /// <param name="Id">商品 ID</param>
    /// <param name="Product">商品資料</param>
    /// <returns>更新是否成功</returns>
    [HttpPut("{Id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResult<bool>>> Update(int Id, [FromBody] Product Product)
    {
        APIResult<bool> apiResult = new APIResult<bool>();

        //檢查數據是否合法
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //檢查 ID 是否存在
        if (!_service.IsExist(Id))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The id is not exist";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        //更新資料
        await _service.UpdateProductAsync(Id, Product);

        _logger.LogInformation("Success");
        return NoContent();
    }

    /// <summary>
    /// 刪除商品
    /// </summary>
    /// <param name="Id">商品 ID</param>
    /// <returns>刪除是否成功</returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResult<bool>>> Delete(int Id)
    {
        APIResult<bool> apiResult = new APIResult<bool>();

        //檢查 ID 是否存在
        if (!_service.IsExist(Id))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The id is not exist";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        //刪除資料
        await _service.DeleteProductAsync(Id);
        apiResult.Succ = true;
        apiResult.Message = "Success";
        apiResult.Data = true;

        _logger.LogInformation(apiResult.Message);
        return apiResult;
    }
}