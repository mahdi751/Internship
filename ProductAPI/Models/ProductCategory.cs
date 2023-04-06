using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Models
{
    [Table("ProductCategory", Schema = "Production")]
    public class ProductCategory
    {
        public int ProductCategoryID { get; set; }
        public string Name { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
