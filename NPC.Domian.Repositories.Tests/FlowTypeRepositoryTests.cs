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
            //添加流程变量声明
            flowType.FlowTypeDataFields.Add(new FlowTypeDataField() { Name = "Auditor" });
            FlowTypeRepository.Save(flowType);

            //第一个节点
            var firstNode = new FlowNode()
            {
                Name = "人大常委会审核",
                ExecutorType = FlowValueType.ByDataField,
                ExecutorValue = "NpcAuditor",
                IsExecutorWithArray = true,
                IsFirstNode = true,
                ProcessUrl = "/Proposals/ScNpcAudit"
            };
           
            //第一个节点的Actions
            firstNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "提交市政办" });
            firstNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "不提交" });
            //第二个节点
            var secordNode = new FlowNode()
            {
                Name = "市政办审核",
                ExecutorType = FlowValueType.ByDataField,
                ExecutorValue = "GovAuditor",
                IsExecutorWithArray = true,
                ProcessUrl = "/Proposals/GovOfficeAudit"
            };

            secordNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "提交主办单位" });
            secordNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "退回人大常委" });

            //第三个节点
            var thirdNode = new FlowNode()
            {
                Name = "主办单位处理",
                ExecutorType = FlowValueType.ByDataField,
                ExecutorValue = "Sponsor",
                IsExecutorWithArray = true,
                ProcessUrl = "/Proposals/SponsorAudit"
            };

            thirdNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "办结" });
            thirdNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "退回市政办" });

            //第四个节点
            var fourthNode = new FlowNode()
            {
                Name = "代表满意度回馈",
                ExecutorType = FlowValueType.ByDataField,
                ExecutorValue = "Originator",
                IsExecutorWithArray = true,
                ProcessUrl = "/Proposals/NpcAssessment"
            };

            fourthNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "不满意" });
            fourthNode.FlowNodeActions.Add(new FlowNodeAction() { Name = "满意" });
            
            //添加流程分支
            firstNode.FlowNodeLines.Add(new FlowNodeLine() { RuleCode = "提交", Name = "提交", ContactTo = secordNode });
            firstNode.FlowNodeLines.Add(new FlowNodeLine() { RuleCode = "不提交", Name = "不提交", ContactTo = null });

            secordNode.FlowNodeLines.Add(new FlowNodeLine() { RuleCode = "提交主办单位", Name = "提交主办单位", ContactTo = thirdNode });
            secordNode.FlowNodeLines.Add(new FlowNodeLine() { RuleCode = "退回人大常委", Name = "退回人大常委", ContactTo = firstNode });

            thirdNode.FlowNodeLines.Add(new FlowNodeLine() { RuleCode = "办结", Name = "办结", ContactTo = fourthNode });
            thirdNode.FlowNodeLines.Add(new FlowNodeLine() { RuleCode = "退回市政办", Name = "退回市政办", ContactTo = secordNode });

            fourthNode.FlowNodeLines.Add(new FlowNodeLine() { RuleCode = "不满意", Name = "不满意", ContactTo = thirdNode });
            fourthNode.FlowNodeLines.Add(new FlowNodeLine() { RuleCode = "满意", Name = "满意", ContactTo = null });

            flowType.FlowNodes.Add(firstNode);
            flowType.FlowNodes.Add(secordNode);
            flowType.FlowNodes.Add(thirdNode);
            flowType.FlowNodes.Add(fourthNode);
            
            FlowTypeRepository.Save(flowType);
            trans.Commit();
        }
    }
}
