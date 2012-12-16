using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application;
using NPC.Application.ManageModels.NodeRecords;
using NPC.Domain.Models.NodeRecords;
using NPC.Domain.Models.Nodes;
using NPC.Domain.Repository;

namespace Saturday.Application
{
    public class NodeRecordAction : BaseAction
    {
        private readonly NodeRecordRepository _nodeRecordRepository;
        private readonly NodeRepository _nodeRepository;
        public NodeRecordAction()
        {
            _nodeRecordRepository = new NodeRecordRepository();
            _nodeRepository = new NodeRepository();
        }
        public EditNodeRecordModel InitializeEditNodeRecordModel(Guid? nodeId, Guid? nodeRecordId)
        {
            var model = new EditNodeRecordModel();
            if (nodeRecordId.HasValue)
            {
                var nodeRecord = _nodeRecordRepository.Find(nodeRecordId.Value);
                model.Node = nodeRecord.BelongsToNode;
                FillFormData(model.FormData, nodeRecord);
            }
            if (nodeId.HasValue)
            {
                model.Node = _nodeRepository.Find(nodeId.Value);
            }
            model.FormData.SelectedNodeId = model.Node != null ? model.Node.Id : default(Guid?);
            model.Node = model.Node ?? new Node();
            WapperNodeRecordMark(model.Node);
            return model;
        }

        #region private FillFormData()
        public void FillFormData(EditNodeRecordModelFormData formData, NodeRecord nodeRecord)
        {
            formData.SelectedNodeId = nodeRecord.BelongsToNode.Id;
            formData.FirstImage = nodeRecord.FirstImage;
            formData.FirstTitle = nodeRecord.FirstTitle;
            formData.FirstContent = nodeRecord.FirstContent;
            formData.SecondTitle = nodeRecord.SecondTitle;
            formData.SecondImage = nodeRecord.SecondImage;
            formData.SecondContent = nodeRecord.SecondContent;
            formData.RecordLink = nodeRecord.RecordLink;
        }
        #endregion

        public void NewNodeRecord(EditNodeRecordModel model)
        {
            if (!model.FormData.SelectedNodeId.HasValue)
                throw new ArgumentException("model.FormData.SelectedNodeId不能为空");
            var nodeRecord = new NodeRecord();
            FillNodeRecord(nodeRecord, model.FormData);
            nodeRecord.RecordDescription.CreateBy(NpcContext.CurrentUser);
            _nodeRecordRepository.Save(nodeRecord);
        }

        public void UpdateNodeRecord(EditNodeRecordModel model)
        {
            if (!model.FormData.SelectedNodeId.HasValue)
                throw new ArgumentException("model.FormData.SelectedNodeId不能为空");
            if (!model.Id.HasValue)
                throw new ArgumentException("model.Id不能为空");
            var nodeRecord = _nodeRecordRepository.Find(model.Id.Value);
            FillNodeRecord(nodeRecord, model.FormData);
            nodeRecord.RecordDescription.UpdateBy(NpcContext.CurrentUser);
            _nodeRecordRepository.Save(nodeRecord);
            model.Id = nodeRecord.Id;
        }

        private void FillNodeRecord(NodeRecord nodeRecord, EditNodeRecordModelFormData formData)
        {
            nodeRecord.BelongsToNode = _nodeRepository.Find(formData.SelectedNodeId.Value);
            nodeRecord.FirstTitle = formData.FirstTitle;
            nodeRecord.FirstContent = formData.FirstContent;
            nodeRecord.SecondTitle = formData.SecondTitle;
            nodeRecord.SecondContent = formData.SecondContent;
            nodeRecord.RecordDescription.UserOfLasetestModify = NpcContext.CurrentUser;
            nodeRecord.RecordLink = formData.RecordLink;
            nodeRecord.IsShow = formData.IsShow;
            if (!string.IsNullOrEmpty(formData.FirstImage))
                nodeRecord.FirstImage = formData.FirstImage;
            if (!string.IsNullOrEmpty(formData.SecondImage))
                nodeRecord.SecondImage = formData.SecondImage;
        }

        public NodeRecordListModel InitializeNodeRecordListModel(NodeRecordQueryItem queryItem)
        {
            var model = new NodeRecordListModel();
            model.NodeRecordSearchModel.NodeRecordQueryItem = queryItem;
            model.NodeRecords = _nodeRecordRepository.Query(queryItem);
            return model;
        }

        public void Delete(params Guid[] ids)
        {
            if (ids != null && ids.Length > 0)
                ids.ToList().ForEach(SingleDelete);
        }

        private void SingleDelete(Guid id)
        {
            var target = _nodeRecordRepository.Find(id);
            target.RecordDescription.Delete();
            _nodeRecordRepository.SaveOrUpdate(target);
        }

        private void WapperNodeRecordMark(Node node)
        {
            if (node.NodeRecordMark == null)
            {
                node.NodeRecordMark = new NodeRecordMark();
            }
            if (string.IsNullOrEmpty(node.NodeRecordMark.FirstContentTitle))
            {
                node.NodeRecordMark.FirstContentTitle = "内容一";
            }
            if (string.IsNullOrEmpty(node.NodeRecordMark.FirstImageTitle))
            {
                node.NodeRecordMark.FirstImageTitle = "图片一";
            }
            if (string.IsNullOrEmpty(node.NodeRecordMark.FisrtTitleTitle))
            {
                node.NodeRecordMark.FisrtTitleTitle = "标题一";
            }
            if (string.IsNullOrEmpty(node.NodeRecordMark.RecordLinkTitle))
            {
                node.NodeRecordMark.RecordLinkTitle = "链接";
            }
            if (string.IsNullOrEmpty(node.NodeRecordMark.SecondContentTitle))
            {
                node.NodeRecordMark.SecondContentTitle = "内容二";
            }
            if (string.IsNullOrEmpty(node.NodeRecordMark.SecondImageTitle))
            {
                node.NodeRecordMark.SecondImageTitle = "图片二";
            }
            if (string.IsNullOrEmpty(node.NodeRecordMark.SecondTitleTitle))
            {
                node.NodeRecordMark.SecondTitleTitle = "标题二";
            }
        }
    }
}
