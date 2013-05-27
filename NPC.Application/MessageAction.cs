using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Repository;
using NPC.Application.ManageModels.Messages;
using NPC.Domain.Models.Messages;
using NPC.Application.Contexts;

namespace NPC.Application
{
    public class MessageAction : BaseAction
    {
        private readonly MessageRepository _messageRepository;
        public MessageAction()
        {
            _messageRepository = new MessageRepository();
        }

        public MessageListModel InitializeMessageListModel(MessageQueryItem queryItem)
        {
            var model = new MessageListModel();
            queryItem.UnitId = NpcContext.CurrentUser.Unit.Id;
            model.MessageSearchModel.MessageQueryItem = queryItem;
            model.Messages = _messageRepository.Query(queryItem);
            return model;
        }

        public void Delete(params Guid[] ids)
        {
            if (ids != null && ids.Length > 0)
                ids.ToList().ForEach(SingleDelete);
        }

        private void SingleDelete(Guid id)
        {
            var target = _messageRepository.Find(id);
            target.RecordDescription.Delete();
            _messageRepository.SaveOrUpdate(target);
        }
    }
}
