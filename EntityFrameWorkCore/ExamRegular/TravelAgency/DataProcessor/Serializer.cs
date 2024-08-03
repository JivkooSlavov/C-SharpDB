using Newtonsoft.Json;
using TravelAgency.Data;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Guides";

            var guides = context.Guides
                .Where(x => x.Language == Language.Spanish)
                .Select(x => new ExportAllGuidesXmlDto
                {
                    FullName = x.FullName,
                    TourPackages = x.TourPackagesGuides
                    .Select(y => new ExportTourPackage
                    {
                        Name = y.TourPackage.PackageName,
                        Description = y.TourPackage.Description,
                        Price = y.TourPackage.Price
                    })
                    .OrderByDescending(y => y.Price)
                    .ThenBy(y => y.Name)
                    .ToArray()
                })
                .OrderByDescending(x => x.TourPackages.Count())
                .ThenBy(x => x.FullName)
                .ToArray();

            return xmlHelper.Serialize(guides, xmlRoot);
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var customers = context.Customers
                .Where(x => x.Bookings.Any(y => y.TourPackage.PackageName == "Horse Riding Tour"))
                .ToArray()
                .Select(x => new ExportAllCustomersJsonDto
                {
                    FullName = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                    Bookings = x.Bookings
                    .Where(y => y.TourPackage.PackageName == "Horse Riding Tour")
                    .Select(y => new ExportBookingsJsonDtoArray
                    {
                        TourPackageName = y.TourPackage.PackageName,
                        Date = y.BookingDate.ToString("yyyy-MM-dd")
                    })
                    .OrderBy(y=>y.Date)
                    .ToArray()
                })
                .OrderByDescending(x=>x.Bookings.Length)
                .ThenBy(x=>x.FullName)
                .ToArray();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }
    }
}
