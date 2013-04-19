using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NotifyMessages;

namespace NPC.Application.ManageModels.NotifyMessages
{
    public class NotifyMessageListModel
    {
        public NotifyMessageListModel()
        {
            NotifyMessages = new List<NotifyMessage>();
            NotifyMessageSearchModel = new NotifyMessageSearchModel();
        }
        public IList<NotifyMessage> NotifyMessages { get; set; }
        public NotifyMessageSearchModel NotifyMessageSearchModel { get; set; }
    }
}
