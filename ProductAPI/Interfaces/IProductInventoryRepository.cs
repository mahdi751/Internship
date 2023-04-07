using ProductAPI.Models;

namespace ProductAPI.Interfaces
{
    public interface IProductInventoryRepository
    {
        public Task<bool> CreateProductInventory(ProductInventory productInventory);
        public Task<ProductInventory> GetProductInventoryByProductID_LocationID(int productid, int locationid);
        public Task<bool> UpdateProductInventory(ProductInventory productInventory);
        public Task<ICollection<Object>> GetProductsQuantity();
        public Task<ICollection<Product>> GetProductsByShelf(string shelf);

        //Check if exist functions 
        public Task<bool> ShelfExist(string shelf);
        public Task<bool> ProductExists(int productID);
        public Task<bool> LocationExists(int locationID);

        //Save
        public Task<bool> Save();
    }
}
