using FluentNHibernate.Mapping;
using NPC.Domain.Models.PhoneBooks;

namespace NPC.Domain.Model.Mappings.PhoneBooks
{
    public class PhoneBookMap : ClassMap<PhoneBook>
    {
        public PhoneBookMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.PhoneBookType).CustomType<PhoneBookType>();
            References(o => o.Unit).Column("UnitId");
            Component(o => o.RecordDescription);
            References(o => o.User).Column("UserId");
            Map(o => o.IsDefault);
            Table("PhoneBooks");
        }
    }
}
