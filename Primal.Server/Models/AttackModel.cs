using System.Collections.Generic;

namespace Primal.Models
{
    public class AttackModel
    {
        public Position Target { get; set; }
        public Dictionary<Might, int> Might { get; set; }
        public int EmpowerTokensUsed { get; set; }
        public int PlayerId { get; set; }
    }
}