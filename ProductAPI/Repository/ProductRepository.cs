using Microsoft.EntityFrameworkCore;

using ProductAPI.Data;
using ProductAPI.Interfaces;
using ProductAPI.Models;

namespace ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Product>> GetAllProducts()
        {
            return await _context.Products.OrderBy(p => p.ProductID).ToListAsync();
        }

        public async Task<bool> CreateProduct(Product product)
        {
            await _context.AddAsync(product);
            return await Save();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            _context.Update(product);
            return await Save();
        }

        public async Task<Product> GetProductByID(int productID)
        {
            return await _context.Products.Where(p => p.ProductID == productID).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            _context.Remove(product);
            return await Save();
        }

        public async Task<Product> GetProductByProductNumber(Product product)
        {
            return await _context.Products
            .Where(p => p.ProductNumber == product.ProductNumber && p.ProductID != product.ProductID)
            .FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByProductRowguid(Product product)
        {
            return await _context.Products
            .Where(p => p.Rowguid == product.Rowguid && p.ProductID != product.ProductID)
            .FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByProductName(Product product)
        {
            return await _context.Products
            .Where(p => p.Name == product.Name && p.ProductID != product.ProductID)
            .FirstOrDefaultAsync();
        }

        public async Task<bool> ProductHasPhotos(int productID)
        {
            return await _context.ProductProductPhotos
            .Where(psc => psc.ProductID == productID)
            .AnyAsync();
        }

        public async Task<bool> ProductHasInventory(int productID)
        {
            return await _context.ProductInventories
            .Where(psc => psc.ProductID == productID)
            .AnyAsync();
        }

        public async Task<bool> ProductExists(int productID)
        {
            return await _context.Products.AnyAsync(p => p.ProductID == productID);
        }

        public async Task<bool> SubCategoryExist(int? productSubcategoryID)
        {
            return await _context.productSubcategories.AnyAsync(p => p.ProductSubcategoryID == productSubcategoryID);
        }

        public async Task<bool> ModelExist(int? modelID)
        {
            return await _context.ProductModels.AnyAsync(p => p.ProductModelId == modelID);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }

}
