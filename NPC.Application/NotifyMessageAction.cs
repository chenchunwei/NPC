using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.NotifyMessages;
using NPC.Domain.Models.NotifyMessages;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class NotifyMessageAction:BaseAction
    {
        private readonly NotifyMessageRepository _notifyMessageRepository;
        public NotifyMessageAction()
        {
            _notifyMessageRepository=new NotifyMessageRepository();
        }
        public NotifyMessageListModel InitializeNpcMmsListModel(NotifyMessageQueryItem queryItem)
        {
            var model = new NotifyMessageListModel();
            queryItem.UnitId = NpcContext.CurrentUser.Unit.Id;
            model.NotifyMessageSearchModel.NotifyMessageQueryItem = queryItem;
            model.NotifyMessages = _notifyMessageRepository.Query(queryItem);
            return model;
        }

        public void Delete(params Guid[] ids)
        {
            if (ids != null && ids.Length > 0)
                ids.ToList().ForEach(SingleDelete);
        }

        private void SingleDelete(Guid id)
        {
            var target = _notifyMessageRepository.Find(id);
            target.RecordDescription.Delete();
            _notifyMessageRepository.SaveOrUpdate(target);
        }
    }
}
