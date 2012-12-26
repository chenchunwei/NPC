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
        }
    }
}
