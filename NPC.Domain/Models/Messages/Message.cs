using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;
using Fluent.Infrastructure.Domain;

namespace NPC.Domain.Models.Messages
{
    public class Message : IAggregateRoot
    {
        public Message()
        {
            RecordDescription = new RecordDescription();
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Tel { get; set; }
        public string MessageContent { get; set; }
        public RecordDescription RecordDescription { get; set; }
    }
}
