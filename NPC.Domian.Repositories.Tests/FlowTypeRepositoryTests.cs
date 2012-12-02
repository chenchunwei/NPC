using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Repository;

namespace NPC.Domian.Repositories.Tests
{
    [TestClass]
    public class FlowTypeRepositoryTests
    {
        protected FlowTypeRepository FlowTypeRepository { get; set; }

        public FlowTypeRepositoryTests()
        {
            FlowTypeRepository = new FlowTypeRepository();
        }
        [TestMethod]
        public void TestDefineFlow()
        {
            var flowType = new FlowType();
            flowType.Name = "ProposalFlow";
            //第一个节点
            var firstClient = new FlowNode()
            {
                Name = "审批",
                ExecutorType = FlowValueType.ByDataField,
                ExecutorValue = "Auditor",
                IsExecutorWithArray = true,
                IsFirstNode = true,
                ProcessUrl = "urlOfProcess"
            };
            //第一个节点的Actions
            firstClient.FlowNodeActions.Add(new FlowNodeAction() { Name = "办结" });
            firstClient.FlowNodeActions.Add(new FlowNodeAction() { Name = "返还" });
            firstClient.FlowNodeActions.Add(new FlowNodeAction() { Name = "下一轮审批" });
            firstClient.FlowNodeActions.Add(new FlowNodeAction() { Name = "否决" });
            //第二个节点
            var auditFlowNode = new FlowNode()
            {
                Name="完结",
                //ExecutorType=Flo
            };
            flowType.FlowNodes.Add(firstClient);
        }
    }
}
