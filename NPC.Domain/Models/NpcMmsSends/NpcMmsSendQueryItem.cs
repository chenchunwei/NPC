using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.NpcMmsSends
{
    public class NpcMmsSendQueryItem : QueryItemBase
    {
        public NpcMmsSendQueryItem()
        {
            Pagination = new Pagination();
        }
        public string Title { get; set; }
        public Guid? UnitId { get; set; }
    }
}
