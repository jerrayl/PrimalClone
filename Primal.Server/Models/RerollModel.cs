using System.Collections.Generic;

namespace Primal.Models
{
    public class RerollModel
    {
        public int AttackId { get; set; }
        public List<int> MightCards { get; set; }
        public int RerollTokensUsed { get; set; }
    }
}