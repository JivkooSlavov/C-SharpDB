using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Invoices.Data.Constaints;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDto
    {
        [Required]
        [Range(InvoiceNumberMinLength,InvoiceNumberMaxLength)]
        public int Number { get; set; }

        [Required]
        public string IssueDate { get; set; } //DateTime => Deserialize as a string!

        [Required]  
        public string DueDate { get; set; } //DateTime => Deserialize as a string!

        [Required]
        public decimal Amount { get; set; }
        [Required]
        [Range (InvoiceMinCurrencyType,InvoiceMaxCurrencyType)]
        public int CurrencyType { get; set; } //Enum => Deserialize as a integer!
        [Required]
        public int ClientId { get; set; }
    }
}
