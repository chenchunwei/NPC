using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Common;

namespace NPC.Domain.Model.Mappings.Common
{
    public class RecordDescriptionMap : ComponentMap<RecordDescription>
    {
        public RecordDescriptionMap()
        {
            Map(o => o.DateOfCreate);
            Map(o => o.DateOfLastestModify);
            References(o => o.UserOfCreate).Column("UserIdOfCreate");
            References(o => o.UserOfLasetestModify).Column("UserIdOfLasetestModify");
            Map(o => o.IsDelete);
        }
    }
}
