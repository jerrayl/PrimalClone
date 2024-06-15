namespace Primal.Entities
{
    public class BossAttackPlayer : BaseEntity
    {
        public int BossAttackId { get; set; }
        public int PlayerId { get; set; }
        
        public virtual BossAttack BossAttack { get; set; }
        public virtual Player Player { get; set; }
    }
}
