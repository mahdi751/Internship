using System.Collections;
using ProductAPI.Models;

namespace ProductAPI.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetAllProducts();
        public ICollection<Product> GetAllProductsWithout(int productid);
        bool CreateProduct(Product product);
        bool DeleteProduct(Product product);
        Product getProductByID(int productID);
        public bool UpdateProduct(Product product);
        bool Save();

        bool ProductExists(int productID);
        public bool SubCategoryExist(int? productSubcategoryID);
        public bool ModelExist(int? modelID);
    }
}
