using System.Collections.Generic;

namespace Primal.Models
{
    public class FreeCompanyModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<PlayerSummaryModel> Players { get; set; }
    }
}