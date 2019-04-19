using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public const string successMessage = "Successfully imported {0}";

        public static void Main(string[] args)
        {
            Mapper.Initialize(config => { config.AddProfile<ProductShopProfile>(); });

            string usersXml =
                File.ReadAllText(@"D:\DatabasesAdvancedEntityFramework\11XmlProcessing\ProductShop\Datasets\users.xml");
            string productsXml =
                File.ReadAllText(
                    @"D:\DatabasesAdvancedEntityFramework\11XmlProcessing\ProductShop\Datasets\products.xml");
            string categoriesXml =
                File.ReadAllText(
                    @"D:\DatabasesAdvancedEntityFramework\11XmlProcessing\ProductShop\Datasets\categories.xml");
            string categoriesProductsXml =
                File.ReadAllText(
                    @"D:\DatabasesAdvancedEntityFramework\11XmlProcessing\ProductShop\Datasets\categories-products.xml");

            using (var context = new ProductShopContext())
            {
                //Console.WriteLine(ImportUsers(context, usersXml));
                //Console.WriteLine(ImportProducts(context, productsXml));
                //Console.WriteLine(ImportCategories(context, categoriesXml));
                //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsXml));
                //Console.WriteLine(GetProductsInRange(context));
                //Console.WriteLine(GetSoldProducts(context));
                //Console.WriteLine(GetCategoriesByProductsCount(context));
                Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        //1
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportUserDto[]), new XmlRootAttribute("Users"));

            var usersDto = (ImportUserDto[])serializer.Deserialize(new StringReader(inputXml));

            var users = Mapper.Map<User[]>(usersDto);

            context.Users.AddRange(users);
            int usersImported = context.SaveChanges();

            return string.Format(successMessage, usersImported);
        }

        //2
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));

            var productsDto = (ImportProductDto[])serializer.Deserialize(new StringReader(inputXml));

            var products = Mapper.Map<Product[]>(productsDto);

            context.Products.AddRange(products);
            var productsImported = context.SaveChanges();

            return string.Format(successMessage, productsImported);
        }

        //3
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(ImportCategoryDto[]), new XmlRootAttribute("Categories"));

            var categoriesDto = ((ImportCategoryDto[])serializer.Deserialize(new StringReader(inputXml)))
                .Where(c => c.Name != null);

            var categories = Mapper.Map<Category[]>(categoriesDto);

            context.Categories.AddRange(categories);
            var categoriesImported = context.SaveChanges();

            return string.Format(successMessage, categoriesImported);
        }

        //4
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCategoryProductDto[]),
                new XmlRootAttribute("CategoryProducts"));

            var categoryProductsDto = ((ImportCategoryProductDto[])serializer.Deserialize(new StringReader(inputXml)))
                .Where(cp => context.Categories.Find(cp.CategoryId) != null &&
                             context.Products.Find(cp.ProductId) != null);

            var categoryProducts = Mapper.Map<CategoryProduct[]>(categoryProductsDto);

            context.CategoryProducts.AddRange(categoryProducts);

            var categoryProductsImported = context.SaveChanges();

            return string.Format(successMessage, categoryProductsImported);
        }

        //5
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ExportProductsInRangeDto()
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerFullName = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToArray();

            XmlSerializer serializer =
                new XmlSerializer(typeof(ExportProductsInRangeDto[]), new XmlRootAttribute("Products"));

            StringBuilder result = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("", ""),
            });

            serializer.Serialize(new StringWriter(result), products, namespaces);

            return result.ToString().TrimEnd();
        }

        //6
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(u => u.ProductsSold)
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => Mapper.Map<ExportUserSoldProductDto>(u))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ToArray();

            XmlSerializer serializer =
                new XmlSerializer(typeof(ExportUserSoldProductDto[]), new XmlRootAttribute("Users"));

            StringBuilder result = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("", ""),
            });

            serializer.Serialize(new StringWriter(result), users, namespaces);

            return result.ToString().TrimEnd();
        }

        //7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Include(c => c.CategoryProducts)
                .ThenInclude(cp => cp.Product)
                .Select(c => new ExportCategoriesByProductsCountDto()
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Select(cp => cp.Product.Price).DefaultIfEmpty(0).Average(),
                    TotalRevenue = c.CategoryProducts.Select(cp => cp.Product.Price).DefaultIfEmpty(0).Sum()
                })
                .ToArray()
                .OrderByDescending(cp => cp.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCategoriesByProductsCountDto[]),
                new XmlRootAttribute("Categories"));

            StringBuilder result = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty,
            });

            serializer.Serialize(new StringWriter(result), categories, namespaces);

            return result.ToString().TrimEnd();
        }

        //8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersWithSoldProducts = context.Users
                .Include(u=>u.ProductsSold)
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .ToArray();

            var users = new ExportUsersWithProductsDto()
            {
                Count = usersWithSoldProducts.Length,
                Users = usersWithSoldProducts
                    .Select(u => new ExportUserDto()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts = new ExportSoldProductsDto()
                        {
                            Count = u.ProductsSold.Count,
                            Products = u.ProductsSold.Select(ps => new ExportProductsDto()
                            {
                                Name = ps.Name,
                                Price = ps.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                        }
                    })
                    .OrderByDescending(u => u.SoldProducts.Count)
                    .Take(10)
                    .ToArray()
            };

            XmlSerializer serializer =
                new XmlSerializer(typeof(ExportUsersWithProductsDto), new XmlRootAttribute("Users"));

            StringBuilder result = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty,
            });

            serializer.Serialize(new StringWriter(result), users, namespaces);

            return result.ToString().TrimEnd();
        }
    }
}