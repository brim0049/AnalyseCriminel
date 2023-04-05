using System.Text.Json.Serialization;

namespace AnalyseApi.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public int? NoStreet { get; set; }
       
        public string? NameStreet { get; set; }
        public string? Ville { get; set; }
        public string? Zip { get; set; }

    }
}
