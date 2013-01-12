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

        public Guid? Id { get; set; }
        public bool IsSend { get; set; }
        public int ByteSize { get; set; }
        public int FrameCount { get; set; }
        public EditNpcMmsModelFormData FormData { get; set; }
        public IList<FrameSerializer> FrameSerializers { get; set; }
    }

    public class EditNpcMmsModelFormData
    {
        public string Title { get; set; }
        public string Frames { get; set; }
        public LayoutType LayoutType { get; set; }
    }
    [DataContract]
    public class FrameSerializer
    {
        [DataMember(Name = "id")]
        public Guid? Id { get; set; }
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
