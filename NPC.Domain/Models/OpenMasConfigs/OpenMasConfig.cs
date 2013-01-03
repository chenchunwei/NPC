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
        public virtual string MasService { get; set; }
        public virtual string AppPwd { get; set; }
        public virtual string AppAccount { get; set; }
        public virtual string ExtensionNo { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
