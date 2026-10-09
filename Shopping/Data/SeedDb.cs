using Microsoft.EntityFrameworkCore;
using Shopping.Data.Entities;
using Shopping.Enums;
using Shopping.Helpers;

namespace Shopping.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
            await CheckProductsAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Jorge", "Carrillo", "jcq@yopmail.com", "123 456 7890", "Calle Luna Calle Sol", UserType.Admin);
            await CheckExchangeRatesAsync();

        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);

            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }
            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                Dictionary<string, (string En, string Pt)> categoryTranslations = new()
                {
                    ["Tecnología"] = ("Technology", "Tecnologia"),
                    ["Ropa"] = ("Clothing", "Roupas"),
                    ["Gamer"] = ("Gaming", "Gamer"),
                    ["Belleza"] = ("Beauty", "Beleza"),
                    ["Nutrición"] = ("Nutrition", "Nutrição"),
                };

                foreach (KeyValuePair<string, (string En, string Pt)> entry in categoryTranslations)
                {
                    _context.Categories.Add(new Category
                    {
                        Name = entry.Key,
                        Translations = new List<CategoryTranslation>
                        {
                            new CategoryTranslation { LanguageCode = "en", Name = entry.Value.En },
                            new CategoryTranslation { LanguageCode = "pt", Name = entry.Value.Pt },
                        },
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "México",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Colima",
                            Cities = new List<City>()
                            {
                                new City() {Name = "Tecomán"},
                                new City() {Name = "Colima"},
                                new City() {Name = "Cuauhtémoc"},
                                new City() {Name = "Comala"},
                                new City() {Name = "Coquimatlán"},
                                new City() {Name = "Manzanillo"},
                                new City() {Name = "Armería"},
                                new City() {Name = "Ixtlahuacán"},
                                new City() {Name = "Minatitlán"},
                                new City() {Name = "Villa de Álvarez"}
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Antioquia",
                            Cities = new List<City>() {
                                new City() { Name = "Medellín" },
                                new City() { Name = "Itagüí" },
                                new City() { Name = "Envigado" },
                                new City() { Name = "Bello" },
                                new City() { Name = "Rionegro" },
                            }
                        },
                        new State()
                        {
                            Name = "Bogotá",
                            Cities = new List<City>() {
                                new City() { Name = "Usaquen" },
                                new City() { Name = "Champinero" },
                                new City() { Name = "Santa fe" },
                                new City() { Name = "Useme" },
                                new City() { Name = "Bosa" },
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Florida",
                            Cities = new List<City>() {
                                new City() { Name = "Orlando" },
                                new City() { Name = "Miami" },
                                new City() { Name = "Tampa" },
                                new City() { Name = "Fort Lauderdale" },
                                new City() { Name = "Key West" },
                            }
                        },
                        new State()
                        {
                            Name = "Texas",
                            Cities = new List<City>() {
                                new City() { Name = "Houston" },
                                new City() { Name = "San Antonio" },
                                new City() { Name = "Dallas" },
                                new City() { Name = "Austin" },
                                new City() { Name = "El Paso" },
                            }
                        },
                    }
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckProductsAsync()
        {
            if (_context.Products.Any())
            {
                return;
            }

            Dictionary<string, (string Description, decimal Price, float Stock)[]> productsByCategory = new()
            {
                ["Tecnología"] = new[]
                {
                    ("Laptop Ultradelgada 14\" 16GB RAM / 512GB SSD", 15999.00m, 12f),
                    ("Smartphone Pro 128GB cámara triple 108MP", 11499.00m, 25f),
                    ("Audífonos Inalámbricos con Cancelación de Ruido", 1899.00m, 40f),
                    ("Smartwatch Serie 5 GPS y monitor cardíaco", 2599.00m, 30f),
                    ("Tablet 10.5\" 64GB Wi-Fi", 4299.00m, 18f),
                },
                ["Ropa"] = new[]
                {
                    ("Chaqueta de Mezclilla Clásica Unisex", 899.00m, 50f),
                    ("Camiseta Básica 100% Algodón", 249.00m, 120f),
                    ("Vestido Casual Estampado Floral", 599.00m, 35f),
                    ("Pantalón Chino Slim Fit", 679.00m, 60f),
                    ("Sudadera con Capucha Oversize", 749.00m, 45f),
                },
                ["Gamer"] = new[]
                {
                    ("Consola de Videojuegos 1TB Edición Estándar", 9999.00m, 10f),
                    ("Silla Gamer Ergonómica Reclinable", 3499.00m, 15f),
                    ("Teclado Mecánico RGB Switch Rojo", 1299.00m, 28f),
                    ("Mouse Gamer Inalámbrico 16000 DPI", 899.00m, 32f),
                    ("Audífonos Gamer Surround 7.1", 1099.00m, 22f),
                },
                ["Belleza"] = new[]
                {
                    ("Set de Maquillaje Profesional 12 Piezas", 1299.00m, 20f),
                    ("Crema Hidratante Facial con Ácido Hialurónico", 399.00m, 55f),
                    ("Perfume Floral 100ml", 899.00m, 30f),
                    ("Secadora de Cabello Iónica Profesional", 749.00m, 24f),
                    ("Paleta de Sombras Tonos Tierra", 459.00m, 38f),
                },
                ["Nutrición"] = new[]
                {
                    ("Proteína Whey Sabor Vainilla 2kg", 1199.00m, 26f),
                    ("Multivitamínico Diario 90 Cápsulas", 349.00m, 60f),
                    ("Batido Sustituto de Comida Chocolate 1kg", 699.00m, 34f),
                    ("Barra Energética Caja x12", 299.00m, 80f),
                    ("Colágeno Hidrolizado en Polvo 300g", 549.00m, 40f),
                },
            };

            Dictionary<string, (string En, string Pt)> categoryTranslations = new()
            {
                ["Tecnología"] = ("Technology", "Tecnologia"),
                ["Ropa"] = ("Clothing", "Roupas"),
                ["Gamer"] = ("Gaming", "Gamer"),
                ["Belleza"] = ("Beauty", "Beleza"),
                ["Nutrición"] = ("Nutrition", "Nutrição"),
            };

            Dictionary<string, (string En, string Pt)> productNameTranslations = new()
            {
                ["Laptop Ultradelgada 14\" 16GB RAM / 512GB SSD"] = ("Ultra-thin Laptop 14\" 16GB RAM / 512GB SSD", "Laptop Ultrafino 14\" 16GB RAM / 512GB SSD"),
                ["Smartphone Pro 128GB cámara triple 108MP"] = ("Smartphone Pro 128GB triple camera 108MP", "Smartphone Pro 128GB câmera tripla 108MP"),
                ["Audífonos Inalámbricos con Cancelación de Ruido"] = ("Wireless Headphones with Noise Cancellation", "Fones de Ouvido Sem Fio com Cancelamento de Ruído"),
                ["Smartwatch Serie 5 GPS y monitor cardíaco"] = ("Smartwatch Series 5 GPS and heart rate monitor", "Smartwatch Série 5 GPS e monitor cardíaco"),
                ["Tablet 10.5\" 64GB Wi-Fi"] = ("Tablet 10.5\" 64GB Wi-Fi", "Tablet 10.5\" 64GB Wi-Fi"),
                ["Chaqueta de Mezclilla Clásica Unisex"] = ("Classic Unisex Denim Jacket", "Jaqueta Jeans Clássica Unissex"),
                ["Camiseta Básica 100% Algodón"] = ("Basic 100% Cotton T-Shirt", "Camiseta Básica 100% Algodão"),
                ["Vestido Casual Estampado Floral"] = ("Casual Floral Print Dress", "Vestido Casual Estampado Floral"),
                ["Pantalón Chino Slim Fit"] = ("Slim Fit Chino Pants", "Calça Chino Slim Fit"),
                ["Sudadera con Capucha Oversize"] = ("Oversize Hooded Sweatshirt", "Moletom com Capuz Oversize"),
                ["Consola de Videojuegos 1TB Edición Estándar"] = ("Video Game Console 1TB Standard Edition", "Console de Videogame 1TB Edição Padrão"),
                ["Silla Gamer Ergonómica Reclinable"] = ("Ergonomic Reclining Gaming Chair", "Cadeira Gamer Ergonômica Reclinável"),
                ["Teclado Mecánico RGB Switch Rojo"] = ("Mechanical Keyboard RGB Red Switch", "Teclado Mecânico RGB Switch Vermelho"),
                ["Mouse Gamer Inalámbrico 16000 DPI"] = ("Wireless Gaming Mouse 16000 DPI", "Mouse Gamer Sem Fio 16000 DPI"),
                ["Audífonos Gamer Surround 7.1"] = ("Gaming Headset Surround 7.1", "Headset Gamer Surround 7.1"),
                ["Set de Maquillaje Profesional 12 Piezas"] = ("Professional Makeup Set 12 Pieces", "Kit de Maquiagem Profissional 12 Peças"),
                ["Crema Hidratante Facial con Ácido Hialurónico"] = ("Facial Moisturizing Cream with Hyaluronic Acid", "Creme Hidratante Facial com Ácido Hialurônico"),
                ["Perfume Floral 100ml"] = ("Floral Perfume 100ml", "Perfume Floral 100ml"),
                ["Secadora de Cabello Iónica Profesional"] = ("Professional Ionic Hair Dryer", "Secador de Cabelo Iônico Profissional"),
                ["Paleta de Sombras Tonos Tierra"] = ("Earth Tones Eyeshadow Palette", "Paleta de Sombras Tons Terrosos"),
                ["Proteína Whey Sabor Vainilla 2kg"] = ("Whey Protein Vanilla Flavor 2kg", "Whey Protein Sabor Baunilha 2kg"),
                ["Multivitamínico Diario 90 Cápsulas"] = ("Daily Multivitamin 90 Capsules", "Multivitamínico Diário 90 Cápsulas"),
                ["Batido Sustituto de Comida Chocolate 1kg"] = ("Chocolate Meal Replacement Shake 1kg", "Shake Substituto de Refeição Chocolate 1kg"),
                ["Barra Energética Caja x12"] = ("Energy Bar Box x12", "Barra Energética Caixa x12"),
                ["Colágeno Hidrolizado en Polvo 300g"] = ("Hydrolyzed Collagen Powder 300g", "Colágeno Hidrolisado em Pó 300g"),
            };

            foreach (KeyValuePair<string, (string Description, decimal Price, float Stock)[]> entry in productsByCategory)
            {
                Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == entry.Key);

                if (category == null)
                {
                    continue;
                }

                (string CategoryEn, string CategoryPt) = categoryTranslations.TryGetValue(entry.Key, out (string En, string Pt) categoryNames)
                    ? categoryNames
                    : (entry.Key, entry.Key);

                foreach ((string Description, decimal Price, float Stock) item in entry.Value)
                {
                    (string NameEn, string NamePt) = productNameTranslations.TryGetValue(item.Description, out (string En, string Pt) names)
                        ? names
                        : (item.Description, item.Description);

                    Product product = new()
                    {
                        Name = item.Description,
                        Description = $"{item.Description}. Producto de la categoría {category.Name}, disponible para entrega inmediata.",
                        Price = item.Price,
                        Stock = item.Stock,
                        ProductCategories = new List<ProductCategory>
                        {
                            new ProductCategory { Category = category },
                        },
                        ProductImages = new List<ProductImage>
                        {
                            new ProductImage { ImageId = Guid.Empty },
                        },
                        Translations = new List<ProductTranslation>
                        {
                            new ProductTranslation
                            {
                                LanguageCode = "en",
                                Name = NameEn,
                                Description = $"{NameEn}. Product from the {CategoryEn} category, available for immediate delivery.",
                            },
                            new ProductTranslation
                            {
                                LanguageCode = "pt",
                                Name = NamePt,
                                Description = $"{NamePt}. Produto da categoria {CategoryPt}, disponível para entrega imediata.",
                            },
                        },
                    };

                    _context.Products.Add(product);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckExchangeRatesAsync()
        {
            if (!_context.ExchangeRates.Any())
            {
                _context.ExchangeRates.Add(new ExchangeRate { CurrencyCode = "MXN", Rate = 1m, LastUpdated = DateTime.UtcNow });
                _context.ExchangeRates.Add(new ExchangeRate { CurrencyCode = "USD", Rate = 0.059m, LastUpdated = DateTime.UtcNow });
                _context.ExchangeRates.Add(new ExchangeRate { CurrencyCode = "BRL", Rate = 0.30m, LastUpdated = DateTime.UtcNow });
                await _context.SaveChangesAsync();
            }
        }

    }
}
