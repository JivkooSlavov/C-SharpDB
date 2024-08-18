namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Clients";

            ExportClientsDto[] clientsToExport = context.Clients
                .Where(c => c.Invoices.Any(i => DateTime.Compare(i.IssueDate, date) > 0))
                .Select(c => new ExportClientsDto
                {
                    ClientName = c.Name,
                    VatNumber = c.NumberVat,
                    Invoices = c.Invoices
                      .OrderBy(i => i.IssueDate)
                      .ThenByDescending(i => i.DueDate)
                      .Select(i => new ExportClientInvoiceDtop
                      {
                          InvoiceNumber = i.Number,
                          InvoiceAmount = i.Amount,
                          Currency = i.CurrencyType.ToString(),
                          DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture)
                      })
                      .ToArray(),
                    InvoicesCount = c.Invoices.Count
                })
                .OrderByDescending(c => c.InvoicesCount)
                .ThenBy(c => c.ClientName)
                .ToArray();

            return xmlHelper.Serialize(clientsToExport, xmlRoot);
                
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            ExmportProductDto[] exmportProductDto = context.Products
            .Where(p => p.ProductsClients.Any())
             .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
             .Select(p=>new ExmportProductDto 
             {
               Name = p.Name,
               Price = p.Price,
               Category = p.CategoryType.ToString(),
                 Clients = p.ProductsClients
                 .Where(pc=>pc.Client.Name.Length >= nameLength)
                 .Select(pc=>new ExportProductClientsDto
                 {
                     Name = pc.Client.Name,
                     NumberVat = pc.Client.NumberVat
                 })
                 .OrderBy(c=>c.Name)
                 .ToArray()
             })
             .OrderByDescending(p=>p.Clients.Length)
             .ThenBy(p=>p.Name)
             .Take(5)
             .ToArray();

            return JsonConvert.SerializeObject(exmportProductDto, Formatting.Indented);
        }
    }
}