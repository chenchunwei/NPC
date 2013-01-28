using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.ClientNodeInstances;
using NPC.Domain.Models.FlowNodeInstances;

namespace NPC.Domain.Repository
{
    public class FlowNodeInstanceRepository : AbstractNhibernateRepository<Guid, FlowNodeInstance>
    {
        public IList<FlowNodeInstance> GetUnDeals()
        {
            return Session.CreateQuery("from FlowNodeInstance where InstanceStatus in(:InstanceStatus) and RecordDescription.IsDelete=0")
                    .SetParameterList("InstanceStatus", new object[] { InstanceStatus.ActionCompleted })
                    .List<FlowNodeInstance>();
        }

        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        public IList<FlowNodeInstance> Query(FlowNodeInstanceQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct fni.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct fni.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(FlowNodeInstance)).List<FlowNodeInstance>();
        }

        private static Tuple<string, Hashtable> FormatQuery(FlowNodeInstanceQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From FlowNodeInstances fni ");
            stringBuilder.Append("left join FlowNodeInstanceUserStates fniu on fniu.FlowNodeInstanceId=fni.Id ");
            stringBuilder.Append("left join Flows f on fni.BelongsFlowId = f.Id ");
            stringBuilder.Append("left join FlowNodes fn on fni.BelongsFlowNodeId= fn.Id ");
            stringBuilder.Append("left join FlowTypes ft on f.FlowTypeId= ft.Id ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.FlowTypeName))
            {
                stringBuilder.Append("And ft.Name = :FlowTypeName ");
                parameters.Add("FlowTypeName", queryItem.FlowTypeName);
            }
            if (queryItem.UserId.HasValue)
            {
                stringBuilder.Append("And fniu.UserId = :UserId ");
                parameters.Add("UserId", queryItem.UserId.Value);
            }
            stringBuilder.Append("And fni.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
    }
}
