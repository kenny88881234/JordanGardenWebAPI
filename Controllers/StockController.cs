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
    /// 取得空氣鳳梨
    /// </summary>
    /// <param name="Id">空氣鳳梨ID，為空時改為依頁數取得空氣鳳梨</param>
    /// <param name="Page">頁數，為空時取得所有空氣鳳梨</param>
    /// <returns>空氣鳳梨</returns>
    [HttpGet("{Id:int}")]
    public ActionResult<APIResult<object>> Get(int? Id, int Page = 0)
    {
        APIResult<object> apiResult = new APIResult<object>();

        if(Id.HasValue)
        {
            object? tillandsia = _service.GetTillandsia((int)Id);
            if (tillandsia is null)
            {
                apiResult.Succ = false;
                apiResult.ErrorCode = "";

                _logger.LogInformation("The id is not exist");
                return BadRequest(apiResult);
            }

            apiResult.Succ = true;
            apiResult.Data = tillandsia;
        }
        else
        {
            //object? tillandsias = _service.GetTillandsias(Page);
        }

        _logger.LogInformation("Success");
        return apiResult;
    }
}
