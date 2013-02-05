using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Departments;
using NPC.Domain.Models.Users;

namespace NPC.Application.ManageModels.Users
{
    public class InteractiveModel
    {
        public InteractiveModel()
        {
            Departments = new List<Department>();
            DepartmentUsers=new Dictionary<Department, IList<User>>();
        }
        public IList<Department> Departments { get; set; }
        public IDictionary<Department, IList<User>> DepartmentUsers { get; set; }
    }
}
