using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Interfaces;
using ProductAPI.Models;
using ProductAPI.Repository;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPhotoController : ControllerBase
    {
        private readonly IProductPhotoRepository _productPhotoRepository;

        public ProductPhotoController(IProductPhotoRepository productPhotoRepository)
        {
            _productPhotoRepository = productPhotoRepository;
        }

        [HttpGet("/Get_Product_Thumbnail/{productID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProductThumbnailByID(int productID)
        {
            if (!await _productPhotoRepository.ProductExists(productID))
            {
                ModelState.AddModelError("", "ProductID entered does not exist!");
                return StatusCode(400, ModelState);
            }

            var productThambnailPhoto = await _productPhotoRepository.GetProductThumbnailPhoto(productID);
            if (productThambnailPhoto == null)
            {
                ModelState.AddModelError("", "ProductID entered does not have a thumbnailPhoto!");
                return StatusCode(400, ModelState);
            }

                if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(productThambnailPhoto);
        }


        [HttpGet("/Get_Product_Large_Photo/{productID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProductLargePhotoByID(int productID)
        {
            if (!await _productPhotoRepository.ProductExists(productID))
            {
                ModelState.AddModelError("", "ProductID entered does not exist!");
                return StatusCode(400, ModelState);
            }

            var productLargePhoto = await _productPhotoRepository.GetProductLargePhoto(productID);
            if (productLargePhoto == null)
            {
                ModelState.AddModelError("", "ProductID entered does not have a LargePhoto!");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(productLargePhoto);
        }

        [HttpGet("/Get_All_Product_Photos")]
        [ProducesResponseType(200, Type = typeof(List<ProductPhoto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllProducts()
        {
            var productPhotos =await _productPhotoRepository.GetAllProductPhotos();

            if (productPhotos == null)
            {
                ModelState.AddModelError("", "There is no Product Photos!");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(productPhotos);
        }
    }
}
