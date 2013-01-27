using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.Departments
{
    public class EditDepartmentModel
    {
        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }
        public EditDepartmentModelFormData FormData { get; set; }
    }

    public class EditDepartmentModelFormData
    {
        public string Name { get; set; }
    }
}
