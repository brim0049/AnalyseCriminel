namespace AnalyseApi.Models
{
    public class Call
    {
        public int CallId { get; set; }
        public DateTime Date { get; set; }
        public string? Number { get; set; }
        public string? Zone { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
