using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType(nameof(Despatcher))]
    public class ExportDespatchersXmlDto
    {
        [XmlElement(nameof(DespatcherName))]
        public string DespatcherName {  get; set; }

        [XmlAttribute(nameof(TrucksCount))]
        public int TrucksCount { get; set; }

        [XmlArray(nameof(Trucks))]
        public ExportTrucksXmlDto[] Trucks {  get; set; }
    }
}
