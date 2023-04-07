using System.Collections.Generic;
using System.Threading.Tasks;
using ProductAPI.Models;

namespace ProductAPI.Interfaces
{
    public interface IProductRepository
    {
        public Task<ICollection<Product>> GetAllProducts();
        public Task<bool> CreateProduct(Product product);
        public Task<bool> UpdateProduct(Product product);
        public Task<bool> DeleteProduct(Product product);
        public Task<Product> GetProductByID(int productID);

        //search for unique attributes
        public Task<Product> GetProductByProductNumber(Product product);
        public Task<Product> GetProductByProductName(Product product);
        public Task<Product> GetProductByProductRowguid(Product product);

        //Check if product id is a fk in other tables
        public Task<bool> ProductHasPhotos(int productID);
        public Task<bool> ProductHasInventory(int productID);

        //Check the existance
        public Task<bool> ProductExists(int productID);
        public Task<bool> SubCategoryExist(int? productSubcategoryID);
        public Task<bool> ModelExist(int? modelID);

        //Saving
        public Task<bool> Save();
    }
}
