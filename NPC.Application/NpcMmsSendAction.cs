using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.ManageModels.NpcMmsSends;
using NPC.Domain.Models.NpcMmsSends;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class NpcMmsSendAction:BaseAction
    {
        private readonly NpcMmsSendRepository _npcMmsSendRepository;
        public NpcMmsSendAction()
        {
            _npcMmsSendRepository=new NpcMmsSendRepository();
        }
        public NpcMmsSendListModel InitializeNpcMmsSendListModel(NpcMmsSendQueryItem queryItem)
        {
            var model = new NpcMmsSendListModel();
            queryItem.UnitId = NpcContext.CurrentUser.Unit.Id;
            model.NpcMmsSendSearchModel.NpcMmsSendQueryItem = queryItem;
            model.NpcMmsSends = _npcMmsSendRepository.Query(queryItem);
            return model;
        }

        public void Delete(params Guid[] ids)
        {
            if (ids != null && ids.Length > 0)
                ids.ToList().ForEach(SingleDelete);
        }

        private void SingleDelete(Guid id)
        {
            var target = _npcMmsSendRepository.Find(id);
            target.RecordDescription.Delete();
            _npcMmsSendRepository.SaveOrUpdate(target);
        }
    }
}
