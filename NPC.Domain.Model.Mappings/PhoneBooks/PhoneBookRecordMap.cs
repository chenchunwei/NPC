using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.PhoneBooks;

namespace NPC.Domain.Model.Mappings.PhoneBooks
{
    public class PhoneBookRecordMap : ClassMap<PhoneBookRecord>
    {
        public PhoneBookRecordMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Mobile);
            Map(o => o.Name);
            References(o => o.PhoneBook).Column("PhoneBookId");
            References(o => o.User).Column("UserId");
            Component(o => o.RecordDescription);
            Table("PhoneBookRecords");
        }
    }
}
