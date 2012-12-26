using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.NpcMmses
{
    public class EditNpcMmsModel
    {
        public EditNpcMmsModel()
        {
            FormData = new EditNpcMmsModelFormData();
        }

        public int ByteSize { get; set; }
        public int FrameCount { get; set; }
        public EditNpcMmsModelFormData FormData { get; set; }
    }

    public class EditNpcMmsModelFormData
    {
        public EditNpcMmsModelFormData()
        {
            TelNums = new List<string>();
        }
        public string Title { get; set; }
        public IList<string> TelNums { get; set; }
        public Guid? NpcMmsDraftId { get; set; }
        public DateTime? TimeOfExceptSend { get; set; }
    }
}
