using Shopping.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class DashboardViewModel
    {
        [Display(Name = "Pedidos")]
        public int TotalOrders { get; set; }

        [Display(Name = "Ingresos totales")]
        public decimal TotalRevenue { get; set; }

        [Display(Name = "Clientes")]
        public int TotalCustomers { get; set; }

        [Display(Name = "Productos")]
        public int TotalProducts { get; set; }

        [Display(Name = "Productos con bajo stock")]
        public int LowStockCount { get; set; }

        public List<string> SalesDays { get; set; } = new();
        public List<decimal> SalesAmounts { get; set; } = new();

        public List<string> OrderStatusLabels { get; set; } = new();
        public List<int> OrderStatusCounts { get; set; } = new();

        public List<string> TopProductNames { get; set; } = new();
        public List<float> TopProductQuantities { get; set; } = new();

        public List<Product> LowStockProducts { get; set; } = new();
        public List<Sale> RecentOrders { get; set; } = new();
    }
}
