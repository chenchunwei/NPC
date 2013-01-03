using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models;

namespace NPC.Domain.Models.NpcMmses
{
    public class NpcMmsQueryItem : QueryItemBase
    {
        public NpcMmsQueryItem()
        {
            Pagination = new Pagination();
        }
        public string Title { get; set; }
        public Guid? UnitId { get; set; }
    }
}
