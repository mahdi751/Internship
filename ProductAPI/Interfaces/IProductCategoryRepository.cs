using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.Interfaces
{
    public interface IProductCategoryRepository
    {
        public ICollection<Product> GetProductByCategotyId(int categoryid);

        bool CategoryExists(int categoryid);

    }
}
