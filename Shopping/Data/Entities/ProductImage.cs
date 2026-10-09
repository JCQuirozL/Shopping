using System.ComponentModel.DataAnnotations;
using Shopping.Helpers;

namespace Shopping.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        public string Extension { get; set; } = ".jpg";

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? ProductImageKeywordHelper.GetImageUrl(Product?.Name, Product?.Id ?? 0)
            : $"/images/products/{ImageId}{Extension}";

    }
}