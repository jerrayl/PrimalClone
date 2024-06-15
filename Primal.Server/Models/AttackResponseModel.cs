using System.Collections.Generic;

namespace Primal.Models
{
    public class AttackResponseModel
    {
        public int AttackId { get; set; }
        public List<MightCardModel> CardsDrawn { get; set; }
    }
}