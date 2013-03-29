using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NPC.Application.ManageModels.Users
{
    [DataContract]
    public class SelectedUsersModel
    {
        public SelectedUsersModel()
        {
            Ids = new List<Guid>();
            WhereOptions = new SelectedUsersModelWhere();
        }
        [DataMember(Name = "checkedAllPage")]
        public bool CheckedAllPage { get; set; }
        [DataMember(Name = "ids")]
        public IList<Guid> Ids { get; set; }
        [DataMember(Name = "whereOptions")]
        public SelectedUsersModelWhere WhereOptions { get; set; }
        public Guid? UnitId { get; set; }
    }

    [DataContract]
    public class SelectedUsersModelWhere
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "departmentLikeId")]
        public Guid? DepartmentLikeId { get; set; }
    }
}
