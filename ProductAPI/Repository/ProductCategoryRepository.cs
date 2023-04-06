using ProductAPI.Data;
using ProductAPI.Interfaces;
using ProductAPI.Models;

namespace ProductAPI.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly DataContext _context;

        public ProductCategoryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CategoryExists(int categoryid)
        {
            return _context.productSubcategories.Any(c => c.ProductSubcategoryID == categoryid);
        }

        public ICollection<Product> GetProductByCategotyId(int categoryid)
        {
            var products = _context.Products
            .Where(p => _context.productSubcategories 
                        .Any (ps => ps.ProductSubcategoryID == p.ProductSubcategoryID && 
                        ps.ProductCategoryID == categoryid)
             ).ToList();

            return products;
        }

    }
}
