using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DataProcessor.ExportDtos
{
    public class ExportAllCustomersJsonDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        public ExportBookingsJsonDtoArray[] Bookings {  get; set; }
    }
}
