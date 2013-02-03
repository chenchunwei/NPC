using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Flows;

namespace NPC.Application.ManageModels.Flows
{
    public class FlowBasicModel
    {
        public Guid FlowId { get; set; }
        public Guid TaskId { get; set; }
        public Flow Flow { get; set; }
    }
}
