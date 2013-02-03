using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Units;

namespace NPC.Application.ManageModels.Configs
{
    public class JieKouRenSettingsModel
    {
        public Unit Unit { get; set; }
        public string AliasName { get; set; }
        public Guid? JieKouRenId { get; set; }
    }
}
