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
        if(!_service.IDIsExist(Id))
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
    /// 依頁數取得空氣鳳梨
    /// </summary>
    /// <param name="Page">頁數，為空時取得所有空氣鳳梨</param>
    /// <param name="SearchString">欲搜尋字串</param>
    /// <returns>當前頁空氣鳳梨</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<APIResult<List<Tillandsia>>> GetByPageAndSearchString(string? SearchString, int Page = 0)
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
        if(_service.NameIsExist(Tillandsia.NameEng))
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
        if(!_service.IDIsExist(Id))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The id is not exist";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        //檢查名稱是否存在
        if(_service.NameIsExist(Tillandsia.NameEng, Id))
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
        if(!_service.IDIsExist(Id))
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
}
