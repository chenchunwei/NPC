using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NodeRecords;
using NPC.Domain.Models.Nodes;
using NPC.Domain.Models.Units;

namespace NPC.Application.MianModels.Homes
{
    public class IndexModel
    {
        public IndexModel()
        {
            WheelBroadcastPicsOfTopLeft = new List<NodeRecord>();
            News=new List<NodeRecord>();
            Notices=new List<NodeRecord>();
            Directors=new List<NodeRecord>();
            ViceDirectors=new List<NodeRecord>();
            Members=new List<NodeRecord>();
            LeaderSpeechs=new List<NodeRecord>();
            SuperviseWindow=new List<NodeRecord>();
            NpcWorks=new List<NodeRecord>();
            SelfImprovement=new List<NodeRecord>();
            Basics=new List<NodeRecord>();
            Mediums=new List<NodeRecord>();
            TownPics=new List<NodeRecord>();
            Elections=new List<NodeRecord>();
            Investigates=new List<NodeRecord>();
            NpcPics=new List<NodeRecord>();
            Links = new List<NodeRecord>();
        }

        public Unit Unit { get; set; }
        public Node NewsNode { get; set; }
        public Node NoticesNode { get; set; }
        public Node DirectorsNode { get; set; }
        public Node ViceDirectorsNode { get; set; }
        public Node MembersNode { get; set; }
        public Node LeaderSpeechsNode { get; set; }
        public Node VideoNode { get; set; }
        public Node SuperviseWindowNode { get; set; }
        public Node NpcWorksNode { get; set; }
        public Node SelfImprovementNode { get; set; }
        public Node BasicsNode { get; set; }
        public Node MediumsNode { get; set; }
        public Node TownPicsNode { get; set; }
        public Node ElectionsNode { get; set; }
        public Node InvestigatesNode { get; set; }
        public Node NpcPicsNode { get; set; }
        public Node LinksNode { get; set; }

        public NodeRecord ContributeNode { get; set; }
        /// <summary>
        /// 顶部轮播 
        /// </summary>
        public IList<NodeRecord> WheelBroadcastPicsOfTopLeft { get; set; }
        /// <summary>
        /// 人大要闻
        /// </summary>
        public IList<NodeRecord> News { get; set; }
        /// <summary>
        /// 公告
        /// </summary>
        public IList<NodeRecord> Notices { get; set; }
        /// <summary>
        /// 主任
        /// </summary>
        public IList<NodeRecord> Directors { get; set; }
        /// <summary>
        /// 副主任
        /// </summary>
        public IList<NodeRecord> ViceDirectors { get; set; }
        /// <summary>
        /// 委员
        /// </summary>
        public IList<NodeRecord> Members { get; set; }

        /// <summary>
        /// 领导讲话
        /// </summary>
        public IList<NodeRecord> LeaderSpeechs { get; set; }
        public NodeRecord Video { get; set; }
        /// <summary>
        /// 监督之窗
        /// </summary>
        public IList<NodeRecord> SuperviseWindow { get; set; }
        /// <summary>
        /// 人大工作
        /// </summary>
        public IList<NodeRecord> NpcWorks { get; set; }
        /// <summary>
        /// 身身建设
        /// </summary>
        public IList<NodeRecord> SelfImprovement { get; set; }
        /// <summary>
        /// 基层建设
        /// </summary>
        public IList<NodeRecord> Basics { get; set; }
        /// <summary>
        /// 媒体报道
        /// </summary>
        public IList<NodeRecord> Mediums { get; set; }
        /// <summary>
        /// 乡镇照片
        /// </summary>
        public IList<NodeRecord> TownPics { get; set; }
        /// <summary>
        /// 选举任免
        /// </summary>
        public IList<NodeRecord> Elections { get; set; }
        /// <summary>
        /// 调查研究
        /// </summary>
        public IList<NodeRecord> Investigates { get; set; }
        /// <summary>
        /// 代表风采
        /// </summary>
        public IList<NodeRecord> NpcPics { get; set; }
        /// <summary>
        /// 友情链接  二级
        /// </summary>
        public IList<NodeRecord> Links { get; set; }

        public NodeRecord FirstFullColumn { get; set; }

    }
}
