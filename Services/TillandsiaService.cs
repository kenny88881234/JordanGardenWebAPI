public class TillandsiaService
{
    private readonly JordanGardenStockDbContext _db;
    private static readonly int DataNumPerPage = 20;

    public TillandsiaService(JordanGardenStockDbContext dbContext)
    {
        _db = dbContext;
    }
    
    public Tillandsia? GetTillandsia(int id)
    {
        return _db.Tillandsias.Where(t => t.Id == id).FirstOrDefault();
    }

    public List<Tillandsia> GetTillandsias(int page)
    {
        if(page is 0)
        {
            return _db.Tillandsias.ToList();
        }
        return _db.Tillandsias.Skip((page - 1) * DataNumPerPage).Take(DataNumPerPage).ToList();
    }
}