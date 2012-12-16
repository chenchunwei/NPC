using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Nodes;

namespace NPC.Domain.Repository
{
    public class NodeRepository : AbstractNhibernateRepository<Guid, Node>
    {
        public IEnumerable<Node> GetSubs(Guid id)
        {
            return Session.CreateQuery("from Node Where  RecordDescription.IsDelete=0 and ParentNode.Id=:id")
                 .SetGuid("id", id).List<Node>();
        }
        public IEnumerable<Node> GetRootNodes()
        {
            return Session.CreateQuery("from Node Where  RecordDescription.IsDelete=0 and ParentNode is Null")
                 .List<Node>();
        }
        public IEnumerable<Node> GetAllNodes()
        {
            return Session.CreateQuery("from Node Where  RecordDescription.IsDelete=0 ")
                  .List<Node>();
        }

        public bool IsNodeCodeRepeat(string code)
        {
            return Session.CreateQuery("select count(*) from Node Where  RecordDescription.IsDelete=0 and Code=:Code")
                .SetString("Code", code.Trim())
                .UniqueResult<long>() > 0;
        }

        public Node GetSingleByCode(string code)
        {
            return Session.CreateQuery("from Node Where  RecordDescription.IsDelete=0 and Code=:Code")
                .SetString("Code", code.Trim())
                .UniqueResult<Node>();
        }

        public bool IsNodeCodeRepeatInUnit(Guid unitId, string code)
        {
            return Session.CreateQuery("select count(*) from Node Where  RecordDescription.IsDelete=0 and Code=:Code and Unit.Id= :UnitId")
                .SetGuid("UnitId", unitId)
                .SetString("Code", code.Trim())
                .UniqueResult<long>() > 0;
        }

        public IEnumerable<Node> GetRootNodesInUnit(Guid unitId)
        {
            return Session.CreateQuery("from Node Where  RecordDescription.IsDelete=0 and ParentNode is Null and Unit.Id=:UnitId")
                .SetGuid("UnitId", unitId) 
                .List<Node>();
        }

        public IEnumerable<Node> GetSubsInUnit(Guid unitId,Guid id)
        {
            return Session.CreateQuery("from Node Where  RecordDescription.IsDelete=0 and ParentNode.Id=:id and Unit.Id=:UnitId")
                 .SetGuid("id", id)
                 .SetGuid("UnitId", unitId)
                 .List<Node>();
        }
    }
}
