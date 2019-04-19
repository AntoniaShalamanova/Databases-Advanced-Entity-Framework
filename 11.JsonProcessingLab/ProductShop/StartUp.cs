using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Models;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            //var usersJson = File.ReadAllText(@"D:\DatabasesAdvancedEntityFramework\10JsonProcessing\ProductShop\Datasets\users.json");
            //var productsJson = File.ReadAllText(@"D:\DatabasesAdvancedEntityFramework\10JsonProcessing\ProductShop\Datasets\products.json");
            //var categoriesJson = File.ReadAllText(@"D:\DatabasesAdvancedEntityFramework\10JsonProcessing\ProductShop\Datasets\categories.json");
            //var categoriesProductsJson = File.ReadAllText(@"D:\DatabasesAdvancedEntityFramework\10JsonProcessing\ProductShop\Datasets\categories-products.json");

            using (var context = new ProductShopContext())
            {
                //Console.WriteLine(ImportUsers(context, usersJson));
                //Console.WriteLine(ImportProducts(context, productsJson));
                //Console.WriteLine(ImportCategories(context, categoriesJson));
                //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsJson));
                //Console.WriteLine(GetProductsInRange(context));
                //Console.WriteLine(GetSoldProducts(context));
                //Console.WriteLine(GetCategoriesByProductsCount(context));
                Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        //1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            User[] users = JsonConvert.DeserializeObject<User[]>(inputJson);

            foreach (var user in users)
            {
                if (IsValid(user))
                {
                    context.Users.Add(user);
                }
            }

            var insertedUsersCount = context.SaveChanges();

            return $"Successfully imported {insertedUsersCount}";
        }

        //2
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            Product[] products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            foreach (var product in products)
            {
                if (IsValid(product))
                {
                    context.Products.Add(product);
                }
            }

            var insertedProductsCount = context.SaveChanges();

            return $"Successfully imported {insertedProductsCount}";
        }

        //3
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            Category[] categories = JsonConvert.DeserializeObject<Category[]>(inputJson);

            foreach (var category in categories)
            {
                if (IsValid(category))
                {
                    context.Categories.Add(category);
                }
            }

            var insertedCategoriesCount = context.SaveChanges();

            return $"Successfully imported {insertedCategoriesCount}";
        }

        //4
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            CategoryProduct[] categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            foreach (var categoryProduct in categoryProducts)
            {
                if (IsValid(categoryProduct))
                {
                    context.CategoryProducts.Add(categoryProduct);
                }
            }

            var insertedCatProdCount = context.SaveChanges();

            return $"Successfully imported {insertedCatProdCount}";
        }

        //5
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .OrderBy(p => p.Price)
                .ToArray();

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };


            return JsonConvert.SerializeObject(products, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }

        //6
        public static string GetSoldProducts(ProductShopContext context)
        {
            var sellers = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            Name = p.Name,
                            Price = p.Price,
                            BuyerFirstName = p.Buyer.FirstName,
                            BuyerLastName = p.Buyer.LastName
                        }).ToArray()
                })
                .ToArray();

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };


            return JsonConvert.SerializeObject(sellers, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }

        //7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoryProducts.Count,
                    AveragePrice = $"{c.CategoryProducts.Average(cp => cp.Product.Price):F2}",
                    TotalRevenue = $"{c.CategoryProducts.Sum(cp => cp.Product.Price):F2}"

                }).ToArray()
                .OrderByDescending(c => c.ProductsCount);

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };


            return JsonConvert.SerializeObject(categories, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
            });
        }

        //8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var sellers = new
            {
                UsersCount = context.Users
                    .Count(u => u.ProductsSold.Any(p => p.Buyer != null)),
                Users = context.Users
                    .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                    .Select(u => new
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts = new
                        {
                            Count = u.ProductsSold
                                .Count(p => p.Buyer != null),
                            Products = u.ProductsSold
                                .Where(p => p.Buyer != null)
                                .Select(p => new
                                {
                                    Name = p.Name,
                                    Price = p.Price,
                                }).ToArray()
                        }
                    })
                    .OrderByDescending(u => u.SoldProducts.Count)
                    .ToArray()
            };

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };


            return JsonConvert.SerializeObject(sellers, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}