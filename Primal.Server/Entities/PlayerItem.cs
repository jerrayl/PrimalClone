namespace Primal.Entities
{
    public class PlayerItem : BaseEntity
    {
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public int? Battleflow { get; set; }

        public virtual Item Item { get; set; }
    }
}
