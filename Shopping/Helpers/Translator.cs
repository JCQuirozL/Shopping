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
            ["SalesLast30Days"] = new() { ["es"] = "Ingresos en MXN (últimos 30 días)", ["en"] = "Revenue in MXN (last 30 days)", ["pt"] = "Receita em MXN (últimos 30 dias)" },
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

            // Storefront: catalog
            ["CategoryAll"] = new() { ["es"] = "Todas", ["en"] = "All", ["pt"] = "Todas" },
            ["CartLabel"] = new() { ["es"] = "Carro", ["en"] = "Cart", ["pt"] = "Carrinho" },
            ["NoProductsFound"] = new() { ["es"] = "No encontramos productos", ["en"] = "We couldn't find any products", ["pt"] = "Não encontramos produtos" },
            ["TryAnotherSearch"] = new() { ["es"] = "Prueba con otra búsqueda o quita los filtros aplicados.", ["en"] = "Try another search or remove the applied filters.", ["pt"] = "Tente outra busca ou remova os filtros aplicados." },
            ["ViewAllProducts"] = new() { ["es"] = "Ver todos los productos", ["en"] = "View all products", ["pt"] = "Ver todos os produtos" },
            ["DetailsButton"] = new() { ["es"] = "Detalles", ["en"] = "Details", ["pt"] = "Detalhes" },
            ["AddButton"] = new() { ["es"] = "Agregar", ["en"] = "Add", ["pt"] = "Adicionar" },

            // Storefront: product detail
            ["AvailableUnits"] = new() { ["es"] = "disponibles", ["en"] = "available", ["pt"] = "disponíveis" },
            ["OptionalPlaceholder"] = new() { ["es"] = "Opcional", ["en"] = "Optional", ["pt"] = "Opcional" },
            ["AddToCartButton"] = new() { ["es"] = "Agregar al Carro de Compras", ["en"] = "Add to Cart", ["pt"] = "Adicionar ao Carrinho" },
            ["BuyNowButton"] = new() { ["es"] = "Comprar Ahora", ["en"] = "Buy Now", ["pt"] = "Comprar Agora" },

            // Storefront: cart
            ["CartTitle"] = new() { ["es"] = "Carro de Compras", ["en"] = "Shopping Cart", ["pt"] = "Carrinho de Compras" },
            ["EmptyCartTitle"] = new() { ["es"] = "Tu carro está vacío", ["en"] = "Your cart is empty", ["pt"] = "Seu carrinho está vazio" },
            ["EmptyCartSubtitle"] = new() { ["es"] = "Agrega productos para continuar con tu compra.", ["en"] = "Add products to continue your purchase.", ["pt"] = "Adicione produtos para continuar sua compra." },
            ["ViewProductsButton"] = new() { ["es"] = "Ver productos", ["en"] = "View products", ["pt"] = "Ver produtos" },
            ["OrderSummary"] = new() { ["es"] = "Resumen del pedido", ["en"] = "Order summary", ["pt"] = "Resumo do pedido" },
            ["SavedAddressLabel"] = new() { ["es"] = "Dirección guardada", ["en"] = "Saved address", ["pt"] = "Endereço salvo" },
            ["WriteManually"] = new() { ["es"] = "-- Escribir manualmente --", ["en"] = "-- Enter manually --", ["pt"] = "-- Digitar manualmente --" },
            ["GoToPayButton"] = new() { ["es"] = "Ir a Pagar", ["en"] = "Go to Payment", ["pt"] = "Ir para Pagamento" },
            ["ContinueShoppingButton"] = new() { ["es"] = "Seguir Comprando", ["en"] = "Continue Shopping", ["pt"] = "Continuar Comprando" },
            ["EditAction"] = new() { ["es"] = "Editar", ["en"] = "Edit", ["pt"] = "Editar" },
            ["DeleteAction"] = new() { ["es"] = "Eliminar", ["en"] = "Delete", ["pt"] = "Excluir" },

            // Storefront: payment
            ["ChoosePaymentTitle"] = new() { ["es"] = "Elige cómo pagar", ["en"] = "Choose how to pay", ["pt"] = "Escolha como pagar" },
            ["PaymentBreadcrumb"] = new() { ["es"] = "Pago", ["en"] = "Payment", ["pt"] = "Pagamento" },
            ["CreditCardOption"] = new() { ["es"] = "Tarjeta de Crédito", ["en"] = "Credit Card", ["pt"] = "Cartão de Crédito" },
            ["DebitCardOption"] = new() { ["es"] = "Tarjeta de Débito", ["en"] = "Debit Card", ["pt"] = "Cartão de Débito" },
            ["CashOnDeliveryOption"] = new() { ["es"] = "Contra Entrega", ["en"] = "Cash on Delivery", ["pt"] = "Pagamento na Entrega" },
            ["CardHolderPlaceholder"] = new() { ["es"] = "Como aparece en la tarjeta", ["en"] = "As it appears on the card", ["pt"] = "Como aparece no cartão" },
            ["CardSimulationNote"] = new() { ["es"] = "Simulación: cualquier número funciona. Termina en \"0000\" para simular un rechazo.", ["en"] = "Simulation: any number works. End with \"0000\" to simulate a decline.", ["pt"] = "Simulação: qualquer número funciona. Termine com \"0000\" para simular uma recusa." },
            ["PaypalRedirectNote"] = new() { ["es"] = "Serás redirigido (simulación) a PayPal para confirmar el pago de forma segura.", ["en"] = "You'll be redirected (simulation) to PayPal to confirm payment securely.", ["pt"] = "Você será redirecionado (simulação) para o PayPal para confirmar o pagamento com segurança." },
            ["CashDeliveryNote"] = new() { ["es"] = "Pagarás en efectivo cuando recibas tu pedido. No se requiere información adicional.", ["en"] = "You'll pay in cash when you receive your order. No additional information required.", ["pt"] = "Você pagará em dinheiro ao receber seu pedido. Nenhuma informação adicional é necessária." },
            ["PayNowButton"] = new() { ["es"] = "Pagar Ahora", ["en"] = "Pay Now", ["pt"] = "Pagar Agora" },
            ["PaymentSummary"] = new() { ["es"] = "Resumen de pago", ["en"] = "Payment summary", ["pt"] = "Resumo do pagamento" },
            ["ItemsLabel"] = new() { ["es"] = "Artículos", ["en"] = "Items", ["pt"] = "Itens" },
            ["TotalToPay"] = new() { ["es"] = "Total a pagar", ["en"] = "Total to pay", ["pt"] = "Total a pagar" },
            ["SimulatedPaymentNote"] = new() { ["es"] = "Pago simulado, no se realiza ningún cargo real.", ["en"] = "Simulated payment, no real charge is made.", ["pt"] = "Pagamento simulado, nenhuma cobrança real é feita." },

            // Storefront: order success
            ["ThankYouTitle"] = new() { ["es"] = "¡Gracias por tu compra!", ["en"] = "Thank you for your purchase!", ["pt"] = "Obrigado pela sua compra!" },
            ["OrderRegisteredNote"] = new() { ["es"] = "Tu pedido fue registrado en nuestro sistema, pronto uno de nuestros asesores se comunicará contigo.", ["en"] = "Your order has been registered in our system, one of our advisors will contact you soon.", ["pt"] = "Seu pedido foi registrado em nosso sistema, em breve um de nossos consultores entrará em contato." },
            ["PaymentMethodLabel"] = new() { ["es"] = "Método de pago", ["en"] = "Payment method", ["pt"] = "Método de pagamento" },
            ["TotalPaidLabel"] = new() { ["es"] = "Total pagado", ["en"] = "Total paid", ["pt"] = "Total pago" },

            // Storefront: addresses & tracking
            ["AddressesTitle"] = new() { ["es"] = "Mis Direcciones", ["en"] = "My Addresses", ["pt"] = "Meus Endereços" },
            ["AddAddressButton"] = new() { ["es"] = "Agregar Dirección", ["en"] = "Add Address", ["pt"] = "Adicionar Endereço" },
            ["NoAddressesNote"] = new() { ["es"] = "Aún no tienes direcciones guardadas. Se usará la dirección de tu perfil al comprar.", ["en"] = "You don't have any saved addresses yet. Your profile address will be used when checking out.", ["pt"] = "Você ainda não tem endereços salvos. O endereço do seu perfil será usado na compra." },
            ["DefaultBadge"] = new() { ["es"] = "Predeterminada", ["en"] = "Default", ["pt"] = "Padrão" },
            ["SetDefaultButton"] = new() { ["es"] = "Predeterminar", ["en"] = "Set as default", ["pt"] = "Definir como padrão" },
            ["TrackOrderTitle"] = new() { ["es"] = "Rastreo del Pedido", ["en"] = "Order Tracking", ["pt"] = "Rastreamento do Pedido" },
            ["OrderCancelledNote"] = new() { ["es"] = "Este pedido fue cancelado.", ["en"] = "This order was cancelled.", ["pt"] = "Este pedido foi cancelado." },
            ["CarrierLabel"] = new() { ["es"] = "Transportadora", ["en"] = "Carrier", ["pt"] = "Transportadora" },
            ["TrackingNumberLabel"] = new() { ["es"] = "Número de guía", ["en"] = "Tracking number", ["pt"] = "Número de rastreio" },
            ["EstimatedDeliveryLabel"] = new() { ["es"] = "Entrega estimada", ["en"] = "Estimated delivery", ["pt"] = "Entrega estimada" },
            ["ShippingAddressLabel"] = new() { ["es"] = "Dirección de envío", ["en"] = "Shipping address", ["pt"] = "Endereço de entrega" },
            ["HistoryLabel"] = new() { ["es"] = "Historial", ["en"] = "History", ["pt"] = "Histórico" },
            ["BackButton"] = new() { ["es"] = "Volver", ["en"] = "Back", ["pt"] = "Voltar" },
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
