using ProductAPI.Models;

namespace ProductAPI.Interfaces
{
    public interface IProductPhotoRepository
    {
        public Task<byte[]> GetProductThumbnailPhoto(int productID);
        public Task<byte[]> GetProductLargePhoto(int productID);
        public Task<bool> ProductExists(int productID);
        public Task<ICollection<ProductPhoto>> GetAllProductPhotos();

    }
}
