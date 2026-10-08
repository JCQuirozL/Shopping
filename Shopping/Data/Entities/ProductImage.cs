using System.ComponentModel.DataAnnotations;

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
            ? $"https://placehold.co/500x500/1f2a44/f7f5f2?font=poppins&text={Uri.EscapeDataString(Product?.Name ?? "Shopping")}"
            : $"/images/products/{ImageId}{Extension}";

    }
}