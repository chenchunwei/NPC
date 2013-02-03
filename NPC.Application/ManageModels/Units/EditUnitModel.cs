using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.Units
{
    public class EditUnitModel
    {
        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }
        public EditUnitModelFormData FormData { get; set; }
    }

    public class EditUnitModelFormData
    {
        public string Name { get; set; }
        public bool IsWebUnit { get; set; }
        public bool IsFlowUnit { get; set; }
    }
}
