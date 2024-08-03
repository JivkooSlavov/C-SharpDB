using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();


            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Customers";

            ICollection<Customer> validCustomer = new List<Customer>();

            ImportCustomersXml[] customers = xmlHelper.Deserialize<ImportCustomersXml[]>(xmlString, xmlRoot);

            foreach (var customerDto in customers)
            {

                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Customer newCustomer = new Customer()
                {
                    FullName = customerDto.FullName,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.phoneNumber
                };

                if (validCustomer.Any(x=>x.FullName==customerDto.FullName) || validCustomer.Any(x => x.Email == customerDto.Email) || validCustomer.Any(x => x.PhoneNumber == customerDto.phoneNumber))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }
                validCustomer.Add(newCustomer);
                sb.AppendLine(string.Format(SuccessfullyImportedCustomer, newCustomer.FullName));
            }
            context.Customers.AddRange(validCustomer);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Booking> validBooking = new List<Booking>();

            ImportBookingsJsonDto[] bookings  = JsonConvert.DeserializeObject<ImportBookingsJsonDto[]>(jsonString)!;

            foreach (var bookingDto in bookings)
            {
                if (!IsValid(bookingDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime bookingDate;
                bool isBookingDateValid = DateTime
                    .TryParseExact(bookingDto.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out bookingDate);

                if (!isBookingDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Customer customerImport = context.Customers.Where(x=>x.FullName==bookingDto.CustomerName).FirstOrDefault();
                TourPackage tourPackageImport = context.TourPackages.Where(y=>y.PackageName==bookingDto.TourPackageName).FirstOrDefault();

                Booking newBooking = new Booking
                {
                    BookingDate = bookingDate,
                    Customer = customerImport,
                    TourPackage = tourPackageImport
                };

                validBooking.Add(newBooking);
                sb.AppendLine(String.Format(SuccessfullyImportedBooking, bookingDto.TourPackageName, bookingDate.ToString("yyyy-MM-dd")));
            }
            context.Bookings.AddRange(validBooking);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
