namespace Primal.Entities
{
    public class AttackMinion : BaseEntity
    {
        public int AttackId { get; set; }
        public int MinionId { get; set; }

        public virtual Attack Attack { get; set; }
        public virtual Minion Minion { get; set; }
    }
}
