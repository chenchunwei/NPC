using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Application.ManageModels.NpcMmses
{
    public class EditNpcMmsModel
    {
        public EditNpcMmsModel()
        {
            FormData = new EditNpcMmsModelFormData();
            FrameSerializers = new List<FrameSerializer>();
        }

        public int ByteSize { get; set; }
        public int FrameCount { get; set; }
        public EditNpcMmsModelFormData FormData { get; set; }
        public IList<FrameSerializer> FrameSerializers { get; set; }
    }

    public class EditNpcMmsModelFormData
    {
        public EditNpcMmsModelFormData()
        {
            TelNums = new List<string>();
        }
        public string Title { get; set; }
        public IList<string> TelNums { get; set; }
        public string Frames { get; set; }
        public Guid? NpcMmsDraftId { get; set; }
        public DateTime? TimeOfExceptSend { get; set; }
        public LayoutType LayoutType { get; set; }
    }
    [DataContract]
    public class FrameSerializer
    {
        [DataMember(Name = "img")]
        public string Image { get; set; }
        [DataMember(Name = "txt")]
        public string Text { get; set; }
        [DataMember(Name = "voice")]
        public string Voice { get; set; }
        [DataMember(Name = "timeDuring")]
        public int TimeDuring { get; set; }
        [DataMember(Name = "orderSort")]
        public int OrderSort { get; set; }
    }
}
