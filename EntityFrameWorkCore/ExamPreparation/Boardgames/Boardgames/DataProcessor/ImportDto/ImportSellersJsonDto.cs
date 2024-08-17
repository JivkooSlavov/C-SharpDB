using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Boardgames.Data.Constraints;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellersJsonDto
    {
        [Required]
        [MinLength(SellerNameMinLength)]
        [MaxLength(SellerNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(SellerAddressMinLength)]
        [MaxLength(SellerAddressMaxLength)]
        public string Address { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [RegularExpression(@"^www\.[a-zA-Z0-9\-]+\.com$")]
        public string Website { get; set; }

        [JsonProperty("Boardgames")]
        public int[] BoardgamesId {  get; set; }
    }
}
