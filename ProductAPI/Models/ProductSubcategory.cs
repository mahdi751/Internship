using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Models
{
    [Table("ProductSubcategory", Schema = "Production")]
    public class ProductSubcategory
    {
        public int ProductSubcategoryID { get; set; }
        public int ProductCategoryID { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string Name { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
