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
    /// 依ID取得空氣鳳梨
    /// </summary>
    /// <param name="Id">空氣鳳梨ID</param>
    /// <returns>空氣鳳梨</returns>
    [HttpGet("{Id:int}")]
    public ActionResult<APIResult<Tillandsia>> GetById(int Id)
    {
        APIResult<Tillandsia> apiResult = new APIResult<Tillandsia>();

        Tillandsia? tillandsia = _service.GetTillandsia((int)Id);
        if (tillandsia is null)
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The id is not exist";

            _logger.LogInformation(apiResult.Message);
            return BadRequest(apiResult);
        }

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
    /// <returns>當前頁空氣鳳梨</returns>
    [HttpGet]
    public ActionResult<APIResult<List<Tillandsia>>> GetByPage(int Page = 0)
    {
        APIResult<List<Tillandsia>> apiResult = new APIResult<List<Tillandsia>>();

        List<Tillandsia> tillandsias = _service.GetTillandsias(Page);

        apiResult.Succ = true;
        apiResult.Message = "Success";
        apiResult.Data = tillandsias;

        _logger.LogInformation(apiResult.Message);
        return apiResult;
    }
}
