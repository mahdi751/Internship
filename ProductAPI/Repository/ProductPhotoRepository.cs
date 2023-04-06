using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Interfaces;
using ProductAPI.Models;

namespace ProductAPI.Repository
{
    public class ProductPhotoRepository : IProductPhotoRepository
    {
        private readonly DataContext _context;
        public ProductPhotoRepository (DataContext context)
        {
            _context = context;
        }

        public byte[] GetProductThumbnailPhoto(int productID)
        {
            var productThambnailPhoto = _context.ProductPhotos
             .FirstOrDefault
             (pp => _context.ProductProductPhotos
              .Any(ppp => ppp.ProductPhotoID == pp.ProductPhotoID && ppp.ProductID == productID && ppp.Primary)
             )?.ThumbnailPhoto;

            return productThambnailPhoto;
        }

        public byte[] GetProductLargePhoto(int productID)
        {
            var productLargePhoto = _context.ProductPhotos
             .FirstOrDefault
             (pp => _context.ProductProductPhotos
              .Any(ppp => ppp.ProductPhotoID == pp.ProductPhotoID && ppp.ProductID == productID && ppp.Primary)
             )?.LargePhoto;

            return productLargePhoto;
        }

        public bool ProductExists(int productID)
        {
            return _context.Products.Any(p => p.ProductID == productID);
        }

        public ICollection<ProductPhoto> getAllProductPhotos()
        {
            return _context.ProductPhotos.OrderBy(p => p.ProductPhotoID).ToList();
        }

    }
}
