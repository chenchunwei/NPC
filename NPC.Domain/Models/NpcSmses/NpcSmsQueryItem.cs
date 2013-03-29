using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.NpcSmses
{
    public class NpcSmsQueryItem : QueryItemBase
    {
        public NpcSmsQueryItem()
        {
            Pagination = new Pagination();
        }
        public string Title { get; set; }
        public Guid? UnitId { get; set; }
    }
}

