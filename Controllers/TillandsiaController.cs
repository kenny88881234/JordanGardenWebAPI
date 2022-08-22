using JordanGardenStockWebAPI.Extensions;
using JordanGardenStockWebAPI.Models;
using JordanGardenStockWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace JordanGardenStockWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TillandsiaController : ControllerBase
{
    private readonly ILogger<TillandsiaController> _logger;
    private readonly TillandsiaService _service;

    public TillandsiaController(ILogger<TillandsiaController> logger, TillandsiaService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// 依 ID 取得空氣鳳梨
    /// </summary>
    /// <param name="Id">空氣鳳梨 ID</param>
    /// <returns>空氣鳳梨</returns>
    [HttpGet("{Id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResult<Tillandsia>>> GetById(int Id)
    {
        APIResult<Tillandsia> apiResult = new APIResult<Tillandsia>();

        //檢查 ID 是否存在
        if (!_service.IsIDExist(Id))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The id is not exist";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        //取得資料
        Tillandsia? tillandsia = await _service.GetTillandsiaAsync(Id);

        apiResult.Succ = true;
        apiResult.Message = "Success";
        apiResult.Data = tillandsia;

        _logger.LogInformation(apiResult.Message);
        return apiResult;
    }

    /// <summary>
    /// 依條件取得空氣鳳梨
    /// </summary>
    /// <param name="SearchString">欲搜尋字串</param>
    /// <param name="Page">頁數，為空時取得所有空氣鳳梨</param>
    /// <returns>當前頁空氣鳳梨</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<APIResult<List<Tillandsia>>> GetByCondition(string? SearchString, int Page = 0)
    {
        APIResult<List<Tillandsia>> apiResult = new APIResult<List<Tillandsia>>();

        //取得資料
        string searchString = SearchString ?? string.Empty;
        List<Tillandsia> tillandsias = _service.GetTillandsias(Page, searchString);

        //檢查是否有資料
        if (tillandsias.Count is 0)
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The page is null";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        apiResult.Succ = true;
        apiResult.Message = "Success";
        apiResult.Data = tillandsias;

        _logger.LogInformation(apiResult.Message);
        return apiResult;
    }

    /// <summary>
    /// 取得頁數資訊
    /// </summary>
    /// /// <param name="SearchString">欲搜尋字串</param>
    /// <returns>頁數資訊</returns>
    [HttpGet("PageInfo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<APIResult<PageInfo>> PageInfo(string? SearchString)
    {
        string searchString = SearchString ?? string.Empty;
        _logger.LogInformation("Success");
        return new APIResult<PageInfo>()
        {
            Succ = true,
            Message = "Success",
            Data = _service.GetPageInfo(searchString)
        };
    }

    /// <summary>
    /// 新增空氣鳳梨
    /// </summary>
    /// <param name="Tillandsia">空氣鳳梨資料</param>
    /// <returns>新增是否成功</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResult<Tillandsia>>> Add([FromBody] Tillandsia Tillandsia)
    {
        APIResult<Tillandsia> apiResult = new APIResult<Tillandsia>();

        //檢查數據是否合法
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //檢查名稱是否存在
        if (_service.IsNameExist(Tillandsia.NameEng))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The name is alredy exist";

            _logger.LogInformation(apiResult.Message);
            return BadRequest(apiResult);
        }

        //新增資料
        await _service.AddTillandsiaAsync(Tillandsia);

        _logger.LogInformation("Success");
        return CreatedAtAction(nameof(GetById), new { Id = Tillandsia.Id }, Tillandsia);
    }

    /// <summary>
    /// 更新空氣鳳梨
    /// </summary>
    /// <param name="Id">空氣鳳梨 ID</param>
    /// <param name="Tillandsia">空氣鳳梨資料</param>
    /// <returns>更新是否成功</returns>
    [HttpPut("{Id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResult<bool>>> Update(int Id, [FromBody] Tillandsia Tillandsia)
    {
        APIResult<bool> apiResult = new APIResult<bool>();

        //檢查數據是否合法
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //檢查 ID 是否存在
        if (!_service.IsIDExist(Id))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The id is not exist";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        //檢查名稱是否存在
        if (_service.IsNameExist(Tillandsia.NameEng, Id))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The name is alredy exist";

            _logger.LogInformation(apiResult.Message);
            return BadRequest(apiResult);
        }

        //更新資料
        await _service.UpdateTillandsiaAsync(Id, Tillandsia);

        _logger.LogInformation("Success");
        return NoContent();
    }

    /// <summary>
    /// 刪除空氣鳳梨
    /// </summary>
    /// <param name="Id">空氣鳳梨 ID</param>
    /// <returns>刪除是否成功</returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResult<bool>>> Delete(int Id)
    {
        APIResult<bool> apiResult = new APIResult<bool>();

        //檢查 ID 是否存在
        if (!_service.IsIDExist(Id))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The id is not exist";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        //刪除資料
        await _service.DeleteTillandsiaAsync(Id);
        apiResult.Succ = true;
        apiResult.Message = "Success";
        apiResult.Data = true;

        _logger.LogInformation(apiResult.Message);
        return apiResult;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResult<string>>> UploadImageAsync([FromBody] IFormFile Image)
    {
        APIResult<string> apiResult = new APIResult<string>();

        //檢查是否為相片
        if(!Image.IsImage())
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The file type is not allow";

            _logger.LogInformation(apiResult.Message);
            return BadRequest(apiResult);
        }

        //儲存相片
        apiResult.Succ = true;
        apiResult.Message = "Success";
        apiResult.Data = await _service.AddImageAsync(Image);

        _logger.LogInformation(apiResult.Message);
        return apiResult;
    }
}
