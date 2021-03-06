﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Repository
{
    public class UnitRepository : AbstractNhibernateRepository<Guid, Unit>
    {
        public IEnumerable<Unit> GetSubUnit(Guid id)
        {
            return Session.CreateQuery("from Unit Where ParentUint.Id=:id And RecordDescription.IsDelete=0")
                .SetGuid("id", id).List<Unit>();
        }
        public IEnumerable<Unit> GetRootUnit()
        {
            return Session.CreateQuery("from Unit Where ParentUint is Null And RecordDescription.IsDelete=0")
               .List<Unit>();
        }

        public IEnumerable<Unit> GetUnits(params Guid[] unitIds)
        {
            return Session.CreateQuery("from Unit Where Id in (:ids) and RecordDescription.IsDelete=0")
                .SetParameterList("ids", unitIds)
                .List<Unit>();
        }
        public IEnumerable<Unit> GetAllUnits()
        {
            return Session.CreateQuery("from Unit Where RecordDescription.IsDelete=0")
                .List<Unit>();
        }

        public IEnumerable<Unit> GetFlowUnits()
        {
            return Session.CreateQuery("from Unit Where RecordDescription.IsDelete=0 And IsFlowUint=1 And JieKouRen is not Null")
                .List<Unit>();
        }
    }
}
