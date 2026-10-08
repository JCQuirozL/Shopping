using System.Globalization;

namespace Shopping.Helpers
{
    public class Translator : ITranslator
    {
        private static readonly Dictionary<string, Dictionary<string, string>> Resources = new()
        {
            ["NavHome"] = new() { ["es"] = "Inicio", ["en"] = "Home", ["pt"] = "Início" },
            ["NavOffers"] = new() { ["es"] = "Ofertas", ["en"] = "Deals", ["pt"] = "Ofertas" },
            ["NavTrends"] = new() { ["es"] = "Tendencias", ["en"] = "Trending", ["pt"] = "Tendências" },
            ["NavHelp"] = new() { ["es"] = "Ayuda", ["en"] = "Help", ["pt"] = "Ajuda" },
            ["NavCategoriesMenu"] = new() { ["es"] = "Categorías", ["en"] = "Categories", ["pt"] = "Categorias" },
            ["NavDashboard"] = new() { ["es"] = "Dashboard", ["en"] = "Dashboard", ["pt"] = "Painel" },
            ["NavUsers"] = new() { ["es"] = "Usuarios", ["en"] = "Users", ["pt"] = "Usuários" },
            ["NavGeography"] = new() { ["es"] = "Geografía", ["en"] = "Geography", ["pt"] = "Geografia" },
            ["NavCategories"] = new() { ["es"] = "Categorías", ["en"] = "Categories", ["pt"] = "Categorias" },
            ["NavProducts"] = new() { ["es"] = "Productos", ["en"] = "Products", ["pt"] = "Produtos" },
            ["NavInventory"] = new() { ["es"] = "Inventario", ["en"] = "Inventory", ["pt"] = "Estoque" },
            ["NavCustomers"] = new() { ["es"] = "Clientes", ["en"] = "Customers", ["pt"] = "Clientes" },
            ["NavOrders"] = new() { ["es"] = "Pedidos", ["en"] = "Orders", ["pt"] = "Pedidos" },
            ["NavPrivacy"] = new() { ["es"] = "Políticas", ["en"] = "Policies", ["pt"] = "Políticas" },
            ["NavMyOrders"] = new() { ["es"] = "Mis Pedidos", ["en"] = "My Orders", ["pt"] = "Meus Pedidos" },
            ["NavMyAddresses"] = new() { ["es"] = "Mis Direcciones", ["en"] = "My Addresses", ["pt"] = "Meus Endereços" },
            ["MyAccount"] = new() { ["es"] = "Mi Cuenta", ["en"] = "My Account", ["pt"] = "Minha Conta" },
            ["Logout"] = new() { ["es"] = "Cerrar Sesión", ["en"] = "Log Out", ["pt"] = "Sair" },
            ["Login"] = new() { ["es"] = "Iniciar Sesión", ["en"] = "Log In", ["pt"] = "Entrar" },
            ["SearchPlaceholder"] = new() { ["es"] = "Buscar productos, marcas y más...", ["en"] = "Search products, brands and more...", ["pt"] = "Buscar produtos, marcas e mais..." },
            ["Region"] = new() { ["es"] = "País / Región", ["en"] = "Country / Region", ["pt"] = "País / Região" },
            ["Footer"] = new() { ["es"] = "Shopping - Todos los derechos reservados", ["en"] = "Shopping - All rights reserved", ["pt"] = "Shopping - Todos os direitos reservados" },
            ["DashboardTitle"] = new() { ["es"] = "Dashboard Administrativo", ["en"] = "Admin Dashboard", ["pt"] = "Painel Administrativo" },
            ["TotalOrders"] = new() { ["es"] = "Pedidos", ["en"] = "Orders", ["pt"] = "Pedidos" },
            ["TotalRevenue"] = new() { ["es"] = "Ingresos", ["en"] = "Revenue", ["pt"] = "Receita" },
            ["TotalCustomers"] = new() { ["es"] = "Clientes", ["en"] = "Customers", ["pt"] = "Clientes" },
            ["TotalProducts"] = new() { ["es"] = "Productos", ["en"] = "Products", ["pt"] = "Produtos" },
            ["LowStock"] = new() { ["es"] = "Bajo Stock", ["en"] = "Low Stock", ["pt"] = "Estoque Baixo" },
            ["SalesLast30Days"] = new() { ["es"] = "Ventas (últimos 30 días)", ["en"] = "Sales (last 30 days)", ["pt"] = "Vendas (últimos 30 dias)" },
            ["OrdersByStatus"] = new() { ["es"] = "Pedidos por Estado", ["en"] = "Orders by Status", ["pt"] = "Pedidos por Status" },
            ["TopProducts"] = new() { ["es"] = "Productos más Vendidos", ["en"] = "Top Selling Products", ["pt"] = "Produtos Mais Vendidos" },
            ["RecentOrders"] = new() { ["es"] = "Pedidos Recientes", ["en"] = "Recent Orders", ["pt"] = "Pedidos Recentes" },
            ["LowStockProducts"] = new() { ["es"] = "Productos con Bajo Stock", ["en"] = "Low Stock Products", ["pt"] = "Produtos com Estoque Baixo" },
            ["Status_Pendiente"] = new() { ["es"] = "Pendiente", ["en"] = "Pending", ["pt"] = "Pendente" },
            ["Status_Confirmado"] = new() { ["es"] = "Confirmado", ["en"] = "Confirmed", ["pt"] = "Confirmado" },
            ["Status_EnPreparacion"] = new() { ["es"] = "En Preparación", ["en"] = "Preparing", ["pt"] = "Em Preparação" },
            ["Status_Despachado"] = new() { ["es"] = "Despachado", ["en"] = "Dispatched", ["pt"] = "Despachado" },
            ["Status_EnCamino"] = new() { ["es"] = "En Camino", ["en"] = "In Transit", ["pt"] = "A Caminho" },
            ["Status_Entregado"] = new() { ["es"] = "Entregado", ["en"] = "Delivered", ["pt"] = "Entregue" },
            ["Status_Cancelado"] = new() { ["es"] = "Cancelado", ["en"] = "Cancelled", ["pt"] = "Cancelado" },
            ["LoginWithGoogle"] = new() { ["es"] = "Continuar con Google", ["en"] = "Continue with Google", ["pt"] = "Continuar com Google" },
            ["LoginWithFacebook"] = new() { ["es"] = "Continuar con Facebook", ["en"] = "Continue with Facebook", ["pt"] = "Continuar com Facebook" },
            ["OrDivider"] = new() { ["es"] = "o", ["en"] = "or", ["pt"] = "ou" },
        };

        public string this[string key]
        {
            get
            {
                string culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if (Resources.TryGetValue(key, out Dictionary<string, string> translations))
                {
                    if (translations.TryGetValue(culture, out string value))
                    {
                        return value;
                    }

                    if (translations.TryGetValue("es", out string fallback))
                    {
                        return fallback;
                    }
                }

                return key;
            }
        }
    }
}
