using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dimac.JMail;
using Fluent.Infrastructure.Utilities;
using NPC.Application;
using NPC.Application.Common;
using NPC.Application.Contexts;
using NPC.Application.MainTownModels;
using NPC.Application.MianModels;
using NPC.Application.MianModels.Homes;
using NPC.Domain.Models.Articles;
using NPC.Domain.Models.NodeRecords;
using NPC.Domain.Repository;
using HeaderModel = NPC.Application.MianModels.HeaderModel;

namespace NPC.Application
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
            var unitId = NpcMainWebContext.CurrentUnit.Id;
            var nodeRecords = new List<NodeRecord>();
            var node = _nodeRepository.Find(nodeId);
            if (node.OuterCategoryId.HasValue)
            {
                nodeRecords = _nodeRecordRepository.GetTopN(unitId, nodeId, count).ToList();
            }
            return nodeRecords;
        }

        public MainTownModels.Homes.IndexModel InitializeMainTownIndexModel()
        {
            var model = new MainTownModels.Homes.IndexModel();
            var unitId = NpcMainWebContext.CurrentUnit.Id;
            model.Unit = NpcMainWebContext.CurrentUnit;
            model.BottomPicsRollingNode = _nodeRepository.GetSingleByCode(unitId, "BottomPicsRolling");
            model.LatestAttentionsNode = _nodeRepository.GetSingleByCode(unitId, "LatestAttentionNode");
            model.ChairmansModuleNode = _nodeRepository.GetSingleByCode(unitId, "ChairmansModule");
            model.DealedProposalsNode = _nodeRepository.GetSingleByCode(unitId, "DealedProposals");
            model.HalfMonthlyTalkingsNode = _nodeRepository.GetSingleByCode(unitId, "HalfMonthlyTalkings");
            model.NavigationsNode = _nodeRepository.GetSingleByCode(unitId, "Navigations");
            model.NewsNode = _nodeRepository.GetSingleByCode(unitId, "News");
            model.NoticesNode = _nodeRepository.GetSingleByCode(unitId, "Notices");
            model.NpcWorksNode = _nodeRepository.GetSingleByCode(unitId, "NpcWorks");
            model.PublicProposalsNode = _nodeRepository.GetSingleByCode(unitId, "PublicProposals");
            model.ReferFilesNode = _nodeRepository.GetSingleByCode(unitId, "ReferFiles");
            model.RegulationsNode = _nodeRepository.GetSingleByCode(unitId, "Regulations");
            model.StudyMaterialsNode = _nodeRepository.GetSingleByCode(unitId, "StudyMaterials");
            model.StudySectionNode = _nodeRepository.GetSingleByCode(unitId, "StudySection");
            model.SuperviseNode = _nodeRepository.GetSingleByCode(unitId, "Supervises");
            model.MembersNode = _nodeRepository.GetSingleByCode(unitId, "Members");

            model.FirstFullColumn = _nodeRecordRepository.GetTopN(unitId, "FirstFullColumn", 1).FirstOrDefault();
            model.LatestAttentions = _nodeRecordRepository.GetTopN(unitId, "LatestAttentions", 8);
            FillRecords(model.BottomPicsRolling, unitId, 0, 8, "LatestAttentions");

            model.WheelBroadcastPicsOfTopLeft = _nodeRecordRepository.GetTopN(unitId, "WheelBroadcastPicsOfTopLeft", 4);

            model.BottomPicsRolling = _nodeRecordRepository.GetTopN(unitId, "BottomPicsRolling", 20);
            FillRecords(model.BottomPicsRolling, unitId, 20, 0, "BottomPicsRolling");

            model.ChairmansModules = _nodeRecordRepository.GetTopN(unitId, "ChairmansModule", 10);
            FillRecords(model.ChairmansModules, unitId, 0, 10, "ChairmansModule");

            model.DealedProposals = _nodeRecordRepository.GetTopN(unitId, "DealedProposals", 4);
            FillRecords(model.DealedProposals, unitId, 0, 4, "DealedProposals");

            model.HalfMonthlyTalkings = _nodeRecordRepository.GetTopN(unitId, "HalfMonthlyTalkings", 5);
            FillRecords(model.HalfMonthlyTalkings, unitId, 0, 5, "HalfMonthlyTalkings");

            model.News = _nodeRecordRepository.GetTopN(unitId, "News", 8);
            FillRecords(model.News, unitId, 0, 8, "News");

            model.PicNews = _nodeRecordRepository.GetTopN(unitId, "PicNews", 4);
            FillRecords(model.PicNews, unitId, 0, 4, "PicNews");

            model.Notices = _nodeRecordRepository.GetTopN(unitId, "Notices", 18);
            FillRecords(model.Notices, unitId, 0, 18, "Notices");

            model.NpcWorks = _nodeRecordRepository.GetTopN(unitId, "NpcWorks", 8);
            FillRecords(model.NpcWorks, unitId, 0, 8, "NpcWorks");

            model.PublicProposals = _nodeRecordRepository.GetTopN(unitId, "PublicProposals", 5);
            FillRecords(model.PublicProposals, unitId, 0, 5, "PublicProposals");

            model.ReferFiles = _nodeRecordRepository.GetTopN(unitId, "ReferFiles", 3);
            FillRecords(model.ReferFiles, unitId, 0, 3, "ReferFiles");

            model.Regulations = _nodeRecordRepository.GetTopN(unitId, "Regulations", 5);
            FillRecords(model.Regulations, unitId, 0, 5, "Regulations");

            model.StudyMaterials = _nodeRecordRepository.GetTopN(unitId, "StudyMaterials", 5);
            FillRecords(model.StudyMaterials, unitId, 0, 5, "StudyMaterials");

            model.Supervises = _nodeRecordRepository.GetTopN(unitId, "Supervises", 8);
            FillRecords(model.Supervises, unitId, 0, 8, "Supervises");

            model.MembersNode = _nodeRepository.GetSingleByCode(unitId, "Members");
            model.Members = _nodeRecordRepository.GetTopN(unitId, "Members", 1);
            FillRecords(model.Members, unitId, 0, 1, "Members");

            model.Video = _nodeRecordRepository.GetTopN(unitId, "Video", 1).FirstOrDefault();

            return model;
        }

        public IndexModel InitializeIndexModel()
        {
            var model = new IndexModel();
            var unitId = NpcMainWebContext.CurrentUnit.Id;
            model.Unit = NpcMainWebContext.CurrentUnit;
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

            model.Notices = _nodeRecordRepository.GetTopN(unitId, "Notices", 5);
            FillRecords(model.Notices, unitId, 0, 5, "Notices");


            model.Directors = _nodeRecordRepository.GetTopN(unitId, "Directors", 1);
            FillRecords(model.Directors, unitId, 1, 0, "Directors");

            model.ViceDirectors = _nodeRecordRepository.GetTopN(unitId, "ViceDirectors", 10);
            FillRecords(model.ViceDirectors, unitId, 10, 0, "ViceDirectors");

            model.Members = _nodeRecordRepository.GetTopN(unitId, "Members", 30);
            FillRecords(model.Members, unitId, 0, 30, "Members");

            model.LeaderSpeechs = _nodeRecordRepository.GetTopN(unitId, "LeaderSpeechs", 6);
            FillRecords(model.LeaderSpeechs, unitId, 0, 6, "LeaderSpeechs");

            model.Video = _nodeRecordRepository.GetTopN(unitId, "Video", 1).FirstOrDefault();

            model.SuperviseWindow = _nodeRecordRepository.GetTopN(unitId, "SuperviseWindow", 5);
            FillRecords(model.SuperviseWindow, unitId, 0, 5, "SuperviseWindow");

            model.NpcWorks = _nodeRecordRepository.GetTopN(unitId, "NpcWorks", 5);
            FillRecords(model.NpcWorks, unitId, 0, 5, "NpcWorks");

            model.SelfImprovement = _nodeRecordRepository.GetTopN(unitId, "SelfImprovement", 7);
            FillRecords(model.SelfImprovement, unitId, 0, 7, "SelfImprovement");

            model.Basics = _nodeRecordRepository.GetTopN(unitId, "Basics", 7);
            FillRecords(model.Basics, unitId, 0, 7, "Basics");

            model.Mediums = _nodeRecordRepository.GetTopN(unitId, "Mediums", 3);
            FillRecords(model.Mediums, unitId, 3, 0, "Mediums");

            model.TownPics = _nodeRecordRepository.GetTopN(unitId, "TownPics", 20);
            FillRecords(model.TownPics, unitId, 20, 0, "TownPics");

            model.Elections = _nodeRecordRepository.GetTopN(unitId, "Elections", 8);
            FillRecords(model.Elections, unitId, 0, 8, "Elections");

            model.Investigates = _nodeRecordRepository.GetTopN(unitId, "Investigates", 8);
            FillRecords(model.Investigates, unitId, 0, 8, "Investigates");

            model.NpcPics = _nodeRecordRepository.GetTopN(unitId, "NpcPics", 80);
            FillRecords(model.NpcPics, unitId, 80, 0, "NpcPics");

            model.ContributeNode = _nodeRecordRepository.GetTopN(unitId, "Contribute", 1).FirstOrDefault();
            return model;
        }

        private void FillRecords(ICollection<NodeRecord> nodeRecords, Guid unitId, int picTopN, int normalTopN,
                                 string code)
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

            var articles =
                _articleRepository.GetTopNWithPic(unitId, node.OuterCategoryId.Value, needPicN, needNormalN).ToList();
            foreach (var article in articles)
            {
                var record = new NodeRecord()
                {
                    FirstTitle = article.Title,
                    RecordLink = "/Home/Detail?Id=" + article.Id,
                    FirstContent = MyString.RemoveSpaceString(MyString.RemoveHtml(article.Content)),
                    FirstImage = article.UrlOfCoverImage
                };
                record.RecordDescription.DateOfCreate = article.RecordDescription.DateOfCreate;
                nodeRecords.Add(record);
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
                nodeRecord.FirstImage = ApplicationConst.ImagePath + article.UrlOfCoverImage;
                nodeRecord.RecordDescription.DateOfCreate = article.RecordDescription.DateOfCreate;
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
            queryItem.UnitId = NpcMainWebContext.CurrentUnit.Id;
            model.ArticleQueryItem = queryItem;
            model.ArticleQueryItem.IsShow = true;
            model.Articles = _articleRepository.Query(queryItem).ToList();
            model.ListTitle = "文章列表";
            var categoryId = queryItem.CategoryId ?? queryItem.CategoryIdLike;
            if (categoryId.HasValue)
            {
                var category = _articleCategoryRepository.Find(categoryId.Value);
                if (category != null)
                    model.ListTitle = category.CategoryName;
            }
            model.ArticleQueryItem = queryItem;
            return model;
        }

        public HeaderModel InitializeHeaderModel()
        {
            var unitId = NpcMainWebContext.CurrentUnit.Id;
            var model = new HeaderModel();
            model.Menus = _nodeRecordRepository.GetTopN(unitId, "PopMenus", 1);
            model.TopBanner = _nodeRecordRepository.GetTopN(unitId, "TopBanner", 1).FirstOrDefault();
            return model;
        }

        public MainTownModels.HeaderModel InitializeMianTownHeaderModel()
        {
            var unitId = NpcMainWebContext.CurrentUnit.Id;
            var model = new MainTownModels.HeaderModel();
            model.Menus = _nodeRecordRepository.GetTopN(unitId, "Menus", 15);
            model.Unit = NpcMainWebContext.CurrentUnit;
            model.TopBanner = _nodeRecordRepository.GetTopN(unitId, "TopBanner", 1).FirstOrDefault();
            return model;
        }

        public Tuple<string, string> InitializeFooterModel()
        {
            var unitId = NpcMainWebContext.CurrentUnit.Id;
            var nodeRecord = _nodeRecordRepository.GetTopN(unitId, "Copyright", 1).FirstOrDefault();
            if (nodeRecord == null)
                return new Tuple<string, string>("", "");
            return new Tuple<string, string>(nodeRecord.FirstTitle, nodeRecord.SecondTitle);
        }

        public object InitializeRecordsModel(NodeRecordQueryItem queryItem)
        {
            var model = new RecordsModel();
            model.NodeRecordQueryItem = queryItem;
            model.NodeRecordQueryItem.IsShow = true;
            model.NodeRecords = _nodeRecordRepository.Query(queryItem).ToList();
            model.ListTitle = "文章列表";
            var nodeId = queryItem.NodeId ?? queryItem.NodeIdLike;
            if (nodeId.HasValue)
            {
                var node = _nodeRepository.Find(nodeId.Value);
                model.ListTitle = node.Name;
            }
            return model;
        }

        public SideBarModel InitializeSideBarModel()
        {
            var unitId = NpcMainWebContext.CurrentUnit.Id;
            var model = new SideBarModel();
            model.ProposalNode = _nodeRecordRepository.GetTopN(unitId, "ProposalEntry", 1).FirstOrDefault();
            model.Mediums = _nodeRecordRepository.GetTopN(unitId, "Mediums", 3);
            FillRecords(model.Mediums, unitId, 3, 0, "Mediums");
            return model;
        }

        public FooterModel InitializeMainTownFooterModel()
        {
            var unitId = NpcMainWebContext.CurrentUnit.Id;
            var model = new FooterModel();
            model.CopyRight = _nodeRecordRepository.GetTopN(unitId, "CopyRight", 1).FirstOrDefault();
            return model;
        }

        public object InitializeRightBarModel()
        {
            var unitId = NpcMainWebContext.CurrentUnit.Id;
            var model = new RightBarModel();
            model.PublicProposals = _nodeRecordRepository.GetTopN(unitId, "PublicProposals", 8);
            FillRecords(model.PublicProposals, unitId, 0, 8, "PublicProposals");
            model.PublicProposalsNode = _nodeRepository.GetSingleByCode(unitId, "PublicProposals");
            return model;
        }

        public void Contribute(ContributeModel model)
        {
            var userName = AppConfig.SmtpUserName;
            var content = string.Format("标题：{0}<br/>作者:{1}<p/>正文：{2}", model.Title, model.Author, model.Content);
            var message = new Message();
            message.Subject = "【新闻投稿】" + model.Title;
            message.From = userName;
            message.Charset = System.Text.Encoding.GetEncoding("GB2312");
            message.BodyHtml = content;
            message.To.Add(AppConfig.ContributeSendTo);
            // 设置SMTP
            var smt = new Smtp
            {
                UserName = userName,
                Password = AppConfig.SmtpPassword,
                HostName = AppConfig.SmtpServer,
                Domain = AppConfig.SmtpDomain,
                Port = short.Parse(AppConfig.SmtpPort),
                Authentication = SmtpAuthentication.Login
            };
            smt.Send(message);
        }
    }
}