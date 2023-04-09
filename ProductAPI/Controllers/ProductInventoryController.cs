using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Interfaces;
using ProductAPI.Models;
using ProductAPI.Repository;
using System;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductInventoryController : ControllerBase
    {
        private readonly IProductInventoryRepository _productInventoryRepository;

        public ProductInventoryController(IProductInventoryRepository productInventoryRepository)
        {
            _productInventoryRepository = productInventoryRepository;
        }

        [HttpPost("/Create_Inventory_Product")]
        [ProducesResponseType(200, Type = typeof(List<ProductInventory>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateProductInventory([FromBody] ProductInventory newProductInventory)
        {
            if (newProductInventory == null)
                return BadRequest(ModelState);

            var productid = newProductInventory.ProductID;
            var locationid = newProductInventory.LocationID;
            var bin = newProductInventory.Bin;
            var shelf = newProductInventory.Shelf;

            if (!await _productInventoryRepository.ProductExists(productid))
            {
                ModelState.AddModelError("", "ProductID entered does not exist!");
                return StatusCode(400, ModelState);
            }

            if (!await _productInventoryRepository.LocationExists(locationid))
            {
                ModelState.AddModelError("", "LocationID entered does not exist!");
                return StatusCode(400, ModelState);
            }

            if (bin < 0 || bin > 100)
            {
                ModelState.AddModelError("", "bin must be >=0 AND <=100!");
                return StatusCode(400, ModelState);
            }
            
            if (shelf.Length > 1 && shelf != "N/A")
            {
                ModelState.AddModelError("", "shelf must be a char from A to Z or 'N/A'!");
                return StatusCode(400, ModelState);
            }
            else{
                bool valid;
                char []str = shelf.ToCharArray();
                
                valid = Char.IsLetter(str[0]);
                if(!valid)
                {
                    ModelState.AddModelError("", "shelf must be a char from A to Z or 'N/A'!");
                    return StatusCode(400, ModelState);
                }
            }
            newProductInventory.Shelf=newProductInventory.Shelf.ToUpper();

            var productInventory = await _productInventoryRepository.GetProductInventoryByProductID_LocationID(productid, locationid);
            if (productInventory != null)
            {
                productInventory.Quantity += newProductInventory.Quantity;

                productInventory.ModifiedDate= DateTime.Now;

                if (!await _productInventoryRepository.UpdateProductInventory(productInventory))
                {
                    ModelState.AddModelError("", "Something went wrong when updating the productInventory!");
                    return StatusCode(400, ModelState);
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok("You entered a ProductID and LocationID that already exits.Product Inventory Quantity has been incremented successfully!");

            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            newProductInventory.ModifiedDate = DateTime.Now;

            if (!await _productInventoryRepository.CreateProductInventory(newProductInventory))
            {
                ModelState.AddModelError("", "Process interrupted!Couldn't add productInventory");
                return StatusCode(400, ModelState);
            }

            return Ok("New Product Invetory has been created!");
        }

        [HttpPut("/Update_ProductInventory/{productID}/{locationID}")]
        [ProducesResponseType(200, Type = typeof(List<ProductInventory>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProductInventory(int productID,int locationID, [FromBody] ProductInventory updatedProductInventory)
        {
            var bin = updatedProductInventory.Bin;
            var shelf = updatedProductInventory.Shelf;    

            if (updatedProductInventory == null)
                return BadRequest(ModelState);

            if (productID != updatedProductInventory.ProductID)
            {
                ModelState.AddModelError("", "Productids not compatible!");
                return StatusCode(400, ModelState);
            }

            if (locationID != updatedProductInventory.LocationID)
            {
                ModelState.AddModelError("", "LocationIDs not compatible!");
                return StatusCode(400, ModelState);
            }

            if (await _productInventoryRepository.GetProductInventoryByProductID_LocationID(productID, locationID) == null)
            {
                ModelState.AddModelError("", "No ProductInventory with such Productid and LocationID!");
                return StatusCode(400, ModelState);
            }

            if (bin < 0 || bin > 100)
            {
                ModelState.AddModelError("", "bin must be >=0 AND <=100!");
                return StatusCode(400, ModelState);
            }

            if (shelf.Length > 1 && shelf != "N/A")
            {
                ModelState.AddModelError("", "shelf must be a char from A to Z or 'N/A'!");
                return StatusCode(400, ModelState);
            }
            else
            {
                bool valid;
                char[] str = shelf.ToCharArray();

                valid = Char.IsLetter(str[0]);
                if (!valid)
                {
                    ModelState.AddModelError("", "shelf must be a char from A to Z or 'N/A'!");
                    return StatusCode(400, ModelState);
                }
            }
            updatedProductInventory.Shelf = updatedProductInventory.Shelf.ToUpper();
            if (!await _productInventoryRepository.UpdateProductInventory(updatedProductInventory))
            {
                ModelState.AddModelError("", "Something went wrong when updating the productInventory!");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Product Inventory has been updated successfully!");
        }

        [HttpGet("/Get_All_Products_By_Shelf/{shelf}")]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllProductsByShelf(string shelf)
        {
            if (!await _productInventoryRepository.ShelfExist(shelf))
            {
                ModelState.AddModelError("", "Shelf not found!");
                return StatusCode(400, ModelState);
            }

            var products = await _productInventoryRepository.GetProductsByShelf(shelf);
            if(products == null)
            {
                ModelState.AddModelError("", "There is no products!");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        [HttpGet("/Get_All_Products_quantity")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllProductsQuantity()
        {

            var productsQuantityList = await _productInventoryRepository.GetProductsQuantity();

            if(productsQuantityList == null)
            {
                ModelState.AddModelError("", "There is no products!");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(productsQuantityList);
        }

    }
}
