using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Models
{
    [Table("ProductInventory", Schema = "Production")]
    public class ProductInventory
    {
        //FK PK
        public int ProductID { get; set; }
        public int LocationID { get; set; }
        //end FK Pk
        public string Shelf { get; set; }
        public int Bin { get; set; }
        public int Quantity { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
