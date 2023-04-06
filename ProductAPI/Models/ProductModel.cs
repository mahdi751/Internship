using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;

namespace ProductAPI.Models
{
    [Table("ProductModel", Schema = "Production")]
    public class ProductModel
    {
        public int ProductModelId { get; set; }
        public string Name { get; set; }
        public System.Security.Cryptography.ECKeyXmlFormat? CatalogDescription { get; set; }
        public System.Security.Cryptography.ECKeyXmlFormat? Instructions { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
