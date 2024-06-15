namespace Primal.Entities
{
    public class PlayerAbility : BaseEntity
    {
        public int PlayerId { get; set; }
        public int Number { get; set; }
        public int? Battleflow { get; set; }
    }
}
