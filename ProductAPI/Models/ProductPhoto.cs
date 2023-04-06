using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Models
{
    [Table("ProductPhoto", Schema = "Production")]
    public class ProductPhoto
    {
        public int ProductPhotoID { get; set; }
        public byte[]? ThumbnailPhoto { get; set; }
        public string? ThumbnailPhotoFileName { get; set; }
        public byte[]? LargePhoto { get; set; }
        public string? LargePhotoFileName { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
