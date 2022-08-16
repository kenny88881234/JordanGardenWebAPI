using JordanGardenStockWebAPI.Models;

namespace JordanGardenStockWebAPI.Services;

public class TillandsiaService
{
    private readonly JordanGardenStockDbContext _db;
    private static readonly int DataNumPerPage = 20;

    public TillandsiaService(JordanGardenStockDbContext dbContext)
    {
        _db = dbContext;
    }

    public bool IsIDExist(int id)
    {
        return _db.Tillandsias.Any(t => t.Id == id);
    }

    public bool IsNameExist(string name, int excludeId = 0)
    {
        return _db.Tillandsias.Any(t => t.NameEng == name && t.Id != excludeId);
    }

    public async Task<Tillandsia?> GetTillandsiaAsync(int id)
    {
        //回傳該筆資料
        return await _db.Tillandsias.FindAsync(id);
    }

    public List<Tillandsia> GetTillandsias(int page, string searchString)
    {
        //page 為 0 時回傳所有資料
        if (page is 0)
        {
            return _db.Tillandsias.Where(t => t.NameEng.Contains(searchString) || (t.NameChi == null ? false : t.NameChi.Contains(searchString))).OrderBy(t => t.NameEng).ToList();
        }

        //回傳當頁資料
        return _db.Tillandsias.Where(t => t.NameEng.Contains(searchString) || (t.NameChi == null ? false : t.NameChi.Contains(searchString))).Skip((page - 1) * DataNumPerPage).Take(DataNumPerPage).OrderBy(t => t.NameEng).ToList();
    }

    public PageInfo GetPageInfo()
    {
        int totalDataNum = _db.Tillandsias.Count();
        return new PageInfo()
        {
            DataNumPerPage = DataNumPerPage,
            TotalDataNum = totalDataNum,
            TotalPageNum = !(totalDataNum % DataNumPerPage is 0) ? (totalDataNum / DataNumPerPage) + 1 : totalDataNum / DataNumPerPage
        };
    }

    public async Task<bool> AddTillandsiaAsync(Tillandsia tillandsia)
    {
        //新增
        await _db.Tillandsias.AddAsync(tillandsia);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateTillandsiaAsync(int id, Tillandsia tillandsia)
    {
        Tillandsia? oldTillandsia = await _db.Tillandsias.FindAsync(id);
        if (oldTillandsia is null)
        {
            return false;
        }

        //更新
        oldTillandsia.NameEng = tillandsia.NameEng;
        oldTillandsia.NameChi = tillandsia.NameChi;
        oldTillandsia.Image = tillandsia.Image;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTillandsiaAsync(int id)
    {
        //刪除
        if (await _db.Tillandsias.FindAsync(id) is Tillandsia tillandsia)
        {
            _db.Tillandsias.Remove(tillandsia);
            await _db.SaveChangesAsync();
            return true;
        }

        return false;
    }
}