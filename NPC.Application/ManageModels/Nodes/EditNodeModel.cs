using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.Nodes
{
    public class EditNodeModel
    {
        public Guid? Id { get; set; }
        public EditNodeModelFormData FormData { get; set; }
    }

    public class EditNodeModelFormData
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? IsRecordNode { get; set; }
    }
}
