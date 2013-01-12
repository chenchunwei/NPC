using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.NodeRecords
{
    public class EditNodeRecordModelFormData
    {
        public string FirstTitle { get; set; }
        public string FirstContent { get; set; }
        public string SecondTitle { get; set; }
        public string SecondContent { get; set; }
        public string FirstImage { get; set; }
        public string SecondImage { get; set; }
        public string RecordLink { get; set; }
        public Guid? SelectedNodeId { get; set; }
        public int OrderSort { get; set; }
        public bool IsShow { get; set; }
    }
}
