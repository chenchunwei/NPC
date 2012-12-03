using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
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
            var trans = TransactionManager.BeginTransaction();
            var flowType = new FlowType();
            flowType.Name = "ProposalFlow";

            FlowTypeRepository.Save(flowType);
            //第一个节点
            var firstNode = new FlowNode()
            {
                Name = "审批",
                ExecutorType = FlowValueType.ByDataField,
                ExecutorValue = "Auditor",
                IsExecutorWithArray = true,
                IsFirstNode = true,
                ProcessUrl = "urlOfProcess"
            };
            //添加流程变量声明
            flowType.FlowTypeDataFields.Add(new FlowTypeDataField() { Name = "Auditor" });
            flowType.FlowTypeDataFields.Add(new FlowTypeDataField() { Name = "IsNeedNextAudit" });
            //第一个节点的Actions
            firstNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "办结" });
            firstNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "返还" });
            firstNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "下一轮审批" });
            firstNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "否决" });
            //第二个节点
            var endAudit = new FlowNode()
            {
                Name = "完结",
                IsServerNode = true
            };
            flowType.FlowNodes.Add(endAudit);
            flowType.FlowNodes.Add(firstNode);
            //添加流程分支
            firstNode.FlowNodeLines.Add(new FlowNodeLine() { RuleCode = "办结", Name = "办结", ContactTo = endAudit });
            firstNode.FlowNodeLines.Add(new FlowNodeLine() { RuleCode = "下一轮审批", Name = "下一轮审批", ContactTo = firstNode });
            FlowTypeRepository.Save(flowType);
            trans.Commit();
        }
    }
}
