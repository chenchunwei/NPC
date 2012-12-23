using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NodeRecords;
using NPC.Domain.Models.Nodes;
using NPC.Domain.Models.Units;

namespace NPC.Application.MainTownModels.Homes
{
    public class IndexModel
    {
        public IndexModel()
        {
            Chairmans = new List<NodeRecord>();
            ViceChairmans = new List<NodeRecord>();
            ChairmanMembers = new List<NodeRecord>();
            News = new List<NodeRecord>();
            Notices = new List<NodeRecord>();
            StudyMaterials = new List<NodeRecord>();
            ReferFiles = new List<NodeRecord>();
            PublicProposals = new List<NodeRecord>();
            DealedProposals = new List<NodeRecord>();
            HalfMonthlyTalkings = new List<NodeRecord>();
            Regulations = new List<NodeRecord>();
            Supervises = new List<NodeRecord>();
            NpcWorks = new List<NodeRecord>();
            BottomPicsRolling = new List<NodeRecord>();
            DownloadOfNpcExes=new List<NodeRecord>();
            WheelBroadcastPicsOfTopLeft=new List<NodeRecord>();
            Members=new List<NodeRecord>();
        }

        public Unit Unit { get; set; }
        public Node ChairmansModuleNode { get; set; }
        public Node ViceChairmanNode { get; set; }
        public Node ChairmanMembersNode { get; set; }
        public Node ChairmansNode { get; set; }

        public IList<NodeRecord> Chairmans { get; set; }
        public IList<NodeRecord> ViceChairmans { get; set; }
        public IList<NodeRecord> ChairmanMembers { get; set; }

        public NodeRecord Video { get; set; }

        public Node NewsNode { get; set; }
        public IList<NodeRecord> News { get; set; }

        public Node NoticesNode { get; set; }
        public IList<NodeRecord> Notices { get; set; }

        public Node StudySectionNode { get; set; }
        public Node StudyMaterialsNode { get; set; }
        public IList<NodeRecord> StudyMaterials { get; set; }

        public Node ReferFilesNode { get; set; }
        public IList<NodeRecord> ReferFiles { get; set; }

        public Node PublicProposalsNode { get; set; }
        public IList<NodeRecord> PublicProposals { get; set; }

        public Node DealedProposalsNode { get; set; }
        public IList<NodeRecord> DealedProposals { get; set; }

        public Node HalfMonthlyTalkingsNode { get; set; }
        public IList<NodeRecord> HalfMonthlyTalkings { get; set; }

        public Node RegulationsNode { get; set; }
        public IList<NodeRecord> Regulations { get; set; }

        public Node NavigationsNode { get; set; }

        public Node SuperviseNode { get; set; }
        public IList<NodeRecord> Supervises { get; set; }

        public Node NpcWorksNode { get; set; }
        public IList<NodeRecord> NpcWorks { get; set; }

        public Node MembersNode { get; set; }
        public IList<NodeRecord> Members { get; set; }

        public Node BottomPicsRollingNode { get; set; }
        public IList<NodeRecord> BottomPicsRolling { get; set; }

        public Node LatestAttentionsNode { get; set; }
        public IList<NodeRecord> LatestAttentions { get; set; }

        public Node DownloadOfNpcExesNode { get; set; }
        public IList<NodeRecord> DownloadOfNpcExes { get; set; }

        public IList<NodeRecord> WheelBroadcastPicsOfTopLeft { get; set; }
    }
}
