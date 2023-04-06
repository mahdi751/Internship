using ProductAPI.Models;

namespace ProductAPI.Interfaces
{
    public interface IProductInventoryRepository
    {
        public bool CreateProductInventory(ProductInventory productInventory);
        public ProductInventory GetProductInventoryByProductID_LocationID(int productid, int locationid);
        public bool UpdateProductInventory(ProductInventory productInventory);
        public ICollection<Product> GetProductsByShelf(string shelf);
        public ICollection<object> GetProductsQuantity();

        //Check if exist functions 
        public bool ShelfExist(string shelf);
        public bool ProductExists(int productID);
        public bool LocationExists(int locationID);
        public bool Save();
    }
}
