using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Interfaces;
using ProductAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Repository
{
    public class ProductPhotoRepository : IProductPhotoRepository
    {
        private readonly DataContext _context;
        public ProductPhotoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GetProductThumbnailPhoto(int productID)
        {
            var productThambnailPhoto = await _context.ProductPhotos
             .Where(pp => _context.ProductProductPhotos
              .Any(ppp => ppp.ProductPhotoID == pp.ProductPhotoID && ppp.ProductID == productID && ppp.Primary))
             .Select(pp => pp.ThumbnailPhoto)
             .FirstOrDefaultAsync();

            return productThambnailPhoto;
        }

        public async Task<byte[]> GetProductLargePhoto(int productID)
        {
            var productLargePhoto = await _context.ProductPhotos
             .Where(pp => _context.ProductProductPhotos
              .Any(ppp => ppp.ProductPhotoID == pp.ProductPhotoID && ppp.ProductID == productID && ppp.Primary))
             .Select(pp => pp.LargePhoto)
             .FirstOrDefaultAsync();

            return productLargePhoto;
        }

        public async Task<bool> ProductExists(int productID)
        {
            return await _context.Products.AnyAsync(p => p.ProductID == productID);
        }

        public async Task<ICollection<ProductPhoto>> GetAllProductPhotos()
        {
            return await _context.ProductPhotos.OrderBy(p => p.ProductPhotoID).ToListAsync();
        }

    }
}
