﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            //string usertext = File.ReadAllText("../../../datasets/users.json");
            //Console.WriteLine(ImportUsers(context, usertext));

            //string productText = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, productText));

            //string categoriesText = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, categoriesText));

            //string categoriesAndProduct = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, categoriesAndProduct));

            //Console.WriteLine(GetProductsInRange(context));
            //Console.WriteLine(GetSoldProducts(context));

            //Console.WriteLine(GetCategoriesByProductsCount(context));

            Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var user = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(user);
            context.SaveChanges();

            return $"Successfully imported {user.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);
            categories.RemoveAll(x => x.Name == null);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";

        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var producs = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new ExportProductDto
                {
                    Name = x.Name,
                    Price =  x.Price,
                    Seller = $"{x.Seller.FirstName} {x.Seller.LastName}"
                })
                .ToArray();

                   return FormatJsonMethod(producs);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(u => u.BuyerId != null))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    SoldProducts = x.ProductsSold
                    .Select(p => new
                    {
                        p.Name,
                        p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName
                    })
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToArray();

            return FormatJsonMethod(users);

        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(x => x.CategoriesProducts.Count())
                .Select(x => new
                {
                    Category = x.Name,
                    ProductsCount = x.CategoriesProducts.Count(),
                    AveragePrice = x.CategoriesProducts.Average(p => p.Product.Price).ToString("f2"),
                    TotalRevenue = x.CategoriesProducts.Sum(p => p.Product.Price).ToString("f2")
                })
                .ToArray();

            return FormatJsonMethod(categories);

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null && p.Price != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = u.ProductsSold
                    .Where(p => p.BuyerId != null && p.Price != null)
                    .Select(p => new
                    {
                        p.Name,
                        p.Price
                    }).ToArray()
                })
                .OrderByDescending(u => u.SoldProducts.Length)
                .ToArray();

            var output = new
            {
                UserCount = users.Length,
                Users = users.Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.SoldProducts.Length,
                        Products = u.SoldProducts
                    }
                })
            };

            return FormatJsonMethod(output);
        }
        private static string FormatJsonMethod(object obj)
        {
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}