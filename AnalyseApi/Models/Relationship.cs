namespace AnalyseApi.Models
{
    public class Relationship
    {
        public int RelationshipId { get; set; }
        public RelationType Relation { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public string? SuspectId { get; set; }
    }
}
