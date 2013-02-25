using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NPC.Application.ManageModels.PhoneBooks
{
    public class SelectePhoneBookRecordModel
    {
        public SelectePhoneBookRecordModel()
        {
            Ids = new List<Guid>();
            Where = new SelectePhoneBookRecordModelWhere();
        }

        [DataMember(Name = "checkedAllPage")]
        public bool CheckedAllPage { get; set; }

        [DataMember(Name = "ids")]
        public IList<Guid> Ids { get; set; }

        [DataMember(Name = "whereOptions")]
        public SelectePhoneBookRecordModelWhere Where { get; set; }

        public Guid? UnitId { get; set; }
    }

    [DataContract]
    public class SelectePhoneBookRecordModelWhere
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "phoneBookId")]
        public Guid? PhoneBookId { get; set; }

        [DataMember(Name = "mobile")]
        public string Mobile { get; set; }
    }
}
