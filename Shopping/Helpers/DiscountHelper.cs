using Shopping.Data.Entities;

namespace Shopping.Helpers
{
    /// <summary>
    /// Deterministically simulates promotional discounts for a subset of products,
    /// without requiring any database schema changes.
    /// </summary>
    public static class DiscountHelper
    {
        private static readonly int[] DiscountPercentages = { 15, 20, 25, 30 };

        public static bool HasDiscount(int productId)
        {
            return productId % 3 == 0;
        }

        public static int GetDiscountPercentage(int productId)
        {
            return DiscountPercentages[productId % DiscountPercentages.Length];
        }

        public static decimal GetDiscountedPrice(Product product)
        {
            if (product == null || !HasDiscount(product.Id))
            {
                return product?.Price ?? 0;
            }

            decimal discount = GetDiscountPercentage(product.Id) / 100m;
            return Math.Round(product.Price * (1 - discount), 2);
        }
    }
}
