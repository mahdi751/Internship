using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Models
{
    [Table("ProductProductPhoto", Schema = "Production")]
    public class ProductProductPhoto
    {
        public int ProductID { get; set; }
        public int ProductPhotoID { get; set; }
        public bool Primary { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
