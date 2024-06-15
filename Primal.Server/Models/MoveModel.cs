using System.Collections.Generic;

namespace Primal.Models
{
    public class MoveModel
    {
        public int PlayerId { get; set; }
        public List<Position> Positions { get; set; }
    }
}