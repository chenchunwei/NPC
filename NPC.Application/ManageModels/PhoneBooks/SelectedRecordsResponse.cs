using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NPC.Application.ManageModels.PhoneBooks
{
    [DataContract]
    public class SelectedRecordsResponse
    {
        public SelectedRecordsResponse()
        {
            Telnumbers = new List<string>();
        }
        [DataMember(Name = "tels")]
        public IList<string> Telnumbers { get; set; }
        [DataMember(Name = "totalCount")]
        public int TotalCount { get; set; }
    }
}
