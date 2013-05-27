using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Messages
{
    public class MessageQueryItem : QueryItemBase
    {
        public Guid? UnitId { get; set; }
        public string Title { get; set; }
        public string MessageContent { get; set; }
    }
}
