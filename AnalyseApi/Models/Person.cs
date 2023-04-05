using System.Text.Json.Serialization;

namespace AnalyseApi.Models
{
    public enum CriminalRecordType
    {
        Oui,
        Non
    }
    public enum RelationType
    {
        Frere,
        Soeur,
        Cousin,
        Ami,
        Proche,
        Aucun
    }
    public class Person
    {
        public Person()
        {
            Cars = new List<Car>();
            Calls = new List<Call>();
            Relations = new List<Relationship>();

        }

        public int PersonId { get; set; }
        public string? NIN { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public CriminalRecordType CriminalRecord { get; set; }        

        //Liaison one to one
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public int? EventId { get; set; }
        public Event? Event { get; set; }
        public ICollection<Call>? Calls { get; set; }
        public ICollection<Car>? Cars { get; set; }
        public ICollection<Relationship>? Relations { get; set; }
    }
}
