using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.PhoneBooks
{
    public class PhoneBookQueryItem : QueryItemBase
    {
        public PhoneBookQueryItem()
        {
            Pagination = new Pagination();
        }
        public Guid? PhoneBookId { get; set; }
        public string Name { get; set; }
        public Guid? UnitId { get; set; }
    }
}
