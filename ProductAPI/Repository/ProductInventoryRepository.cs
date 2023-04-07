using ProductAPI.Data;
using ProductAPI.Interfaces;
using ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Repository
{
    public class ProductInventoryRepository : IProductInventoryRepository
    {
        private readonly DataContext _context;
        public ProductInventoryRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateProductInventory(ProductInventory productInventory)
        {
            await _context.AddAsync(productInventory);
            return await Save();
        }

        public async Task<ProductInventory> GetProductInventoryByProductID_LocationID(int productid, int locationid)
        {
            return await _context.ProductInventories
                .Where(pi => pi.ProductID == productid && pi.LocationID == locationid)
                .Select(pi => new ProductInventory
                {
                    ProductID = Convert.ToInt32(pi.ProductID),
                    LocationID = Convert.ToInt32(pi.LocationID),
                    Shelf = pi.Shelf,
                    Bin = Convert.ToInt32(pi.Bin),
                    Quantity = Convert.ToInt32(pi.Quantity),
                    rowguid = pi.rowguid,
                    ModifiedDate = pi.ModifiedDate
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProductInventory(ProductInventory productInventory)
        {
            _context.Update(productInventory);
            return await Save();
        }

        public async Task<ICollection<Product>> GetProductsByShelf(string shelf)
        {
            var productsIDList = await _context.ProductInventories
                .Where(pi => pi.Shelf == shelf)
                .Select(pi => pi.ProductID)
                .Distinct()
                .ToListAsync();

            var allProducts = await _context.Products
            .Where(p => productsIDList.Contains(p.ProductID))
            .ToListAsync();

            return allProducts;
        }

        public async Task<ICollection<Object>> GetProductsQuantity()
        {
            var productQuantities = await _context.ProductInventories
                .GroupBy(pi => pi.ProductID)
                .Select(g => new
                {
                    //g.key that is the key used for grouping
                    ProductID = g.Key,
                    TotalQuantity = g.Sum(pi => pi.Quantity)
                })
                .ToListAsync();

            return productQuantities.Select(q => new Dictionary<string, int>
            {
                { "ProductID", q.ProductID },
                { "TotalQuantity", q.TotalQuantity }

            }).ToList<object>();
        }


        public async Task<bool> ProductExists(int productID)
        {
            return await _context.Products.AnyAsync(p => p.ProductID == productID);
        }

        public async Task<bool> LocationExists(int locationID)
        {
            return await _context.Locations.AnyAsync(p => p.LocationID == locationID);
        }

        public async Task<bool> ShelfExist(string shelf)
        {
            return await _context.ProductInventories.AnyAsync(p => p.Shelf == shelf);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

    }
}
