using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;

namespace NPC.Domain.Models.NpcMmses
{
    public class NpcMmsContent
    {
        public virtual Guid Id { get; set; }
        public virtual string Content { get; set; }
        public virtual string UrlOfVoice { get; set; }
        public virtual string UrlOfPic { get; set; }
        public virtual int DueTime { get; set; }
        public virtual LayoutType LayoutType { get; set; }
        public virtual int ByteSize { get; set; }
        public virtual int OrderSort { get; set; }
    }
}
