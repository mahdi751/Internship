using ProductAPI.Models;

namespace ProductAPI.Interfaces
{
    public interface IProductPhotoRepository
    {
        public byte[] GetProductThumbnailPhoto(int productID);
        public byte[] GetProductLargePhoto(int productID);
        public bool ProductExists(int productID);
        public ICollection<ProductPhoto> getAllProductPhotos();

    }
}
