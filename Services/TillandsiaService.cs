public class TillandsiaService
{
    private readonly JordanGardenStockDbContext _db;
    private static readonly int DataNumPerPage = 20;

    public TillandsiaService(JordanGardenStockDbContext dbContext)
    {
        _db = dbContext;
    }
    
    public async Task<Tillandsia?> GetTillandsiaAsync(int id)
    {
        //回傳該筆資料
        return await _db.Tillandsias.FindAsync(id);
    }

    public List<Tillandsia> GetTillandsias(int page, string SearchString)
    {
        //page 為 0 時回傳所有資料
        if(page is 0)
        {
            return _db.Tillandsias.Where(t => t.NameEng.Contains(SearchString) || t.NameChi == null ? false : t.NameChi.Contains(SearchString)).ToList();
        }

        //回傳當頁資料
        return _db.Tillandsias.Where(t => t.NameEng.Contains(SearchString) || t.NameChi == null ? false : t.NameChi.Contains(SearchString)).Skip((page - 1) * DataNumPerPage).Take(DataNumPerPage).ToList();
    }

    public async Task<bool> AddTillandsiaAsync(Tillandsia tillandsia)
    {
        //檢查是否已存在
        if(_db.Tillandsias.Any(t => t.NameEng == tillandsia.NameEng))
        {
            return false;
        }

        //新增
        await _db.Tillandsias.AddAsync(tillandsia);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateTillandsiaAsync(int id, Tillandsia tillandsia)
    {
        Tillandsia? oldTillandsia = await _db.Tillandsias.FindAsync(id);

        //檢查是否存在
        if(oldTillandsia is null)
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
        Tillandsia? oldTillandsia = await _db.Tillandsias.FindAsync(id);

        //檢查是否存在，並刪除
        if(await _db.Tillandsias.FindAsync(id) is Tillandsia tillandsia)
        {
            _db.Tillandsias.Remove(tillandsia);
            await _db.SaveChangesAsync();
            return true;
        }

        return false;
    }
}