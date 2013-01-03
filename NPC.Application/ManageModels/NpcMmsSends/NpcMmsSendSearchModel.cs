using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NpcMmsSends;

namespace NPC.Application.ManageModels.NpcMmsSends
{
    public class NpcMmsSendSearchModel
    {
        public NpcMmsSendSearchModel()
        {
            NpcMmsSendQueryItem = new NpcMmsSendQueryItem();
        }
        public NpcMmsSendQueryItem NpcMmsSendQueryItem { get; set; }
    }
}
