using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Nodes;
using NPC.Application.ManageModels.NodeRecords;

namespace NPC.Application.ManageModels.NodeRecords
{
    public class EditNodeRecordModel
    {
        public EditNodeRecordModel()
        {
            FormData = new EditNodeRecordModelFormData();
            FormData.IsShow = true;
        }
        public Guid? NodeId { get; set; }
        public Node Node { get; set; }
        public Guid? Id { get; set; }
        public EditNodeRecordModelFormData FormData { get; set; }
    }
}
