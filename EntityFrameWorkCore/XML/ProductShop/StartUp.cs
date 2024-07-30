using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            //01
            //string usersXml = File.ReadAllText("../../../Datasets/users.xml");
            //Console.WriteLine(ImportUsers(context, usersXml));
            //02
            string productsXml = File.ReadAllText("../../../Datasets/products.xml");
            Console.WriteLine(ImportProducts(context, productsXml));
            //03
            string categoriesXml = File.ReadAllText("../../../Datasets/categories.xml");
            Console.WriteLine(ImportCategories(context, categoriesXml));
            //04
            string categoriesProductsXml = File.ReadAllText("../../../Datasets/categories-products.xml");
            Console.WriteLine(ImportCategoryProducts(context, categoriesProductsXml));
            //}
            //05
            //Console.WriteLine(GetProductsInRange(context));
            //06
            //Console.WriteLine(GetSoldProducts(context));
            //07
            //Console.WriteLine(GetCategoriesByProductsCount(context));
            //08
            //Console.WriteLine(GetUsersWithProducts(context));

        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UsersImportDto[]),
              new XmlRootAttribute("Users"));
            UsersImportDto[] usersDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                usersDtos = (UsersImportDto[])xmlSerializer.Deserialize(reader);
            }

            User[] users = usersDtos
                .Select(dto => new User()
                {
FirstName = dto.FirstName,
LastName = dto.LastName,
Age = dto.Age
                }).ToArray();
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Length}";
        }
        //02
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProductsImportDto[]),
                new XmlRootAttribute("Products"));
            ProductsImportDto[] productsDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                productsDtos = (ProductsImportDto[])xmlSerializer.Deserialize(reader);
            }

            var products = productsDtos
                .Select(dto => new Product()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    SellerId = dto.SellerId,
                    BuyerId = dto.BuyerId
                }).ToArray();
            context.Products.AddRange(products);
            context.SaveChanges();
          return  $"Successfully imported {products.Length}";
        }
        //03
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CategoriesImportDto[]),
                new XmlRootAttribute("Categories"));
            CategoriesImportDto[] categoriesDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                categoriesDtos = (CategoriesImportDto[])xmlSerializer.Deserialize(reader);
            }

            Category[] categories = categoriesDtos
                .Where(c=>c.Name!=null)
                .Select(dto => new Category()
                {
                    Name = dto.Name
                }).ToArray();
            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Length}";
        }
        //04
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CategoriesProductsImportDto[]),
                new XmlRootAttribute("CategoryProducts"));
            CategoriesProductsImportDto[] categoriesDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                categoriesDtos = (CategoriesProductsImportDto[])xmlSerializer.Deserialize(reader);
            }

            CategoryProduct[] categoriesProducts = categoriesDtos
                .Where(c=>c.CategoryId!=null&&c.ProductId!=null)
                .Select(dto => new CategoryProduct()
                {
                    CategoryId = dto.CategoryId,
                    ProductId = dto.ProductId
                }).ToArray();
            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();
            return $"Successfully imported {categoriesProducts.Length}";
        }
        //05
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p => new ProductsInRangeDto()
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerName = p.BuyerId.HasValue
                        ? $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                        : null
                })
                .ToArray();


            return SerializeToXml(products, "Products");
        }
        //06
        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new GetSoldProductsDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(p => new ProductDto()
                        {
                            Name = p.Name,
                            Price = p.Price,
                        })
                        .ToArray()
                })
                .ToArray();
            return SerializeToXml(products, "Users");
        }
        //07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .AsNoTracking()
                .Select(c => new CategoriesByProductDto()
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();
            return SerializeToXml(categories, "Categories");
        }
        //08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderByDescending(u => u.ProductsSold.Count)
                .Take(10)
                .Select(u => new UserInfo()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsCount()
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold.Select(p => new SoldProduct()
                            {
                                Name = p.Name,
                                Price = p.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                    }
                })
                .Take(10)
                .ToArray();

            GetUsersWithProductsDto getUsers = new GetUsersWithProductsDto()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                Users = users
            };
            return SerializeToXml(getUsers, "Users");
        }
        private static string SerializeToXml<T>(T dto, string xmlRootAttribute, bool omitDeclaration = false)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttribute));
            StringBuilder stringBuilder = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitDeclaration,
                Encoding = new UTF8Encoding(false),
                Indent = true
            };

            using (StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);

                try
                {
                    xmlSerializer.Serialize(xmlWriter, dto, xmlSerializerNamespaces);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return stringBuilder.ToString();
        }
    }
}