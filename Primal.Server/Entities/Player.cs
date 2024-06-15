using System.Collections.Generic;

namespace Primal.Entities
{
    public class Player : BaseEntity
    {
        public int UserId { get; set; }
        public int? FreeCompanyId { get; set; }
        public string Name { get; set; }
        public Class Class { get; set; }
        public int Health { get; set; }
        public int Defence { get; set; }
        public int MaxAnimus { get; set; }
        public int AnimusRegen { get; set; }
        public Dictionary<Might, int> Might { get; set; }

        public virtual User User { get; set; }
        public virtual FreeCompany FreeCompany { get; set; }
        public virtual EncounterPlayer EncounterPlayer { get; set; }
        public virtual List<PlayerItem> PlayerItems { get; set; }
        public virtual List<PlayerAbility> PlayerAbilities { get; set; }
        public virtual List<Attack> Attacks { get; set; }
    }
}
