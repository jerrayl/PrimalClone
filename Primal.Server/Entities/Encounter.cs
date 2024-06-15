using System;
using System.Collections.Generic;

namespace Primal.Entities
{
    public class Encounter : BaseEntity
    {
        public Encounter()
        {
            DateStarted = DateTime.UtcNow;
        }
        
        public DateTime DateStarted { get; set; }
        public CharacterType? CharacterPerformingAction { get; set; }
        public virtual List<EncounterPlayer> EncounterPlayers { get; set; }
        public virtual Boss Boss { get; set; }
        public virtual List<Minion> Minions { get; set; }
        public virtual List<EncounterMightDeck> EncounterMightDecks { get; set; }
    }
}
