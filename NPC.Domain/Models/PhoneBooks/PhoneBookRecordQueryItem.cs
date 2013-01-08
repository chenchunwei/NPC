using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.PhoneBooks
{
    public class PhoneBookRecordQueryItem: QueryItemBase
    {
        public PhoneBookRecordQueryItem()
        {
            Pagination=new Pagination();
        }
        public Guid? PhoneBookId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public Guid? UnitId { get; set; }
    }
}
