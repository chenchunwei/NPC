using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.FlowNodeInstances;
using NPC.Domain.Models.Proposals;

namespace NPC.Domain.Repository
{
    public class FlowNodeInstanceTaskRepository : AbstractNhibernateRepository<Guid, FlowNodeInstanceTask>
    {
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        public IList<FlowNodeInstanceTask> Query(FlowNodeInstanceTaskQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct fnit.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct fnit.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(FlowNodeInstanceTask)).List<FlowNodeInstanceTask>();
        }

        private static Tuple<string, Hashtable> FormatQuery(FlowNodeInstanceTaskQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From FlowNodeInstanceTasks fnit ");
            stringBuilder.Append("left join FlowNodeInstances fni on fnit.FlowNodeInstanceId=fni.Id ");
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
                stringBuilder.Append("And fnit.UserId = :UserId ");
                parameters.Add("UserId", queryItem.UserId.Value);
            }
            if (!string.IsNullOrEmpty(queryItem.NodeName))
            {
                stringBuilder.Append("And fn.Name = :NodeName ");
                parameters.Add("NodeName", queryItem.NodeName);
            }
            var status = new List<TaskStatus>();
            status.Add(TaskStatus.Created);
            status.Add(TaskStatus.Executed);
            status.Add(TaskStatus.Executing);
            status.Add(TaskStatus.Ignore);

            var matchStatus = new List<TaskStatus>();
            if (queryItem.TaskStatus.HasValue)
            {
                status.ForEach(o =>
                {
                    if ((o & queryItem.TaskStatus.Value) > 0)
                    {
                        matchStatus.Add(o);
                    }
                });
                stringBuilder.Append("And fnit.TaskStatus in (:TaskStatus) ");
                parameters.Add("TaskStatus", matchStatus);
            }
            stringBuilder.Append("And fnit.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
    }
}
