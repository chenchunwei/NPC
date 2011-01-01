using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Units;

namespace NPC.Application.ManageModels.Units
{
    public class UnitFlowSettingsModel
    {
        public Guid Id { get; set; }
        public Unit Unit { get; set; }
        public Guid? GovUnitId { get; set; }
        public Guid? NpcUnitId { get; set; }
        public string SponsorUnitIdString { get; set; }
        public string SubsidiaryUnitString { get; set; }
    }
}
