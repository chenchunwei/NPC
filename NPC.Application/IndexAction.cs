using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Utilities;
using NPC.Application;
using NPC.Application.MianModels;
using NPC.Application.MianModels.Homes;
using NPC.Domain.Models.NodeRecords;
using NPC.Domain.Repository;
using NPC.Query.Articles;

namespace Saturday.Application
{
    public class IndexAction : BaseAction
    {
        private readonly ArticleCategoryRepository _articleCategoryRepository;
        private readonly NodeRepository _nodeRepository;
        private readonly NodeRecordRepository _nodeRecordRepository;
        private readonly ArticleRepository _articleRepository;
        public IndexAction()
        {
            _articleRepository = new ArticleRepository();
            _nodeRepository = new NodeRepository();
            _articleCategoryRepository = new ArticleCategoryRepository();
            _nodeRecordRepository = new NodeRecordRepository();
        }
        private IList<NodeRecord> GetNodeRecord(Guid nodeId, int count)
        {
            var unitId = NpcContext.CurrentUser.Unit.Id;
            var nodeRecords = new List<NodeRecord>();
            var node = _nodeRepository.Find(nodeId);
            if (node.OuterCategoryId.HasValue)
            {
                nodeRecords = _nodeRecordRepository.GetTopN(unitId, nodeId, count).ToList();
            }
            return nodeRecords;
        }

        public IndexModel InitializeIndexModel()
        {
            var model = new IndexModel();
            var unitId = NpcContext.CurrentUser.Unit.Id;
            model.BasicsNode = _nodeRepository.GetSingleByCode(unitId, "Basics");
            model.DirectorsNode = _nodeRepository.GetSingleByCode(unitId, "Directors");
            model.ElectionsNode = _nodeRepository.GetSingleByCode(unitId, "Elections");
            model.InvestigatesNode = _nodeRepository.GetSingleByCode(unitId, "Investigates");
            model.LeaderSpeechsNode = _nodeRepository.GetSingleByCode(unitId, "LeaderSpeechs");
            model.LinksNode = _nodeRepository.GetSingleByCode(unitId, "Links");
            model.MediumsNode = _nodeRepository.GetSingleByCode(unitId, "Mediums");
            model.MembersNode = _nodeRepository.GetSingleByCode(unitId, "Members");
            model.NewsNode = _nodeRepository.GetSingleByCode(unitId, "News");
            model.NoticesNode = _nodeRepository.GetSingleByCode(unitId, "Notices");
            model.NpcPicsNode = _nodeRepository.GetSingleByCode(unitId, "NpcPics");
            model.NpcWorksNode = _nodeRepository.GetSingleByCode(unitId, "NpcWorks");
            model.SelfImprovementNode = _nodeRepository.GetSingleByCode(unitId, "SelfImprovement");
            model.SuperviseWindowNode = _nodeRepository.GetSingleByCode(unitId, "SuperviseWindow");
            model.TownPicsNode = _nodeRepository.GetSingleByCode(unitId, "TownPics");
            model.ViceDirectorsNode = _nodeRepository.GetSingleByCode(unitId, "ViceDirectors");
            model.VideoNode = _nodeRepository.GetSingleByCode(unitId, "Video");

            model.News = _nodeRecordRepository.GetTopN(unitId, "News", 8);
            FillRecords(model.News, unitId, 0, 8, "News");

            model.WheelBroadcastPicsOfTopLeft = _nodeRecordRepository.GetTopN(unitId, "WheelBroadcastPicsOfTopLeft", 4);

            model.Links = _nodeRecordRepository.GetTopN(unitId, "Links", 6);
            model.FirstFullColumn = _nodeRecordRepository.GetTopN(unitId, "FirstFullColumn", 1).FirstOrDefault();

            model.News = _nodeRecordRepository.GetTopN(unitId, "Notices", 5);
            FillRecords(model.News, unitId, 0, 5, "Notices");

            model.Directors = _nodeRecordRepository.GetTopN(unitId, "Directors", 1);
            FillRecords(model.ViceDirectors, unitId, 1, 0, "Directors");

            model.ViceDirectors = _nodeRecordRepository.GetTopN(unitId, "ViceDirectors", 10);
            FillRecords(model.ViceDirectors, unitId, 10, 0, "ViceDirectors");

            model.Members = _nodeRecordRepository.GetTopN(unitId, "Members", 30);
            FillRecords(model.Members, unitId, 0, 30, "Members");

            model.LeaderSpeechs = _nodeRecordRepository.GetTopN(unitId, "LeaderSpeechs", 6);
            FillRecords(model.News, unitId, 0, 6, "LeaderSpeechs");

            model.Video = _nodeRecordRepository.GetTopN(unitId, "Video", 1).FirstOrDefault();

            model.SuperviseWindow = _nodeRecordRepository.GetTopN(unitId, "SuperviseWindow", 5);
            FillRecords(model.News, unitId, 0, 5, "SuperviseWindow");

            model.NpcWorks = _nodeRecordRepository.GetTopN(unitId, "NpcWorks", 5);
            FillRecords(model.News, unitId, 0, 5, "NpcWorks");

            model.SelfImprovement = _nodeRecordRepository.GetTopN(unitId, "SelfImprovement", 7);
            FillRecords(model.News, unitId, 0, 7, "SelfImprovement");

            model.Basics = _nodeRecordRepository.GetTopN(unitId, "Basics", 7);
            FillRecords(model.News, unitId, 0, 7, "Basics");

            model.Mediums = _nodeRecordRepository.GetTopN(unitId, "Mediums", 3);
            FillRecords(model.News, unitId, 3, 0, "Mediums");

            model.TownPics = _nodeRecordRepository.GetTopN(unitId, "TownPics", 20);
            FillRecords(model.News, unitId, 20, 0, "TownPics");

            model.Elections = _nodeRecordRepository.GetTopN(unitId, "Elections", 8);
            FillRecords(model.News, unitId, 0, 8, "Elections");

            model.Investigates = _nodeRecordRepository.GetTopN(unitId, "Investigates", 8);
            FillRecords(model.News, unitId, 0, 8, "Investigates");

            model.NpcPics = _nodeRecordRepository.GetTopN(unitId, "NpcPics", 80);
            FillRecords(model.News, unitId, 80, 0, "NpcPics");

            return model;
        }

        private void FillRecords(ICollection<NodeRecord> nodeRecords, Guid unitId, int picTopN, int normalTopN, string code)
        {
            var node = _nodeRepository.GetSingleByCode(unitId, code);
            if (node == null)
                return;
            if (!node.OuterCategoryId.HasValue)
                return;
            if (nodeRecords.Count >= picTopN + normalTopN)
                return;
            var needPicN = picTopN - nodeRecords.Count;
            if (needPicN < 0)
                needPicN = 0;
            var needNormalN = normalTopN - (nodeRecords.Count - picTopN);
            if (needNormalN < 0)
                needNormalN = 0;

            if (needNormalN <= 0) return;

            var articles = _articleRepository.GetTopNWithPic(unitId, node.OuterCategoryId.Value, needPicN, needNormalN).ToList();
            foreach (var article in articles)
            {
                nodeRecords.Add(new NodeRecord()
                {
                    FirstTitle = article.Title,
                    RecordLink = "/Home/Detail?Id=" + article.Id,
                    FirstContent = MyString.RemoveSpaceString(MyString.RemoveHtml(article.Content)),
                    FirstImage = article.UrlOfCoverImage
                });
            }
        }

        private NodeRecord FillSingleRecord(Guid unitId, string code, bool isPic)
        {
            var node = _nodeRepository.GetSingleByCode(unitId, code);
            if (node == null)
                return null;
            if (!node.OuterCategoryId.HasValue)
                return null;
            var article = isPic
                ? _articleRepository.GetTopNPic(unitId, node.OuterCategoryId.Value, 1).FirstOrDefault()
                : _articleRepository.GetTopN(unitId, node.OuterCategoryId.Value, 1).FirstOrDefault();
            if (article != null)
            {
                var nodeRecord = new NodeRecord();
                nodeRecord.FirstTitle = article.Title;
                nodeRecord.RecordLink = "/Home/Detail?Id=" + article.Id;
                nodeRecord.FirstContent = MyString.RemoveSpaceString(MyString.RemoveHtml(article.Content));
                nodeRecord.FirstImage = "/Manage/Attachments/" + article.UrlOfCoverImage;
                return nodeRecord;
            }
            return null;
        }

        public DetailModel InitializeDetailModel(Guid id)
        {
            var model = new DetailModel();
            model.Article = _articleRepository.Find(id);
            return model;
        }

        public ListModel InitializeListModel(ArticleQueryItem queryItem)
        {
            var model = new ListModel();
            model.ArticleQueryItem = queryItem;
            model.ArticleQueryItem.IsShow = true;
            model.Articles = _articleRepository.Query(queryItem).ToList();
            model.ListTitle = "文章列表";
            if (queryItem.CategoryId.HasValue)
            {
                var category = _articleCategoryRepository.Find(queryItem.CategoryId.Value);
                if (category != null)
                    model.ListTitle = category.CategoryName;
            }
            model.ArticleQueryItem = queryItem;
            return model;
        }

        public HeaderModel InitializeHeaderModel()
        {
            var unitId = NpcContext.CurrentUser.Unit.Id;
            var model = new HeaderModel();
            model.Menus = _nodeRecordRepository.GetTopN(unitId, "Menus", 15);
            model.TopBanner = _nodeRecordRepository.GetTopN(unitId, "TopBanner", 1).FirstOrDefault();
            return model;
        }

        public Tuple<string, string> InitializeFooterModel()
        {
            var unitId = NpcContext.CurrentUser.Unit.Id;
            var nodeRecord = _nodeRecordRepository.GetTopN(unitId, "Copyright", 1).FirstOrDefault();
            if (nodeRecord == null)
                return new Tuple<string, string>("", "");
            return new Tuple<string, string>(nodeRecord.FirstTitle, nodeRecord.SecondTitle);
        }
    }
}
