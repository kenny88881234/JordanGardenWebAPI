public class TillandsiaService
{
    private readonly JordanGardenStockDbContext _db;

    public TillandsiaService(JordanGardenStockDbContext dbContext)
    {
        _db = dbContext;
    }
    public Tillandsia? GetTillandsia(int id)
    {
        return _db.Tillandsias.Where(t => t.Id == id).FirstOrDefault();
    }
}