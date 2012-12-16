using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application;
using NPC.Application.ManageModels.Nodes;
using NPC.Domain.Models.Nodes;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class NodeAction : BaseAction
    {
        private readonly NodeRepository _nodeRepository;
        private readonly ArticleCategoryRepository _articleCategoryRepository;
        public NodeAction()
        {
            _articleCategoryRepository = new ArticleCategoryRepository();
            _nodeRepository = new NodeRepository();
        }
        #region 初始化树模型
        public NodeTreeModel InitializeNodeTreeModel(Guid? id)
        {
            var model = new NodeTreeModel();
            var unitId = NpcContext.CurrentUser.Unit.Id;
            var subs = id.HasValue ? _nodeRepository.GetSubsInUnit(unitId, id.Value) : _nodeRepository.GetRootNodesInUnit(unitId);
            subs.ToList().ForEach(o => model.Components.Add(ConvertArticleCategoryToModel(o, true)));
            return model;

        }
        #endregion

        #region 转换ArticleCategory到Model
        private NodeTreeModelComponent ConvertArticleCategoryToModel(Node node, bool isNeedSub)
        {
            var model = new NodeTreeModelComponent()
            {
                Id = node.Id,
                Name = node.Name,
                CategoryId = node.OuterCategoryId,
                Code = node.Code,
                CategoryName = node.OuterCategoryId.HasValue ? _articleCategoryRepository.Find(node.OuterCategoryId.Value).CategoryName : "",
                IconCls = ApplicationConst.TreeLeafCls,
                NodeRecordMark = node.NodeRecordMark
            };
            var childrens = _nodeRepository.GetSubs(node.Id).ToList();
            if (childrens.Any())
            {
                if (isNeedSub)
                {
                    childrens.ForEach(o => model.Childrens.Add(ConvertArticleCategoryToModel(o, true)));
                }
                model.IconCls = ApplicationConst.TreeParentNode;
                model.State = isNeedSub ? "open" : "closed";
            }

            return model;
        }
        #endregion

        #region 添加新的分类
        public void CreateNewNode(EditNodeModel model)
        {
            var node = new Node();
            node.Name = model.FormData.Name;
            node.Code = model.FormData.Code;
            if (_nodeRepository.IsNodeCodeRepeatInUnit(NpcContext.CurrentUser.Unit.Id, node.Code))
            {
                throw new ApplicationException("节点编码不能重复，请重新设置！");
            }
            if (model.Id.HasValue)
            {
                node.ParentNode = _nodeRepository.Find(model.Id.Value);
            }
            node.Unit = NpcContext.CurrentUser.Unit;
            node.RecordDescription.CreateBy(NpcContext.CurrentUser);
            _nodeRepository.Save(node);
        }
        #endregion

        #region 删除
        public void Delete(Guid id)
        {
            var target = _nodeRepository.Find(id);
            target.RecordDescription.Delete();
            _nodeRepository.Save(target);
        }
        #endregion

        public void RelateNode(RelateNodeModel model)
        {
            var node = _nodeRepository.Find(model.NodeId);
            node.OuterCategoryId = model.ArticleCategoryId;
            _nodeRepository.Save(node);
        }

        public void SettingNode(Guid nodeId, NodeRecordMark nodeRecordMark)
        {
            var node = _nodeRepository.Find(nodeId);
            node.NodeRecordMark = nodeRecordMark;
            _nodeRepository.Save(node);
        }
    }
}
