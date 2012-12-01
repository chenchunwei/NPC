using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using NPC.Domain.Models.ClientNodeInstances;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Tasks;
using NPC.Domain.Repository;
using log4net;

namespace NPC.FlowEngine
{
    public class FlowEngineService
    {
        private readonly ClientNodeInstanceRepository _clientNodeInstanceRepository;
        private readonly FlowRepository _flowRepository;
        private readonly TaskRepository _taskRepository;
        private readonly ILog _logger;
        public FlowEngineService()
        {
            _logger = new DefaultLoggerFactory().GetLogger();
            _clientNodeInstanceRepository = new ClientNodeInstanceRepository();
            _flowRepository = new FlowRepository();
            _taskRepository = new TaskRepository();
        }

        public void CreateClientNodeInstance()
        {
            var instances = _flowRepository.GetInstanceFlow();
            instances.ToList().ForEach(CreateSingleClinentNodeInstance);
        }

        private void CreateSingleClinentNodeInstance(Flow flow)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var clientNode = flow.FlowType.GetFirstNode();
                //创建节点实例
                var clientNodeInstance = new ClientNodeInstance();
                clientNodeInstance.BelongsClientNode = clientNode;
                clientNodeInstance.BelongsFlow = flow;
                //clientNodeInstance.ClientNodeInstanceUserState.Add(null); 任务处理人暂时没有处理
                clientNodeInstance.RecordDescription.CreateBy(flow.RecordDescription.UserOfCreate);
                _clientNodeInstanceRepository.Save(clientNodeInstance);
                flow.FlowStatus = FlowStatus.Start;
                flow.RecordDescription.DateOfLastestModify = DateTime.Now;
                _flowRepository.Save(flow);
                //创建任务
                var task = new Task();
                task.Body = clientNodeInstance.Id.ToString();
                task.Description = "流程任务";
                task.GroupName = TaskConst.FlowTaskGroup;
                task.TypeName = TaskConst.FlowTaskType;
                task.OuterId = clientNodeInstance.Id;
                task.TaskProcessUrl = clientNodeInstance.BelongsClientNode.ProcessUrl;
                task.TaskUserStates.Add(null);
                task.Title = flow.Title;
                _taskRepository.Save(task);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public void DealClientNodeFlowTo()
        {

        }

        private void DealSingleClientNodeFlowTo(ClientNodeInstance clientNodeInstance)
        {
            var clientNode = clientNodeInstance.BelongsClientNode;
            foreach (var clientLine in clientNode.ClientNodeLines)
            {
                //clientLine.RuleCode == "";
            }

        }
    }
}
