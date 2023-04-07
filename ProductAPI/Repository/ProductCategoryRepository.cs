using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Interfaces;
using ProductAPI.Models;
using System.Threading.Tasks;

namespace ProductAPI.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly DataContext _context;

        public ProductCategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Product>> GetProductByCategotyId(int categoryid)
        {
            var products = await _context.Products
            .Where(p => _context.productSubcategories
                        .Any(ps => ps.ProductSubcategoryID == p.ProductSubcategoryID &&
                        ps.ProductCategoryID == categoryid)
             ).ToListAsync();

            return products;
        }

        public async Task<bool> CreateProductCategory(ProductCategory category)
        {
            await _context.AddAsync(category);
            return await Save();
        }

        public async Task<bool> UpdateCategory(ProductCategory category)
        {
            _context.Update(category);
            return await Save();
        }

        public async Task<bool> DeleteCategory(ProductCategory category)
        {
            _context.Remove(category);
            return await Save();
        }

        public async Task<ProductCategory> getCategoryByID(int categoryID)
        {
            return await _context.ProductCategories.FindAsync(categoryID);
        }

        public async Task<ProductCategory> GetCategoryByRowguid(ProductCategory category)
        {
            return await _context.ProductCategories
            .Where(p => p.rowguid == category.rowguid && p.ProductCategoryID != category.ProductCategoryID)
            .FirstOrDefaultAsync();
        }

        public async Task<ProductCategory> GetCategoryByName(ProductCategory category)
        {
            return await _context.ProductCategories
            .Where(p => p.Name == category.Name && p.ProductCategoryID != category.ProductCategoryID)
            .FirstOrDefaultAsync();
        }

        public async Task<bool> CategoryHasSubCategories(int categoryID)
        {
            return await _context.productSubcategories
            .Where(psc => psc.ProductCategoryID == categoryID)
            .AnyAsync();
        }

        public async Task<bool> CategoryExists(int categoryid)
        {
            return await _context.ProductCategories.AnyAsync(c => c.ProductCategoryID == categoryid);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
