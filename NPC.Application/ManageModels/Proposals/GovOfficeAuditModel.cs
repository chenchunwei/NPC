using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Proposals;

namespace NPC.Application.ManageModels.Proposals
{
    public class GovOfficeAuditModel
    {
        public GovOfficeAuditModel()
        {
            SubsidiaryUnitIds = new List<Guid>();
            UnitOptions = new Dictionary<string, string>();
        }
        public Guid TaskId { get; set; }
        public Flow Flow { get; set; }
        public string Comment { get; set; }
        public Guid? SponsorUnitId { get; set; }
        public Proposal Proposal { get; set; }
        public IList<Guid> SubsidiaryUnitIds { get; set; }
        public IDictionary<string, string> UnitOptions { get; set; }
        public GovOfficeAuditAction Action { get; set; }
    }
}
