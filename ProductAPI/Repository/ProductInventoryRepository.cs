using Microsoft.AspNetCore.Mvc.Infrastructure;
using ProductAPI.Data;
using ProductAPI.Interfaces;
using ProductAPI.Models;
using System.Text.RegularExpressions;

namespace ProductAPI.Repository
{
    public class ProductInventoryRepository : IProductInventoryRepository
    {
        private readonly DataContext _context;
        public ProductInventoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateProductInventory(ProductInventory productInventory)
        {
            _context.Add(productInventory);
            return Save();
        }

        public ProductInventory GetProductInventoryByProductID_LocationID(int productid, int locationid)
        {
            return _context.ProductInventories
                .Where(pi=> pi.ProductID == productid && pi.LocationID == locationid)
                .Select(pi => new ProductInventory
            {
                ProductID = Convert.ToInt32(pi.ProductID),
                LocationID = Convert.ToInt32(pi.LocationID),
                Shelf = pi.Shelf,
                Bin = Convert.ToInt32(pi.Bin),
                Quantity = Convert.ToInt32(pi.Quantity),
                rowguid = pi.rowguid,
                ModifiedDate = pi.ModifiedDate
            }).FirstOrDefault();
        }

        public bool UpdateProductInventory(ProductInventory productInventory)
        {
            _context.Update(productInventory);
            return Save();
        }

        public ICollection<Product> GetProductsByShelf(string shelf)
        {
            var productsIDList = _context.ProductInventories
                .Where(pi => pi.Shelf == shelf)
                .Select(pi => pi.ProductID)
                .Distinct()
                .ToList();

            var allProducts = _context.Products
            .Where(p => productsIDList.Contains(p.ProductID))
            .ToList();

            return allProducts;
        }

        public ICollection<object> GetProductsQuantity()
        {
            var productQuantities = _context.ProductInventories
                .GroupBy(pi => pi.ProductID)
                .Select(g => new
                {
                    //g.key that is the key used for grouping
                    ProductID = g.Key,
                    TotalQuantity = g.Sum(pi => pi.Quantity)
                })
                .ToList();

            //since productQuantities variable is an anonymous type due to the select we need to cast it to be compatible with the return
            return (ICollection<object>)productQuantities;
        }

        public bool ProductExists(int productID)
        {
            return _context.Products.Any(p => p.ProductID == productID);
        }

        public bool LocationExists(int locationID)
        {
            return _context.Locations.Any(p => p.LocationID == locationID);
        }

        public bool ShelfExist(string shelf)
        {
            return _context.ProductInventories.Any(p => p.Shelf == shelf);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
