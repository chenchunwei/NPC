using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.ManageModels.NpcMmses;
using NPC.Domain.Models.NpcMmsSends;

namespace NPC.Application.ManageModels.NpcMmsSends
{
    public class NpcMmsSendListModel
    {
        public NpcMmsSendListModel()
        {
            NpcMmsSends = new List<NpcMmsSend>();
            NpcMmsSendSearchModel = new NpcMmsSendSearchModel();
        }
        public IList<NpcMmsSend> NpcMmsSends { get; set; }
        public NpcMmsSendSearchModel NpcMmsSendSearchModel { get; set; }
    }
}
