using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Proposals;
using NPC.Domain.Models.Tasks;

namespace NPC.Domain.Repository
{
    public class ProposalRepository : AbstractNhibernateRepository<Guid, Proposal>
    {
        #region 分页查询
        private readonly NestSqlBuilder _nestSqlBuilder = new NestSqlBuilder();

        public IList<Proposal> Query(ProposalQueryItem queryItem)
        {
            var queryReturns = FormatQuery(queryItem);
            var tempString = queryReturns.Item1;
            var parameters = queryReturns.Item2;
            var query = Session.CreateSQLQuery(_nestSqlBuilder.BuilderRecord(string.Format(tempString, "distinct ps.*", ""), "Order by DateOfCreate desc"));
            var queryTotalCount = Session.CreateSQLQuery(string.Format(tempString, "count(Distinct ps.Id)", ""));

            SetParameters(query, parameters);
            SetParameters(queryTotalCount, parameters);

            query.SetFirstResult((queryItem.Pagination.PageIndex - 1) * queryItem.Pagination.PageSize);
            query.SetMaxResults(queryItem.Pagination.PageSize);

            var count = queryTotalCount.UniqueResult<object>();
            queryItem.Pagination.TotalRecordsCount = Int32.Parse(count.ToString());

            return query.AddEntity(typeof(Proposal)).List<Proposal>();
        }

        private static Tuple<string, Hashtable> FormatQuery(ProposalQueryItem queryItem)
        {
            //表区域
            var stringBuilder = new StringBuilder("Select {0} From Proposals ps ");
            stringBuilder.Append("Where 1=1 ");
            var parameters = new Hashtable();

            if (!string.IsNullOrEmpty(queryItem.Title))
            {
                stringBuilder.Append("And ps.Title like :Title ");
                parameters.Add("Title", "%" + queryItem.Title + "%");
            }
            var status = new List<ProposalStatus>();
            status.Add(ProposalStatus.Default);
            status.Add(ProposalStatus.Finished);
            status.Add(ProposalStatus.GovAudit);
            status.Add(ProposalStatus.NpcAssessment);
            status.Add(ProposalStatus.NpcAudit);
            status.Add(ProposalStatus.SponsorAudit);
            status.Add(ProposalStatus.Stop);
            var matchStatus = new List<ProposalStatus>();
            if (queryItem.ProposalStatus.HasValue)
            {
                status.ForEach(o =>
                {
                    if ((o | queryItem.ProposalStatus.Value) > 0)
                    {
                        matchStatus.Add(o);
                    }
                });
                stringBuilder.Append("And ps.ProposalStatus in (:ProposalStatus) ");
                parameters.Add("ProposalStatus", matchStatus);
            }
           
            stringBuilder.Append("And ps.IsDelete=0 ");
            stringBuilder.Append("{1}");
            return new Tuple<string, Hashtable>(stringBuilder.ToString(), parameters);
        }
        #endregion
    }
}
