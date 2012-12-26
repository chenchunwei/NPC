using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Domain.Model.Mappings.NpcMmses
{
    public class NpcMmsMap : ClassMap<NpcMms>
    {
       public NpcMmsMap()
       {
           Id(o => o.Id).GeneratedBy.GuidComb(); 
           Map(o => o.ByteSize);
           HasMany(o => o.NpcMmsReceivers).KeyColumn("NpcMmsId");
           Map(o => o.TimeOfExceptSend);
           Component(o => o.RecordDescription);
           Map(o => o.Title);
           Table("NpcMmses");
       }
    }
}
