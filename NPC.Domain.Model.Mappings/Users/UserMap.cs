using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Model.Mappings.Users
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.Pwd);
            Map(o => o.Account);
            Map(o => o.QQ);
            References(o => o.Unit).Column("UnitId");
            References(o => o.Department).Column("DepartmentId");
            References(o => o.PhoneBookRecord).Column("PhoneBookRecordId");
            Component(o => o.RecordDescription);
            HasMany(o => o.Agents).KeyColumn("UserId");
            Table("Users");
        }
    }
}
