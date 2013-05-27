using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Messages;

namespace NPC.Application.ManageModels.Messages
{
    public class MessageListModel
    {
        public MessageListModel()
        {
            Messages = new List<Message>();
            MessageSearchModel = new MessageSearchModel();
        }
        public IList<Message> Messages { get; set; }
        public MessageSearchModel MessageSearchModel { get; set; }
    }
}
