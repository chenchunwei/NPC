using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Proposals;

namespace NPC.Application.ManageModels.Proposals
{
    public class GovOfficeAuditModel : ProposalBasicModel
    {
        public GovOfficeAuditModel()
        {
            SubsidiaryUnitIds = new List<Guid>();
            UnitOptions = new Dictionary<string, string>();
            SubsidiaryOptions = new Dictionary<string, string>();
        }
        public string Comment { get; set; }
        public Guid? SponsorUnitId { get; set; }
        public IList<Guid> SubsidiaryUnitIds { get; set; }
        public IDictionary<string, string> UnitOptions { get; set; }
        public IDictionary<string, string> SubsidiaryOptions { get; set; }
        public GovOfficeAuditAction Action { get; set; }
    }
}
