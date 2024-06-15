namespace Primal.Models
{
    public class DisplayAttackModel : AttackResponseModel
    {
        public int AttackerId { get; set; }
        public CharacterType CharacterType { get; set; }
    }
}