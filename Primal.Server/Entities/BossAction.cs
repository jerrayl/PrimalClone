namespace Primal.Entities
{
    public class BossAction : BaseEntity
    {
        public int BossId { get; set; }
        public int Stage { get; set; }
        public int Number { get; set; }

        public virtual Boss Boss { get; set; }
    }
}
