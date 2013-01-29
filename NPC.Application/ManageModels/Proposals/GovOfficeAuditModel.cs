using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Flows;

namespace NPC.Application.ManageModels.Proposals
{
    public class GovOfficeAuditModel
    {
        public GovOfficeAuditModel()
        {
            SubsidiaryUnitIds=new List<Guid>();
        }
        public Guid Id { get; set; }
        public Flow Flow { get; set; }
        public string Comment { get; set; }
        public Guid? UnitId { get; set; }
        public IList<Guid> SubsidiaryUnitIds { get; set; }
        public GovOfficeAuditAction  Action { get; set; }
    }
}
