using JordanGardenStockWebAPI.Models;

namespace JordanGardenStockWebAPI.Services;

public class ProductService
{
    private readonly JordanGardenStockDbContext _db;
    private static readonly int DataNumPerPage = 20;

    public ProductService(JordanGardenStockDbContext dbContext)
    {
        _db = dbContext;
    }

    public bool IsExist(int companyId, int tillandsiaId)
    {
        return _db.Products.Any(p => p.CompanyId == companyId && p.TillandsiaId == tillandsiaId);
    }

    public Product? GetProduct(int companyId, int tillandsiaId)
    {
        return _db.Products.Where(p => p.CompanyId == companyId && p.TillandsiaId == tillandsiaId).FirstOrDefault();
    }

    public List<Product> GetProducts(int page, int companyId, string searchString)
    {
        //依搜尋字串取得空氣鳳梨
        List<Tillandsia> tillandsias = _db.Tillandsias.Where(t => t.NameEng.ToLower().Contains(searchString.ToLower()) || (t.NameChi == null ? false : t.NameChi.Contains(searchString))).ToList();
        //依公司 ID 取得其商品
        List<Product> productsByCompany = _db.Products.Where(p => p.CompanyId == companyId).ToList();

        //page 為 0 時回傳所有資料
        if (page is 0)
        {
            return productsByCompany.Where(pc => tillandsias.Contains(pc.Tillandsia)).OrderBy(pc => pc.Tillandsia.NameEng).ToList();
        }

        //回傳當頁資料
        return productsByCompany.Where(pc => tillandsias.Contains(pc.Tillandsia)).Skip((page - 1) * DataNumPerPage).Take(DataNumPerPage).OrderBy(pc => pc.Tillandsia.NameEng).ToList();
    }

    public PageInfo GetPageInfo(int companyId, string searchString)
    {
        //依搜尋字串取得空氣鳳梨
        List<Tillandsia> tillandsias = _db.Tillandsias.Where(t => t.NameEng.ToLower().Contains(searchString.ToLower()) || (t.NameChi == null ? false : t.NameChi.Contains(searchString))).ToList();
        //依公司 ID 取得其商品
        List<Product> productsByCompany = _db.Products.Where(p => p.CompanyId == companyId).ToList();

        int totalDataNum = productsByCompany.Count();
        return new PageInfo()
        {
            DataNumPerPage = DataNumPerPage,
            TotalDataNum = totalDataNum,
            TotalPageNum = !(totalDataNum % DataNumPerPage is 0) ? (totalDataNum / DataNumPerPage) + 1 : totalDataNum / DataNumPerPage
        };
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        //新增
        await _db.Products.AddAsync(product);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateProductAsync(int id, Product product)
    {
        Product? oldProduct = await _db.Products.FindAsync(id);
        if (oldProduct is null)
        {
            return false;
        }

        //更新
        oldProduct.CompanyId = product.CompanyId;
        oldProduct.TillandsiaId = product.TillandsiaId;
        oldProduct.Price = product.Price;
        oldProduct.Size = product.Size;
        oldProduct.Quantity = product.Quantity;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        //刪除
        if (await _db.Products.FindAsync(id) is Product product)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }

        return false;
    }
}