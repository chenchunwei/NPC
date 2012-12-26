using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Domain.Model.Mappings.NpcMmses
{
    public class NpcMmsContentMap : ClassMap<NpcMmsContent>
    {
        public NpcMmsContentMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.ByteSize);
            Map(o => o.Content);
            Map(o => o.DueTime);
            Map(o => o.LayoutType).CustomType<LayoutType>();
            Map(o => o.OrderSort);
            Map(o => o.UrlOfPic);
            Map(o => o.UrlOfVoice);
            Table("NpcMmsContents");
        }
    }
}
