using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.ArticleCategories
{
    public class EditArticleCategoryModel
    {
        public Guid? Id { get; set; }
        public EditArticleCategoryModelFormData FormData { get; set; }
    }

    public class EditArticleCategoryModelFormData
    {
        public string Name { get; set; }
    }
}
