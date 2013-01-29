using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Flows;

namespace NPC.Application.ManageModels.Proposals
{
    public class ScNpcAuditModel
    {
        public Guid Id { get; set; }
        public Flow Flow { get; set; }
        public string Comment { get; set; }
        public ScNpcAuditAction Action { get; set; }
    }
}
