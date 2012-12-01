using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.Tasks;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;

namespace NPC.FlowEngine
{
    public class ClientNodeInstanceService
    {
        private readonly ClientNodeInstanceRepository _clientNodeInstanceRepository;
        private readonly TaskRepository _taskRepository;
        public ClientNodeInstanceService()
        {
            _clientNodeInstanceRepository = new ClientNodeInstanceRepository();
            _taskRepository = new TaskRepository();
        }

        public void ExecuteClientNodeInstance(Guid taskId, string actionName, User executor, string comment,
           Dictionary<string, string> args = null)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var task = _taskRepository.Find(taskId);
                task.Done(executor, TaskStatus.Finished);

                var clientNodeInstance = _clientNodeInstanceRepository.Find(Guid.Parse(task.Body));
                if (clientNodeInstance == null)
                    throw new ArgumentException("该任务未找到对应的流程节点对象");
                var action = clientNodeInstance.BelongsClientNode.ClientNodeActions.Single(o => o.Name == actionName);
                clientNodeInstance.Execute(executor, action);

                _taskRepository.Save(task);
                _clientNodeInstanceRepository.Save(clientNodeInstance);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
           
        }
    }
}
