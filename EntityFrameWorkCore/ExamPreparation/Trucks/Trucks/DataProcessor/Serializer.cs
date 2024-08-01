namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using Trucks.DataProcessor.ExportDto;
    using Trucks.Utilities;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Despatchers";

            ExportDespatchersXmlDto[] despatchersToExport = context.Despatchers
                .Where(x => x.Trucks.Any())
                .Select(x => new ExportDespatchersXmlDto
                {
                    TrucksCount = x.Trucks.Count,
                    DespatcherName = x.Name,
                    Trucks = x.Trucks
                    .Select(t => new ExportTrucksXmlDto
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = t.MakeType.ToString(),
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()

                })
                .OrderByDescending(x=>x.TrucksCount)
                .ThenBy(x=>x.DespatcherName)
                .ToArray();


            return xmlHelper.Serialize(despatchersToExport, xmlRoot);
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .Where(x => x.ClientsTrucks.Any(y => y.Truck.TankCapacity >= capacity))
                .ToArray()
                .Select(x => new ExportClientsJsonDto
                {
                    Name = x.Name,
                    Trucks = x.ClientsTrucks
                    .Where(y => y.Truck.TankCapacity >= capacity)
                    .ToArray()
                    .Select(y => new ExportTrucksJsonDto
                    {
                        TruckRegistrationNumber = y.Truck.RegistrationNumber,
                        VinNumber = y.Truck.VinNumber,
                        TankCapacity = y.Truck.TankCapacity,
                        CargoCapacity = y.Truck.CargoCapacity,
                        CategoryType = y.Truck.CategoryType.ToString(),
                        MakeType = y.Truck.MakeType.ToString()
                    })
                    .OrderBy(y => y.MakeType)
                    .ThenByDescending(y => y.CargoCapacity)
                    .ToArray()
                })
                .OrderByDescending(c => c.Trucks.Length)
                .ThenBy(c => c.Name)
                .Take(10);

            return JsonConvert.SerializeObject(clients, Formatting.Indented);
        }
    }
}
