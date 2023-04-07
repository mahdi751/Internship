using Microsoft.AspNetCore.Mvc;
using ProductAPI.Interfaces;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }

        [HttpGet ("/Get_All_Products")]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllProducts()
        {
            var products =await _productRepository.GetAllProducts();
            if (products == null)
            {
                ModelState.AddModelError("", "There is no products!");
                return StatusCode(400, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        [HttpDelete("/Delete_Product/{ProductID}")]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteProductByID(int ProductID)
        {
            if (!await _productRepository.ProductExists(ProductID))
            {
                ModelState.AddModelError("", "No Product with such id!");
                return StatusCode(400, ModelState);
            }

            if (await _productRepository.ProductHasInventory(ProductID))
            {
                ModelState.AddModelError("", "Delete All inventories related to this product first!");
                return StatusCode(400, ModelState);
            }

            if (await _productRepository.ProductHasPhotos(ProductID))
            {
                ModelState.AddModelError("", "Delete All product photos related to this product first!");
                return StatusCode(400, ModelState);
            }

            var productToDelete =await _productRepository.GetProductByID(ProductID);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _productRepository.DeleteProduct(productToDelete))
            {
                ModelState.AddModelError("", "Delete Product Process Cancelled!");
                return StatusCode(400, ModelState);
            }

            return Ok("Product Deleted Successfully");
        }

        [HttpPost("/Create_Product")]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateProduct([FromBody] Product newProduct)
        {

            if (newProduct == null)
                return BadRequest(ModelState);



            var productid = newProduct.ProductID;
            var productSubID = newProduct.ProductSubcategoryID;
            var productModelID = newProduct.ProductModelID;
            var sizeUnitMeasure = newProduct.SizeUnitMeasureCode;
            var weightUnitMeasure = newProduct.WeightUnitMeasureCode;

            var productClass = newProduct.Class;
            var productStyle = newProduct.Style;
            var productSafetyStockLvl = newProduct.SafetyStockLevel;
            var productLine = newProduct.ProductLine;
            var reorderPoint = newProduct.ReorderPoint;
            var weight = newProduct.Weight;
            var size = newProduct.Size;
            var productNumber = newProduct.ProductNumber;
            var productName = newProduct.Name;
            var productRowguid = newProduct.Rowguid;
            var daysToManifacture = newProduct.DaysToManufacture;
            var listPrice = newProduct.ListPrice;
            var sellenddate = newProduct.SellEndDate;
            var sellstartdate = newProduct.SellStartDate;
            var standardCost = newProduct.StandardCost;
            var productColor = newProduct.Color;

            if (await _productRepository.ProductExists(productid))
            {

                ModelState.AddModelError("", "Product already exist!");
                return StatusCode(400, ModelState);
            }

            if (productSubID != null && !await _productRepository.SubCategoryExist(productSubID))
            {

                ModelState.AddModelError("", "SubcategoryID does not exist!");
                return StatusCode(400, ModelState);
            }

            if (productModelID != null && !await _productRepository.ModelExist(productModelID))
            {

                ModelState.AddModelError("", "ProductModelID does not exist!");
                return StatusCode(400, ModelState);
            }

            if (sizeUnitMeasure != null)
            {
                bool found = false;
                string[] sizeValues = { "BOX", "BTL", "C", "CAN", "CAR", "CBM", "CCM", "CDM", "CM", "CM2", "CR", "CS", "CTN", "EA", "FT3", "GAL", "IN", "KM", "KT", "L", "M", "M2", "M3", "MG", "ML", "MM", "PAK", "PAL", "PC", "PCT", "PT" };

                foreach (string sizevalue in sizeValues)
                {
                    if (sizeUnitMeasure == sizevalue)
                    {
                        found = true;
                        break;
                    }
                }
                if (sizeUnitMeasure.Length > 3 || found == false)
                {
                    ModelState.AddModelError("", "Size must be 'BOX' OR 'BTL' OR 'C' OR 'CAN' OR 'CAR'\n" +
                    "'OR CBM' OR 'CCM' OR 'CDM' OR 'CM' OR 'CM2' OR 'CR' OR 'CS' OR 'CTN' OR 'EA' OR 'FT3' OR 'GAL'\n" +
                    "OR 'IN' OR 'KM' OR 'KT' OR 'L' OR 'M' OR 'M2' OR 'M3' OR 'MG' OR 'ML' OR 'MM' OR 'PAK' OR 'PAL'\n" +
                    "OR 'PC' OR 'PCT' OR 'PT'!");
                    return StatusCode(400, ModelState);
                }
            }

            if (weightUnitMeasure != null)
            {
                bool found = false;
                string[] weightValues = { "KG", "KGV", "LB", "DM", "DZ", "G", "MG", "OZ" };
                foreach (string weightValue in weightValues)
                {
                    if (weightUnitMeasure == weightValue)
                    {
                        found = true;
                        break;
                    }
                }
                if (weightUnitMeasure.Length > 3 || found == false)
                {
                    ModelState.AddModelError("", "Weight must be 'KG' OR 'KGV' OR 'LB' OR 'DM' OR 'DZ' OR 'G' OR 'MG' OR 'OZ'!");
                    return StatusCode(400, ModelState);
                }
            }

            if (productClass != null && (productClass != "H" && productClass != "M" && productClass != "L" || productClass.Length>2)) 
            {

                ModelState.AddModelError("", "Product Class should be either 'H' OR 'M' OR 'L' OR NULL)!");
                return StatusCode(400, ModelState);
            }

            if (productStyle != null && (productStyle != "U" && productStyle != "M" && productStyle != "W" || productStyle.Length > 2 ))
            {
                ModelState.AddModelError("", "Product Style should be either 'U' OR 'M' OR 'W' OR NULL)!");
                return StatusCode(400, ModelState);
            }

            if(daysToManifacture < 0 )
            {
                ModelState.AddModelError("", "DayToManifacture must be >= 0!");
                return StatusCode(400, ModelState);
            }

            if(listPrice < 0)
            {
                ModelState.AddModelError("", "listPrice must be >= 0!");
                return StatusCode(400, ModelState);
            }

            if (productLine != null && (productLine != "R" && productLine != "M" && productLine != "T" && productLine != "S" || productLine.Length > 2))
            {
                ModelState.AddModelError("", "ProductLine should be either 'R' OR 'M' OR 'T' OR 'S' OR NULL)!");
                return StatusCode(400, ModelState);
            }

            if (reorderPoint <= 0)
            {
                ModelState.AddModelError("", "reorderPoint must be > 0!");
                return StatusCode(400, ModelState);
            }

            if(productSafetyStockLvl <= 0) 
            {
                ModelState.AddModelError("", "product Safety Stock LeveL must be > 0!");
                return StatusCode(400, ModelState);
            }

            if(sellenddate != null &&  (sellenddate < sellstartdate))
            {
                ModelState.AddModelError("", "sellEnd date must be >= sellStart date!");
                return StatusCode(400, ModelState);
            }

            if (standardCost < 0)
            {
                ModelState.AddModelError("", "Product Standard Cost must be >= 0!");
                return StatusCode(400, ModelState);
            }

            if(weight <= 0)
            {
                ModelState.AddModelError("", "weight must be > 0!");
                return StatusCode(400, ModelState);
            }

            if(productName.Length > 50)
            {
                ModelState.AddModelError("", "name must be <= 50 characteres!");
                return StatusCode(400, ModelState);
            }
            else
            {
                if(await _productRepository.GetProductByProductName(newProduct) != null)
                {
                    ModelState.AddModelError("", "Name is a unique attr.Name entered already exist!");
                    return StatusCode(400, ModelState);
                }
            }

            if(productNumber.Length >25)
            {
                ModelState.AddModelError("", "productNumber must be <= 25 characteres!");
                return StatusCode(400, ModelState);
            }
            else
            {
                if (await _productRepository.GetProductByProductNumber(newProduct) != null)
                {
                    
                    ModelState.AddModelError("", "ProductNumber is a unique attr.ProductNumber entered already exist!");
                    return StatusCode(400, ModelState);
                }
            }


            if(productColor != null && productColor.Length > 15)
            {
                ModelState.AddModelError("", "product Color must be <= 15 characteres!");
                return StatusCode(400, ModelState);
            }

            if (await _productRepository.GetProductByProductRowguid(newProduct) != null)
            {

                ModelState.AddModelError("", "productRowGuid is a unique attr.ProductNumber entered already exist!");
                return StatusCode(400, ModelState);
            }

            if(size != null && size.Length > 5)
            {
                ModelState.AddModelError("", "Product Size must be <= 5 characteres!");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!await _productRepository.CreateProduct(newProduct))
            {
                ModelState.AddModelError("", "Process interrupted!Couldn't add product");
                return StatusCode(400, ModelState);
            }

            return Ok("Product Successfully Created");
        }

        [HttpPut("/Update_Product/{productID}")]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProduct(int productID, [FromBody]  Product updatedProduct)
        {
            var updatedProductID = updatedProduct.ProductID;
            var productSubID = updatedProduct.ProductSubcategoryID;
            var productModelID = updatedProduct.ProductModelID;
            var sizeUnitMeasure = updatedProduct.SizeUnitMeasureCode;
            var weightUnitMeasure = updatedProduct.WeightUnitMeasureCode;

            var productClass = updatedProduct.Class;
            var productStyle = updatedProduct.Style;
            var productSafetyStockLvl = updatedProduct.SafetyStockLevel;
            var productLine = updatedProduct.ProductLine;
            var reorderPoint = updatedProduct.ReorderPoint;
            var weight = updatedProduct.Weight;
            var size = updatedProduct.Size;
            var productNumber = updatedProduct.ProductNumber;
            var productName = updatedProduct.Name;
            var productRowguid = updatedProduct.Rowguid;
            var daysToManifacture = updatedProduct.DaysToManufacture;
            var listPrice = updatedProduct.ListPrice;
            var sellenddate = updatedProduct.SellEndDate;
            var sellstartdate = updatedProduct.SellStartDate;
            var standardCost = updatedProduct.StandardCost;
            var productColor = updatedProduct.Color;

            if (updatedProduct == null)
                return BadRequest(ModelState);

            if(productID != updatedProductID)
            {
                ModelState.AddModelError("", "Productid not compatible!");
                return StatusCode(400, ModelState);
            }

            if(!await _productRepository.ProductExists(productID))
            {
                ModelState.AddModelError("", "No Product with such id!");
                return StatusCode(400, ModelState);
            }

            if (productSubID != null && !await _productRepository.SubCategoryExist(productSubID))
            {
                ModelState.AddModelError("", "SubcategoryID does not exist!");
                return StatusCode(400, ModelState);
            }

            if (productModelID != null && !await _productRepository.ModelExist(productModelID))
            {
                ModelState.AddModelError("", "ProductModelID does not exist!");
                return StatusCode(400, ModelState);
            }

            if (sizeUnitMeasure != null)
            {
                if (sizeUnitMeasure.Length > 3)
                {
                    ModelState.AddModelError("", "Size must be 'BOX' OR 'BTL' OR 'C' OR 'CAN' OR 'CAR'\n" +
                    "'OR CBM' OR 'CCM' OR 'CDM' OR 'CM' OR 'CM2' OR 'CR' OR 'CS' OR 'CTN' OR 'EA' OR 'FT3' OR 'GAL'\n" +
                    "OR 'IN' OR 'KM' OR 'KT' OR 'L' OR 'M' OR 'M2' OR 'M3' OR 'MG' OR 'ML' OR 'MM' OR 'PAK' OR 'PAL'\n" +
                    "OR 'PC' OR 'PCT' OR 'PT'!");
                    return StatusCode(400, ModelState);
                }
                bool found = false;
                string[] sizeValues = { "BOX", "BTL", "C", "CAN", "CAR", "CBM", "CCM", "CDM", "CM", "CM2", "CR", "CS", "CTN", "EA", "FT3", "GAL", "IN", "KM", "KT", "L", "M", "M2", "M3", "MG", "ML", "MM", "PAK", "PAL", "PC", "PCT", "PT" };

                foreach (string sizevalue in sizeValues)
                {
                    if (sizeUnitMeasure == sizevalue)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    ModelState.AddModelError("", "Size must be 'BOX' OR 'BTL' OR 'C' OR 'CAN' OR 'CAR'\n" +
                    "'OR CBM' OR 'CCM' OR 'CDM' OR 'CM' OR 'CM2' OR 'CR' OR 'CS' OR 'CTN' OR 'EA' OR 'FT3' OR 'GAL'\n" +
                    "OR 'IN' OR 'KM' OR 'KT' OR 'L' OR 'M' OR 'M2' OR 'M3' OR 'MG' OR 'ML' OR 'MM' OR 'PAK' OR 'PAL'\n" +
                    "OR 'PC' OR 'PCT' OR 'PT'!");
                    return StatusCode(400, ModelState);
                }
            }

            if (weightUnitMeasure != null)
            {
                if (weightUnitMeasure.Length > 3)
                {
                    ModelState.AddModelError("", "Weight must be 'KG' OR 'KGV' OR 'LB' OR 'DM' OR 'DZ' OR 'G' OR 'MG' OR 'OZ'!");
                    return StatusCode(400, ModelState);
                }
                bool found = false;
                string[] weightValues = { "KG", "KGV", "LB", "DM", "DZ", "G", "MG", "OZ" };
                foreach (string weightValue in weightValues)
                {
                    if (weightUnitMeasure == weightValue)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    ModelState.AddModelError("", "Weight must be 'KG' OR 'KGV' OR 'LB' OR 'DM' OR 'DZ' OR 'G' OR 'MG' OR 'OZ'!");
                    return StatusCode(400, ModelState);
                }
            }

            if (productClass != null && (productClass != "H" && productClass != "M" && productClass != "L") && productClass.Length > 2)
            {
                ModelState.AddModelError("", "Product Class should be either 'H' OR 'M' OR 'L' OR NULL)!");
                return StatusCode(400, ModelState);
            }

            if (productStyle != null && (productStyle != "U" && productStyle != "M" && productStyle != "W") && productStyle.Length > 2)
            {
                ModelState.AddModelError("", "Product Style should be either 'U' OR 'M' OR 'W' OR NULL)!");
                return StatusCode(400, ModelState);
            }

            if (daysToManifacture < 0)
            {
                ModelState.AddModelError("", "DayToManifacture must be >= 0!");
                return StatusCode(400, ModelState);
            }

            if (listPrice < 0)
            {
                ModelState.AddModelError("", "listPrice must be >= 0!");
                return StatusCode(400, ModelState);
            }

            if (productLine != null && (productLine != "R" && productLine != "M" && productLine != "T" && productLine != "S") && productLine.Length > 2)
            {
                ModelState.AddModelError("", "ProductLine should be either 'R' OR 'M' OR 'T' OR 'S' OR NULL)!");
                return StatusCode(400, ModelState);
            }

            if (reorderPoint <= 0)
            {
                ModelState.AddModelError("", "reorderPoint must be > 0!");
                return StatusCode(400, ModelState);
            }

            if (productSafetyStockLvl <= 0)
            {
                ModelState.AddModelError("", "product Safety Stock LeveL must be > 0!");
                return StatusCode(400, ModelState);
            }

            if (sellenddate != null && (sellenddate < sellstartdate))
            {
                ModelState.AddModelError("", "sellEnd date must be >= sellStart date!");
                return StatusCode(400, ModelState);
            }

            if (standardCost < 0)
            {
                ModelState.AddModelError("", "Product Standard Cost must be >= 0!");
                return StatusCode(400, ModelState);
            }

            if (weight <= 0)
            {
                ModelState.AddModelError("", "weight must be > 0!");
                return StatusCode(400, ModelState);
            }

            if (productName.Length > 50)
            {
                ModelState.AddModelError("", "name must be <= 50 characteres!");
                return StatusCode(400, ModelState);
            }
            else
            {
                if (await _productRepository.GetProductByProductName(updatedProduct) != null)
                {
                    ModelState.AddModelError("", "Name is a unique attr.Name entered already exist!");
                    return StatusCode(400, ModelState);
                }
            }

            if (productNumber.Length > 25)
            {
                ModelState.AddModelError("", "productNumber must be <= 25 characteres!");
                return StatusCode(400, ModelState);
            }
            else
            {
                if (await _productRepository.GetProductByProductNumber(updatedProduct) != null)
                {
                    ModelState.AddModelError("", "ProductNumber is a unique attr.ProductNumber entered already exist!");
                    return StatusCode(400, ModelState);
                }
            }

            if (productColor != null && productColor.Length > 15)
            {
                ModelState.AddModelError("", "product Color must be <= 15 characteres!");
                return StatusCode(400, ModelState);
            }

            if (await _productRepository.GetProductByProductRowguid(updatedProduct) != null)
            {
                ModelState.AddModelError("", "productRowGuid is a unique attr.ProductNumber entered already exist!");
                return StatusCode(400, ModelState);
            }

            if (size != null && size.Length > 5)
            {
                ModelState.AddModelError("", "Product Size must be <= 5 characteres!");
                return StatusCode(400, ModelState);
            }

            if (!await _productRepository.UpdateProduct(updatedProduct))
            {
                ModelState.AddModelError("", "Something went wrong when updating the product!");
                return StatusCode(400, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Product "+productID+" updated successfully!");

        }
    }
}
