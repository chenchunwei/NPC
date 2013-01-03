using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.OpenMasConfigs;
using NPC.Domain.Models.Units;

namespace NPC.Application.ManageModels.OpenMasConfigs
{
    public class EditOpenMasConfigModel
    {
        public string MasService { get; set; }
        public string AppPwd { get; set; }
        public string AppAccount { get; set; }
        public string ExtensionNo { get; set; }
        public Unit Unit { get; set; }
    }
}
