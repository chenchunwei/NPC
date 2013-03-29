using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using Fluent.Infrastructure.Web.HttpMoudles;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.PhoneBooks;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.NpcSmses
{
    public class NpcSms : IAggregateRoot
    {
        public NpcSms()
        {
            RecordDescription = new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual string Content { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual SmsType SmsType { get; set; }
        public virtual bool IsNeedSignature { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual SignatureType SignatureType { get; set; }
    }
}
