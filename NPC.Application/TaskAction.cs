using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Application.ManageModels.Tasks;
using NPC.Domain.Models.Tasks;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class TaskAction
    {
        private readonly TaskRepository _taskRepository;
        public TaskAction()
        {
            _taskRepository = new TaskRepository();
        }

        public MyTasksModel InitializeMyTasksModel(TaskQueryItem taskQueryItem)
        {
            var model = new MyTasksModel();
            model.Tasks = _taskRepository.Query(taskQueryItem);
            model.TaskQueryItem = taskQueryItem;
            return model;
        }
    }
}
