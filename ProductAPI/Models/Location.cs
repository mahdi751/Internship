using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Models
{
    [Table("Location", Schema = "Production")]
    public class Location
    {
        public int LocationID { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal CostRate { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Availability { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
