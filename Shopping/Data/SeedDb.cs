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
                _context.Categories.Add(new Category { Name = "Tecnología" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Gamer" });
                _context.Categories.Add(new Category { Name = "Belleza" });
                _context.Categories.Add(new Category { Name = "Nutrición" });
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

            foreach (KeyValuePair<string, (string Description, decimal Price, float Stock)[]> entry in productsByCategory)
            {
                Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == entry.Key);

                if (category == null)
                {
                    continue;
                }

                foreach ((string Description, decimal Price, float Stock) item in entry.Value)
                {
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
