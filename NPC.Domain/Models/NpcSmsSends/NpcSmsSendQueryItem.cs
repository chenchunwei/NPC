using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models;

namespace NPC.Domain.Models.NpcSmsSends
{
   public class NpcSmsSendQueryItem: QueryItemBase
    {
       public NpcSmsSendQueryItem()
        {
            Pagination = new Pagination();
        }
        public string Title { get; set; }
        public Guid? UnitId { get; set; }
    }
}

