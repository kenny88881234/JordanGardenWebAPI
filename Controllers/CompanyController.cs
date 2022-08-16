using JordanGardenStockWebAPI.Models;
using JordanGardenStockWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace JordanGardenStockWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ILogger<CompanyController> _logger;
    private readonly CompanyService _service;

    public CompanyController(ILogger<CompanyController> logger, CompanyService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// 依 ID 取得公司
    /// </summary>
    /// <param name="Id">公司 ID</param>
    /// <returns>公司</returns>
    [HttpGet("{Id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResult<Company>>> GetById(int Id)
    {
        APIResult<Company> apiResult = new APIResult<Company>();

        //檢查 ID 是否存在
        if(!_service.IsIDExist(Id))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The id is not exist";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        //取得資料
        Company? tillandsia = await _service.GetCompanyAsync(Id);

        apiResult.Succ = true;
        apiResult.Message = "Success";
        apiResult.Data = tillandsia;

        _logger.LogInformation(apiResult.Message);
        return apiResult;
    }

    /// <summary>
    /// 依頁數取得公司
    /// </summary>
    /// <param name="Page">頁數，為空時取得所有公司</param>
    /// <param name="SearchString">欲搜尋字串</param>
    /// <returns>當前頁公司</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<APIResult<List<Company>>> GetByPageAndSearchString(string? SearchString, int Page = 0)
    {
        APIResult<List<Company>> apiResult = new APIResult<List<Company>>();

        //取得資料
        string searchString = SearchString ?? string.Empty;
        List<Company> tillandsias = _service.GetCompanies(Page, searchString);

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
    /// <returns>頁數資訊</returns>
    [HttpGet("PageInfo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<APIResult<PageInfo>> PageInfo()
    {
        return new APIResult<PageInfo>()
        {
            Succ = true,
            Message = "Success",
            Data = _service.GetPageInfo()
        };
    }

    /// <summary>
    /// 新增公司
    /// </summary>
    /// <param name="Company">公司資料</param>
    /// <returns>新增是否成功</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResult<Company>>> Add([FromBody] Company Company)
    {
        APIResult<Company> apiResult = new APIResult<Company>();

        //檢查數據是否合法
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //檢查名稱是否存在
        if(_service.IsNameExist(Company.Name))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The name is alredy exist";

            _logger.LogInformation(apiResult.Message);
            return BadRequest(apiResult);
        }

        //檢查信箱是否存在
        if(_service.IsMailExist(Company.Mail))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The mail is alredy exist";

            _logger.LogInformation(apiResult.Message);
            return BadRequest(apiResult);
        }

        //新增資料
        await _service.AddCompanyAsync(Company);

        _logger.LogInformation("Success");
        return CreatedAtAction(nameof(GetById), new { Id = Company.Id }, Company);
    }

    /// <summary>
    /// 更新公司
    /// </summary>
    /// <param name="Id">公司 ID</param>
    /// <param name="Company">公司資料</param>
    /// <returns>更新是否成功</returns>
    [HttpPut("{Id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResult<bool>>> Update(int Id, [FromBody] Company Company)
    {
        APIResult<bool> apiResult = new APIResult<bool>();

        //檢查數據是否合法
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //檢查 ID 是否存在
        if(!_service.IsIDExist(Id))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The id is not exist";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        //檢查名稱是否存在
        if(_service.IsNameExist(Company.Name, Id))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The name is alredy exist";

            _logger.LogInformation(apiResult.Message);
            return BadRequest(apiResult);
        }

        //檢查信箱是否存在
        if(_service.IsMailExist(Company.Mail))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The mail is alredy exist";

            _logger.LogInformation(apiResult.Message);
            return BadRequest(apiResult);
        }

        //更新資料
        await _service.UpdateCompanyAsync(Id, Company);

        _logger.LogInformation("Success");
        return NoContent();
    }

    /// <summary>
    /// 刪除公司
    /// </summary>
    /// <param name="Id">公司 ID</param>
    /// <returns>刪除是否成功</returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResult<bool>>> Delete(int Id)
    {
        APIResult<bool> apiResult = new APIResult<bool>();

        //檢查 ID 是否存在
        if(!_service.IsIDExist(Id))
        {
            apiResult.Succ = false;
            apiResult.ErrorCode = "";
            apiResult.Message = "The id is not exist";

            _logger.LogInformation(apiResult.Message);
            return NotFound(apiResult);
        }

        //刪除資料
        await _service.DeleteCompanyAsync(Id);
        apiResult.Succ = true;
        apiResult.Message = "Success";
        apiResult.Data = true;

        _logger.LogInformation(apiResult.Message);
        return apiResult;
    }
}