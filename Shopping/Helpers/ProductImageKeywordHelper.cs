namespace Shopping.Helpers
{
    /// <summary>
    /// Maps known product names (and a first-word fallback) to a curated,
    /// real stock photo hosted on Unsplash's static CDN (images.unsplash.com),
    /// which is not rate-limited like keyword-search photo APIs. Used so
    /// product cards show a relevant photo without downloading/storing files.
    /// </summary>
    public static class ProductImageKeywordHelper
    {
        private static readonly Dictionary<string, string> ImageUrlsByName = new()
        {
            ["Laptop Ultradelgada 14\" 16GB RAM / 512GB SSD"] = "photo-1496181133206-80ce9b88a853",
            ["Smartphone Pro 128GB cámara triple 108MP"] = "photo-1511707171634-5f897ff02aa9",
            ["Audífonos Inalámbricos con Cancelación de Ruido"] = "photo-1505740420928-5e560c06d30e",
            ["Smartwatch Serie 5 GPS y monitor cardíaco"] = "photo-1523275335684-37898b6baf30",
            ["Tablet 10.5\" 64GB Wi-Fi"] = "photo-1544244015-0df4b3ffc6b0",
            ["Chaqueta de Mezclilla Clásica Unisex"] = "photo-1544022613-e87ca75a784a",
            ["Camiseta Básica 100% Algodón"] = "photo-1521572163474-6864f9cf17ab",
            ["Vestido Casual Estampado Floral"] = "photo-1572804013309-59a88b7e92f1",
            ["Pantalón Chino Slim Fit"] = "photo-1473966968600-fa801b869a1a",
            ["Sudadera con Capucha Oversize"] = "photo-1556821840-3a63f95609a7",
            ["Consola de Videojuegos 1TB Edición Estándar"] = "photo-1486401899868-0e435ed85128",
            ["Silla Gamer Ergonómica Reclinable"] = "photo-1598550476439-6847785fcea6",
            ["Teclado Mecánico RGB Switch Rojo"] = "photo-1587829741301-dc798b83add3",
            ["Mouse Gamer Inalámbrico 16000 DPI"] = "photo-1527814050087-3793815479db",
            ["Audífonos Gamer Surround 7.1"] = "photo-1599669454699-248893623440",
            ["Set de Maquillaje Profesional 12 Piezas"] = "photo-1512496015851-a90fb38ba796",
            ["Crema Hidratante Facial con Ácido Hialurónico"] = "photo-1556228720-195a672e8a03",
            ["Perfume Floral 100ml"] = "photo-1541643600914-78b084683601",
            ["Secadora de Cabello Iónica Profesional"] = "photo-1522338140262-f46f5913618a",
            ["Paleta de Sombras Tonos Tierra"] = "photo-1583241800698-e8ab01830a07",
            ["Proteína Whey Sabor Vainilla 2kg"] = "photo-1593095948071-474c5cc2989d",
            ["Multivitamínico Diario 90 Cápsulas"] = "photo-1584017911766-d451b3d0e843",
            ["Batido Sustituto de Comida Chocolate 1kg"] = "photo-1553530666-ba11a7da3888",
            ["Barra Energética Caja x12"] = "photo-1571748982800-fa51082c2224",
            ["Colágeno Hidrolizado en Polvo 300g"] = "photo-1607619056574-7b8d3ee536b2",
        };

        private static readonly Dictionary<string, string> ImageUrlsByFirstWord = new(StringComparer.OrdinalIgnoreCase)
        {
            ["laptop"] = "photo-1496181133206-80ce9b88a853",
            ["smartphone"] = "photo-1511707171634-5f897ff02aa9",
            ["audífonos"] = "photo-1505740420928-5e560c06d30e",
            ["smartwatch"] = "photo-1523275335684-37898b6baf30",
            ["tablet"] = "photo-1544244015-0df4b3ffc6b0",
            ["chaqueta"] = "photo-1544022613-e87ca75a784a",
            ["camiseta"] = "photo-1521572163474-6864f9cf17ab",
            ["vestido"] = "photo-1572804013309-59a88b7e92f1",
            ["pantalón"] = "photo-1473966968600-fa801b869a1a",
            ["sudadera"] = "photo-1556821840-3a63f95609a7",
            ["consola"] = "photo-1486401899868-0e435ed85128",
            ["silla"] = "photo-1598550476439-6847785fcea6",
            ["teclado"] = "photo-1587829741301-dc798b83add3",
            ["mouse"] = "photo-1527814050087-3793815479db",
            ["set"] = "photo-1512496015851-a90fb38ba796",
            ["crema"] = "photo-1556228720-195a672e8a03",
            ["perfume"] = "photo-1541643600914-78b084683601",
            ["secadora"] = "photo-1522338140262-f46f5913618a",
            ["paleta"] = "photo-1583241800698-e8ab01830a07",
            ["proteína"] = "photo-1593095948071-474c5cc2989d",
            ["multivitamínico"] = "photo-1584017911766-d451b3d0e843",
            ["batido"] = "photo-1553530666-ba11a7da3888",
            ["barra"] = "photo-1571748982800-fa51082c2224",
            ["colágeno"] = "photo-1607619056574-7b8d3ee536b2",
        };

        /// <summary>
        /// Returns a real, relevant product photo URL for known/seeded products.
        /// Falls back to a generic (but reliable, non rate-limited) stock photo
        /// seeded by the product name/id for anything not in the curated list.
        /// </summary>
        public static string GetImageUrl(string productName, int productId)
        {
            if (!string.IsNullOrWhiteSpace(productName))
            {
                if (ImageUrlsByName.TryGetValue(productName, out string exactPhoto))
                {
                    return $"https://images.unsplash.com/{exactPhoto}?w=600&h=600&fit=crop&q=80";
                }

                string firstWord = productName.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? productName;
                if (ImageUrlsByFirstWord.TryGetValue(firstWord, out string mappedPhoto))
                {
                    return $"https://images.unsplash.com/{mappedPhoto}?w=600&h=600&fit=crop&q=80";
                }
            }

            // Generic fallback for products not in the curated catalog (e.g. newly created by an admin).
            string seed = Uri.EscapeDataString($"{productName}-{productId}");
            return $"https://picsum.photos/seed/{seed}/500/500";
        }
    }
}
