using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.NpcMmses
{
    public class NpcMms : IAggregateRoot
    {
        public NpcMms()
        {
            NpcMmsContents=new List<NpcMmsContent>();
            RecordDescription = new RecordDescription();
        }

        public virtual IList<NpcMmsContent> NpcMmsContents { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
        public virtual int ByteSize { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual LayoutType LayoutType { get; set; }

    }
}
