namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();


            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Despatchers";

            ICollection<Despatcher> validDespatchers = new List<Despatcher>();

            ImportDespatchersXmlDto[] despatchers = xmlHelper.Deserialize<ImportDespatchersXmlDto[]>(xmlString, xmlRoot);

            foreach (ImportDespatchersXmlDto despDto in despatchers)
            {
                if (!IsValid(despDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (despDto.Position==null || despDto.Position ==string.Empty)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher despatcher = new Despatcher
                {
                    Name = despDto.Name,
                    Position = despDto.Position
                };

                foreach (ImportTrucksXmlDto truckDto in despDto.Trucks)
                {
                    if (!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = new Truck
                    {
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity,
                        CategoryType = (CategoryType)truckDto.CategoryType,
                        MakeType = (MakeType)truckDto.MakeType,
                    };
                    despatcher.Trucks.Add(truck);
                }

                validDespatchers.Add(despatcher);
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));

            }
            context.Despatchers.AddRange(validDespatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ICollection<Client> clientToImport = new List<Client>();

            ImportClientsJsonDto[] deserializedInvoices = JsonConvert.DeserializeObject<ImportClientsJsonDto[]>(jsonString)!;

            foreach (var clientsDto in deserializedInvoices)
            {
                if (!IsValid(clientsDto) || clientsDto.Type== "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client
                {
                    Name = clientsDto.Name,
                    Nationality = clientsDto.Nationality,
                    Type = clientsDto.Type
                };

                foreach (var truckId in clientsDto.Trucks.Distinct())
                {
                    Truck truck = context.Trucks.Find(truckId);
                    if(truck == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    client.ClientsTrucks.Add(new ClientTruck
                    {
                        Truck = truck
                    });

                }
                clientToImport.Add(client);
                sb.AppendLine(String.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count()));
            }
            context.Clients.AddRange(clientToImport);
            context.SaveChanges();

            return sb.ToString();
        }

       private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}