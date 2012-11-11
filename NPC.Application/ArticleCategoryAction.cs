using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.ManageModels.ArticleCategories;
using NPC.Domain.Models.ArticleCategories;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class ArticleCategoryAction : BaseAction
    {
        private readonly ArticleCategoryRepository _articleCategoryRepository;
        public ArticleCategoryAction()
        {
            _articleCategoryRepository=new ArticleCategoryRepository();
        }
        #region 初始化树模型
       public ArticleCategoryTreeModel InitializeArticleCategoryTreeModel(Guid? id)
        {
            var model = new ArticleCategoryTreeModel();
            var subs = id.HasValue ? _articleCategoryRepository.GetSubs(NpcContext.CurrentUser.Unit.Id, id.Value) : _articleCategoryRepository.GetRoot(NpcContext.CurrentUser.Unit.Id);

            subs.ToList().ForEach(o => model.Components.Add(ConvertArticleCategoryToModel(o, true)));

            return model;

        }
        #endregion
        #region 转换unit到Model
        private ArticleCategoryTreeModelComponent ConvertArticleCategoryToModel(ArticleCategory articleCategory, bool isNeedSub)
        {
            var model = new ArticleCategoryTreeModelComponent()
            {
                Id = articleCategory.Id,
                Text = articleCategory.CategoryName,
                IconCls = ApplicationConst.TreeLeafCls,
            };
            var childrens = _articleCategoryRepository.GetSubs(NpcContext.CurrentUser.Unit.Id, articleCategory.Id).ToList();
            if (childrens.Any())
            {
                if (isNeedSub)
                {
                    model.State = "";
                    childrens.ForEach(o => model.Childrens.Add(ConvertArticleCategoryToModel(o, false)));
                }
                model.IconCls = ApplicationConst.TreeParentNode;
            }
            else
                model.State = "";

            return model;
        }
        #endregion
       
    }
}
