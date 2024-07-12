using CarDealer.Data;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using CarDealer.Models;
using CarDealer.DTOs.Import;
using System.Text;
using CarDealer.DTOs.Export;
using System.Globalization;
using Castle.Core.Resource;
using System.IO;
using System.Diagnostics;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {

            CarDealerContext context = new CarDealerContext();

            //9. Import Suppliers
            //string suppliersText = File.ReadAllText("../../../datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context, suppliersText));

            //10. Import Parts
            //string partsText = File.ReadAllText("../../../datasets/parts.json");
            //Console.WriteLine(ImportParts(context, partsText));

            //11. Import Cars
            //string carsText = File.ReadAllText("../../../datasets/cars.json");
            //Console.WriteLine(ImportCars(context, carsText));

            //12. Import Customers
            //string customersText = File.ReadAllText("../../../datasets/customers.json");
            //Console.WriteLine(ImportCustomers(context, customersText));

            //13. Import Sales
            //string salesText = File.ReadAllText("../../../datasets/sales.json");
            //Console.WriteLine(ImportCars(context, salesText));

            //14. Export Ordered Customers
            //Console.WriteLine(GetOrderedCustomers(context));


            //15. Export Cars from Make Toyota
            //Console.WriteLine(GetCarsFromMakeToyota(context));

            //16. Export Local Suppliers
            //Console.WriteLine(GetLocalSuppliers(context));

            //17. Export Cars with Their List of Parts
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            //18. Export Total Sales by Customer
            //Console.WriteLine(GetTotalSalesByCustomer(context));

            //19. Export Sales with Applied Discount
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }


        //9. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        //10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            var validSupplierIds = context.Suppliers
                .Select(x => x.Id)
                .ToArray();

            var partsWithValidId = parts
                .Where(p => validSupplierIds.Contains(p.SupplierId))
                .ToArray();

            context.Parts.AddRange(partsWithValidId);
            context.SaveChanges();

            return $"Successfully imported {partsWithValidId.Length}.";
        }

        //11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDtos = JsonConvert.DeserializeObject<List<ImportCarDto>>(inputJson);

            var cars = new HashSet<Car>();
            var partsCars = new HashSet<PartCar>();

            foreach (var carDto in carsDtos)
            {
                var newCar = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TraveledDistance = carDto.TraveledDistance
                };
                cars.Add(newCar);

                foreach (var partId in carDto.PartsId.Distinct())
                {
                    partsCars.Add(new PartCar()
                    {
                        Car = newCar,
                        PartId = partId
                    });
                }
            }
            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(partsCars);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        //12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        //13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }


        //14. Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {


            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                    x.IsYoungDriver
                })
                .ToArray();

            return FormatJsonMethod(customers);
        }

        //15. Export Cars from Make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var carsOfToyota = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TraveledDistance)
                       .Select(x => new
                       {
                           x.Id,
                           x.Make,
                           x.Model,
                           x.TraveledDistance
                       })
                .ToArray();

            return FormatJsonMethod(carsOfToyota);
        }

        //16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                       .Select(x => new
                       {
                           x.Id,
                           x.Name,
                           PartsCount = x.Parts.Count

                       })
                .ToArray();

            return FormatJsonMethod(suppliers);
        }


        //17. Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsAndParts = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance
                    },

                    parts = c.PartsCars
                    .Select(p => new
                    {
                        p.Part.Name,
                        Price = p.Part.Price.ToString("f2")
                    })
                })
                .ToArray();

            return FormatJsonMethod(carsAndParts);
        }


        //18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(x => x.Sales.Count > 0)
                .Select(x => new
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count(),
                    SpentMoney = x.Sales.SelectMany(s => s.Car.PartsCars).Sum(pc => pc.Part.Price)
                })
                .OrderByDescending(x => x.SpentMoney)
                .ThenBy(x => x.BoughtCars)
                .ToArray();

            return FormatJsonMethod(customers);
        }

        //19. Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var customers = context.Sales
                .Take(10)
                .Select(c => new
                {
                    car = new
                    {
                        c.Car.Make,
                        c.Car.Model,
                        c.Car.TraveledDistance
                    },
                    customerName = c.Customer.Name,
                    discount = c.Discount.ToString("F2"),
                    price = c.Car.PartsCars.Sum(p => p.Part.Price).ToString("F2"),
                    priceWithDiscount = ((1 - c.Discount / 100) * c.Car.PartsCars.Sum(p => p.Part.Price)).ToString("F2")
                });

            return FormatJsonMethod(customers);
        }
        private static string FormatJsonMethod(object obj)
        {
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
                
            };

            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}