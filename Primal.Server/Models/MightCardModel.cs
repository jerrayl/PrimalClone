namespace Primal.Models
{
    public class MightCardModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public Might Type { get; set; }
        public bool IsCritical { get; set; }
        public bool IsDrawnFromCritical { get; set; }
    }
}