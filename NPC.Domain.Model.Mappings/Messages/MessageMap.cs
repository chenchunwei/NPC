using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Messages;
using FluentNHibernate.Mapping;

namespace NPC.Domain.Model.Mappings.Messages
{
    public class MessageMap: ClassMap<Message>
    {
        public MessageMap()
       {
           Id(o => o.Id).GeneratedBy.GuidComb();
           Map(o => o.Title);
           Map(o => o.Tel);
           Map(o => o.MessageContent);
           Component(o => o.RecordDescription);
       }
    }
}
