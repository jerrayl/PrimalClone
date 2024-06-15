using System.Collections.Generic;

namespace Primal.Models
{
    public class GameStateModel
    {
        public List<PlayerModel> Players { get; set; }
        public BossModel Boss { get; set; }
        public CharacterType? CharacterPerformingAction { get; set; }
        public DisplayAttackModel Attack { get; set; }
    }
}