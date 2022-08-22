using JordanGardenStockWebAPI.Models;

namespace JordanGardenStockWebAPI.Services;

public class CompanyService
{
    private readonly JordanGardenStockDbContext _db;
    private static readonly int DataNumPerPage = 20;

    public CompanyService(JordanGardenStockDbContext dbContext)
    {
        _db = dbContext;
    }

    public bool IsIDExist(int id)
    {
        return _db.Companies.Any(c => c.Id == id);
    }

    public bool IsNameExist(string name, int excludeId = 0)
    {
        return _db.Companies.Any(c => c.Name == name && c.Id != excludeId);
    }

    public bool IsMailExist(string mail, int excludeId = 0)
    {
        return _db.Companies.Any(c => c.Mail == mail && c.Id != excludeId);
    }

    public async Task<Company?> GetCompanyAsync(int id)
    {
        //回傳該筆資料
        return await _db.Companies.FindAsync(id);
    }

    public List<Company> GetCompanies(int page, string searchString, string country)
    {
        //page 為 0 時回傳所有資料
        if (page is 0)
        {
            return _db.Companies.Where(c => c.Name.ToLower().Contains(searchString.ToLower()) && c.Country.Contains(country)).OrderBy(t => t.Name).ToList();
        }

        //回傳當頁資料
        return _db.Companies.Where(c => c.Name.ToLower().Contains(searchString.ToLower()) && c.Country.Contains(country)).Skip((page - 1) * DataNumPerPage).Take(DataNumPerPage).OrderBy(t => t.Name).ToList();
    }

    public PageInfo GetPageInfo(string searchString, string country)
    {
        int totalDataNum = _db.Companies.Where(c => c.Name.ToLower().Contains(searchString.ToLower()) && c.Country.Contains(country)).Count();
        return new PageInfo()
        {
            DataNumPerPage = DataNumPerPage,
            TotalDataNum = totalDataNum,
            TotalPageNum = !(totalDataNum % DataNumPerPage is 0) ? (totalDataNum / DataNumPerPage) + 1 : totalDataNum / DataNumPerPage
        };
    }

    public async Task<bool> AddCompanyAsync(Company company)
    {
        //新增
        await _db.Companies.AddAsync(company);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateCompanyAsync(int id, Company company)
    {
        Company? oldCompany = await _db.Companies.FindAsync(id);
        if (oldCompany is null)
        {
            return false;
        }

        //更新
        oldCompany.Name = company.Name;
        oldCompany.Mail = company.Mail;
        oldCompany.Country = company.Country;
        oldCompany.Address = company.Address;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCompanyAsync(int id)
    {
        //刪除
        if (await _db.Companies.FindAsync(id) is Company company)
        {
            _db.Companies.Remove(company);
            await _db.SaveChangesAsync();
            return true;
        }

        return false;
    }
}