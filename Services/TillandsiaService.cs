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
        return await _db.Tillandsias.FindAsync(id);
    }

    public List<Tillandsia> GetTillandsias(int page)
    {
        if(page is 0)
        {
            return _db.Tillandsias.ToList();
        }
        return _db.Tillandsias.Skip((page - 1) * DataNumPerPage).Take(DataNumPerPage).ToList();
    }

    public async Task<bool> AddTillandsiaAsync(Tillandsia tillandsia)
    {
        //判斷是否已存在
        if(_db.Tillandsias.Any(t => t.NameEng == tillandsia.NameEng))
        {
            return false;
        }

        await _db.Tillandsias.AddAsync(tillandsia);
        await _db.SaveChangesAsync();
        return true;
    }
}