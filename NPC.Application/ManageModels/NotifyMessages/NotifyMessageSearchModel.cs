using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NotifyMessages;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Application.ManageModels.NotifyMessages
{
    public class NotifyMessageSearchModel
    {
        public NotifyMessageSearchModel()
        {
            NotifyMessageQueryItem = new NotifyMessageQueryItem();
        }
        public NotifyMessageQueryItem NotifyMessageQueryItem { get; set; }
    }
}
