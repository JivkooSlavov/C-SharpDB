using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Invoices.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            ExportPropertiesWithOwnersJsonDto[] properties = dbContext.Properties
                .Where(x=>x.DateOfAcquisition>= new DateTime(2000,1,1))
                .OrderByDescending(x => x.DateOfAcquisition)
                .ThenBy(x => x.PropertyIdentifier)
                .Select(x=> new ExportPropertiesWithOwnersJsonDto()
                {
                    PropertyIdentifier = x.PropertyIdentifier,
                    Area = x.Area,
                    Address = x.Address,
                    DateOfAcquisition = x.DateOfAcquisition.ToString("dd/MM/yyyy"),
                    Owners = x.PropertiesCitizens
                    .Select(pc => pc.Citizen)
                    .OrderBy(c => c.LastName)
                    .Select(c=> new ExportOwnersJsonDtop()
                    {
                        LastName= c.LastName,
                        MaritalStatus = c.MaritalStatus.ToString()
                    })
                    .OrderBy(x=>x.LastName)
                    .ToArray()
                })

                .ToArray();

            return JsonConvert.SerializeObject(properties, Formatting.Indented);
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {

            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Properties";

            ExportPropertiesXmlDto[] properties = dbContext.Properties
                .Where(x=>x.Area>=100)
                .OrderByDescending(x => x.Area)
                .ThenBy(x => x.DateOfAcquisition)
                .Select(x=> new ExportPropertiesXmlDto() 
                { 
                    PostalCode=x.District.PostalCode,
                   PropertyIdentifier=x.PropertyIdentifier,
                  Area=x.Area,
                  DateOfAcquisition= x.DateOfAcquisition.ToString("dd/MM/yyyy"),
                })
                .ToArray();

            return xmlHelper.Serialize(properties, xmlRoot);
        }
    }
}
