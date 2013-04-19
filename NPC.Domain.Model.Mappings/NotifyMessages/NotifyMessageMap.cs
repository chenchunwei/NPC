using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.NotifyMessages;

namespace NPC.Domain.Model.Mappings.NotifyMessages
{
    public class NotifyMessageMap : ClassMap<NotifyMessage>
    {
        public NotifyMessageMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.ApplicationId);
            Map(o => o.Content);
            Map(o => o.ExtendCode);
            Map(o => o.From);
            Map(o => o.MessageId);
            Map(o => o.MessageType).CustomType<MessageType>();
            Map(o => o.ReceivedTime);
            Map(o => o.To);
            Component(o => o.RecordDescription);
        }
    }
}
