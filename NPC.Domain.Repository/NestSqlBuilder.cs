using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Repository
{
    public class NestSqlBuilder
    {
        public string BuilderRecord(string sql, string order)
        {
            return string.Format("Select * from ({0}) as T {1}", sql, order);
        }

        public string BuilderFunction(string fun, string sql, string order)
        {
            return string.Format("Select {0} from ({1}) as T {2}", fun, sql, order);
        }
    }
}
