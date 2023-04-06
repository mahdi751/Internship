using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Models
{
    [Table("UnitMeasure", Schema = "Production")]
    public class UnitMeasure
    {
        public string UnitMeasureCode { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
