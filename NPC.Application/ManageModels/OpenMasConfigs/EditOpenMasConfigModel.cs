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
        public string MmsMasService { get; set; }
        public string MmsAppPwd { get; set; }
        public string MmsAppAccount { get; set; }
        public string MmsExtensionNo { get; set; }
        public string SmsMasService { get; set; }
        public string SmsAppPwd { get; set; }
        public string SmsAppAccount { get; set; }
        public string SmsExtensionNo { get; set; }
        public string Signature { get; set; }
        public Unit Unit { get; set; }
    }
}
