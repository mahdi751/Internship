using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Interfaces;
using ProductAPI.Models;
using ProductAPI.Repository;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryController(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        [HttpGet("/Get_All_Products_By/{categoryID}")]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        public async Task<IActionResult> GetProductByCategoryID(int categoryID)
        {
            if (!await _productCategoryRepository.CategoryExists(categoryID))
            {
                ModelState.AddModelError("", "No Category with such id!");
                return StatusCode(400, ModelState);
            }

            var products = await _productCategoryRepository.GetProductByCategotyId(categoryID);

            if (products == null)
            {
                ModelState.AddModelError("", "No Products in this category!");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        [HttpPost("/Create_Category")]
        [ProducesResponseType(200, Type = typeof(List<ProductCategory>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] ProductCategory newCategory)
        {
            if (newCategory == null)
                return BadRequest(ModelState);

            var categoryId = newCategory.ProductCategoryID;
            if (await _productCategoryRepository.CategoryExists(categoryId))
            {
                ModelState.AddModelError("", "CategoryID already exist!");
                return StatusCode(400, ModelState);
            }

            if (await _productCategoryRepository.GetCategoryByRowguid(newCategory) != null)
            {

                ModelState.AddModelError("", "Rowguid is a unique attr.Rowguid entered already exist!");
                return StatusCode(400, ModelState);
            }

            if (await _productCategoryRepository.GetCategoryByName(newCategory) != null)
            {

                ModelState.AddModelError("", "Name is a unique attr.Name entered already exist!");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _productCategoryRepository.CreateProductCategory(newCategory))
            {
                ModelState.AddModelError("", "Process interrupted!Couldn't add category");
                return StatusCode(400, ModelState);
            }

            return Ok("Category Successfully Created");

        }

        [HttpPut("/Update_Category/{categoryID}")]
        [ProducesResponseType(200, Type = typeof(List<ProductCategory>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProduct(int categoryID, [FromBody] ProductCategory updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);

            var categoryId = updatedCategory.ProductCategoryID;
            if (categoryId != categoryID)
            {
                ModelState.AddModelError("", "CategoryIDs not compatible!");
                return StatusCode(400, ModelState);
            }

            if (!await _productCategoryRepository.CategoryExists(categoryID))
            {
                ModelState.AddModelError("", "CategoryID not found!");
                return StatusCode(400, ModelState);
            }

            if (await _productCategoryRepository.GetCategoryByRowguid(updatedCategory) != null)
            {

                ModelState.AddModelError("", "Rowguid is a unique attr.Rowguid entered already exist!");
                return StatusCode(400, ModelState);
            }

            if (await _productCategoryRepository.GetCategoryByName(updatedCategory) != null)
            {

                ModelState.AddModelError("", "Name is a unique attr.Name entered already exist!");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _productCategoryRepository.UpdateCategory(updatedCategory))
            {
                ModelState.AddModelError("", "Process interrupted!Couldn't add category");
                return StatusCode(400, ModelState);
            }

            return Ok("Category Successfully updated!");
        }

        [HttpDelete("/Delete_Category/{categoryID}")]
        [ProducesResponseType(200, Type = typeof(List<ProductCategory>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteCategoryByID(int categoryID)
        {
            if (!await _productCategoryRepository.CategoryExists(categoryID))
            {
                ModelState.AddModelError("", "No Category with such id!");
                return StatusCode(400, ModelState);
            }

            if(await _productCategoryRepository.CategoryHasSubCategories(categoryID))
            {
                ModelState.AddModelError("", "Delete All subcategories related to this category first!");
                return StatusCode(400, ModelState);
            }

            var categoryToDelete = await _productCategoryRepository.getCategoryByID(categoryID);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _productCategoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Delete Category Process Cancelled!");
                return StatusCode(400, ModelState);
            }

            return Ok("Category Deleted Successfully");
        }

    }
}