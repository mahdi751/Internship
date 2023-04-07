using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.Interfaces
{
    public interface IProductCategoryRepository
    {
        public Task<ICollection<Product>> GetProductByCategotyId(int categoryid);
        public Task<bool> CreateProductCategory(ProductCategory category);
        public Task<bool> UpdateCategory(ProductCategory category);
        public Task<bool> DeleteCategory(ProductCategory category);
        public Task<ProductCategory> getCategoryByID(int categoryID);

        //search for unique attributes
        public Task<ProductCategory> GetCategoryByRowguid(ProductCategory category);
        public Task<ProductCategory> GetCategoryByName(ProductCategory category);

        //search if their is any sub categories related to the category
        public Task<bool> CategoryHasSubCategories(int categoryID);

        public Task<bool> CategoryExists(int categoryid);
        public Task<bool> Save();

    }
}
