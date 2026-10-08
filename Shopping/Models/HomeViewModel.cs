using Shopping.Data.Entities;

namespace Shopping.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public string Search { get; set; }

        public int? CategoryId { get; set; }

        public float Quantity { get; set; }

    }
}
