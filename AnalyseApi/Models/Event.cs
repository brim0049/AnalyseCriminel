namespace AnalyseApi.Models
{
    public enum TypeEvent
    {
        Aucun,
        Vol,
        Aggression,
        Assassinat,
    }
    public class Event
    {
     

        public int EventId { get; set; }
        public DateTime Date { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public TypeEvent Type { get; set; }
    }
}
