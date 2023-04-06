using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProductAPI.Data;
using ProductAPI.Interfaces;
using ProductAPI.Models;

namespace ProductAPI.Repository;
public class ProductRepository : IProductRepository
{
    private readonly DataContext _context;
    public ProductRepository(DataContext context)
    {
         _context = context;
    }
    
    public ICollection<Product> GetAllProducts() => _context.Products.OrderBy(p => p.ProductID).ToList();

    public ICollection<Product> GetAllProductsWithout(int productid)
    {
        return   _context.Products.Where(p => p.ProductID != productid).ToList();
    }

    public Product getProductByID(int productID)
    {
        return _context.Products.Where(p => p.ProductID == productID).FirstOrDefault();
    }

    public bool CreateProduct(Product product)
    {
        /*var subcategoryEntity = _context.productSubcategories.Where(p => p.ProductSubcategoryID == subcategoryID).FirstOrDefault();
        var modelEntity = _context.ProductModels.Where(m => m.ProductModelId == modelID).FirstOrDefault();

        var subcategory = new ProductSubcategory()
        {
            ProductSubcategory = subcategoryEntity,
            Product = product,
        };*/
        _context.Add(product);
        return Save();
    }
    public bool DeleteProduct(Product product)
    {
        _context.Remove(product);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }

    public bool ProductExists(int productID)
    {
        return _context.Products.Any(p => p.ProductID == productID);
    }

    public bool SubCategoryExist(int? productSubcategoryID)
    {
        return _context.productSubcategories.Any(p => p.ProductSubcategoryID == productSubcategoryID);
    }

    public bool ModelExist(int? modelID)
    {
        return _context.ProductModels.Any(p => p.ProductModelId == modelID);
    }

    public bool UpdateProduct(Product product)
    {
        _context.Update(product);
        return Save();
    }
}

