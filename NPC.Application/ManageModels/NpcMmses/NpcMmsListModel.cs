using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Application.ManageModels.NpcMmses
{
    public class NpcMmsListModel
    {
        public NpcMmsListModel()
        {
            NpcMmses = new List<NpcMms>();
            NpcMmsSearchModel = new NpcMmsSearchModel();
        }
        public IList<NpcMms> NpcMmses { get; set; }
        public NpcMmsSearchModel NpcMmsSearchModel { get; set; }
    }
}
