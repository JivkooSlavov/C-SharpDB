using Invoices.Data.Models;

using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType(nameof(Client))]
    public class ExportClientsDto
    {
        [XmlElement(nameof(ClientName))]
        public string ClientName { get; set; }

        [XmlElement(nameof(VatNumber))]
        public string VatNumber { get; set; }

        [XmlArray(nameof(Invoices))]
        public ExportClientInvoiceDtop[] Invoices { get; set; } = null!;

        [XmlAttribute(nameof(InvoicesCount))]
        public int InvoicesCount { get; set; }
    }
}
