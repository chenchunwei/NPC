using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;

namespace NPC.Domian.Repositories.Tests
{
    [TestClass]
    public class FlowTypeRepositoryTest
    {
        protected readonly FlowTypeRepository FlowTypeRepository;
        public FlowTypeRepositoryTest()
        {
            FlowTypeRepository = new FlowTypeRepository();
        }
        [TestMethod]
        public void TestAdd()
        {
            var flowType = new FlowType();
            flowType.Description = "提案建议流程";
            flowType.Name = "提案建议流程";
            flowType.UrlOfDetail = "/Proposals/FlowDetail";
            flowType.RecordDescription.CreateBy(new User(){Id = Guid.NewGuid()});
            FlowTypeRepository.Save(flowType);
        }
    }
}
