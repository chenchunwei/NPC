using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.NodeRecords;

namespace NPC.Domain.Repository
{
    public class NodeRecordRepository : AbstractNhibernateRepository<Guid, NodeRecord>
    {
        #region 分页查询
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        public IList<NodeRecord> Query(NodeRecordQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct nr.*", ""), "Order by OrderSort desc, DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct nr.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(NodeRecord)).List<NodeRecord>();
        }

        private static Tuple<string, Hashtable> FormatQuery(NodeRecordQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From NodeRecords nr ");
            stringBuilder.Append("join Nodes n on n.Id= nr.BelongsToNodeId ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.Keyword))
            {
                stringBuilder.Append("And nr.FirstTitle like :Keyword ");
                parameters.Add("Keyword", "%" + queryItem.Keyword + "%");
            }
            if (queryItem.IsShow.HasValue)
            {
                stringBuilder.Append("And nr.IsShow =:IsShow ");
                parameters.Add("IsShow", queryItem.IsShow.Value);
            }
            if (queryItem.NodeId.HasValue)
            {
                stringBuilder.Append("And nr.BelongsToNodeId = :BelongsToNodeId ");
                parameters.Add("BelongsToNodeId", queryItem.NodeId);
            }
            if (queryItem.UnitId.HasValue)
            {
                stringBuilder.Append("And n.UnitId = :UnitId ");
                parameters.Add("UnitId", queryItem.UnitId.Value);
            }
            stringBuilder.Append("And nr.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
        #endregion

        #region top n
        public IList<NodeRecord> GetTopN(Guid unitId, Guid nodeId, int topN)
        {
            return Session.CreateSQLQuery(
                    string.Format(@"select top {0} * from NodeRecords nr 
                        where nr.BelongsToNodeId=:BelongsToNodeId and IsDelete =0 and  nr.IsShow=1 and n.UnitId=:UnitId Order by OrderSort desc,DateOfCreate desc", topN))
                       .AddEntity(typeof(NodeRecord))
                       .SetGuid("UnitId", unitId)
                       .SetGuid("BelongsToNodeId", nodeId)
                       .List<NodeRecord>();
        }
        #endregion

        #region top n
        public IList<NodeRecord> GetTopN(Guid unitId, string code, int topN)
        {
            return Session.CreateSQLQuery(
                    string.Format(@"select top {0} * from NodeRecords nr join Nodes n on n.Id=nr.BelongsToNodeId 
                        where n.Code=:code and nr.IsDelete =0 and  nr.IsShow=1 and n.UnitId=:UnitId Order by nr.OrderSort desc,nr.DateOfCreate desc", topN))
                       .AddEntity(typeof(NodeRecord))
                       .SetGuid("UnitId", unitId)
                       .SetString("code", code)
                       .List<NodeRecord>();
        }
        #endregion
    }
}
