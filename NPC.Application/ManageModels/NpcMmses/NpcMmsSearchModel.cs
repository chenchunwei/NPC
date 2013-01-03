using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Application.ManageModels.NpcMmses
{
    public class NpcMmsSearchModel
    {
        public NpcMmsSearchModel()
        {
            NpcMmsQueryItem = new NpcMmsQueryItem();
        }
        public NpcMmsQueryItem NpcMmsQueryItem { get; set; }
    }
}
