using System.Collections.Generic;

namespace Primal.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }

        public virtual List<Player> Players { get; set; }
    }
}
