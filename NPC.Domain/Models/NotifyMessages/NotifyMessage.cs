﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Proposals;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.NotifyMessages
{
    public class NotifyMessage : IAggregateRoot
    {
        public NotifyMessage()
        {
            RecordDescription = new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string ApplicationId { get; set; }
        public virtual string Content { get; set; }
        public virtual string ExtendCode { get; set; }
        public virtual string From { get; set; }
        public virtual DateTime ReceivedTime { get; set; }
        public virtual string To { get; set; }
        public virtual string MessageId { get; set; }
        public virtual MessageType MessageType { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual bool IsDealed { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ProposalType ProposalType { get; set; }
    }

    public enum MessageType
    {
        Sms = 1,
        Mms = 2

    }
}
