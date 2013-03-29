using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.OpenMasConfigs
{
    public class OpenMasConfig:IAggregateRoot
    {
        public OpenMasConfig()
        {
            RecordDescription=new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual string MmsMasService { get; set; }
        public virtual string MmsAppPwd { get; set; }
        public virtual string MmsAppAccount { get; set; }
        public virtual string MmsExtensionNo { get; set; }

        public virtual string SmsMasService { get; set; }
        public virtual string SmsAppPwd { get; set; }
        public virtual string SmsAppAccount { get; set; }
        public virtual string SmsExtensionNo { get; set; }

        public virtual string Signature { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
