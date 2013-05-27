using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Messages;

namespace NPC.Application.ManageModels.Messages
{
    public class MessageSearchModel
    {
        public MessageSearchModel()
        {
            MessageQueryItem = new MessageQueryItem();
        }
        public MessageQueryItem MessageQueryItem { get; set; }
    }
}
