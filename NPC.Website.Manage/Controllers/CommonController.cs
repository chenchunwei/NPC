using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;

namespace NPC.Website.Manage.Controllers
{
    [ValidateInput(false)]
    public class CommonController : BaseController
    {
        const string Inputname = "filedata"; //表单文件域name
        const int Maxattachsize = 58097152; // 最大上传大小， 
        private const string Upext = "txt,rar,zip,jpg,jpeg,gif,png,swf,wmv,avi,wma,mp3,mid"; // 上传扩展名


        public JsonResult Upload()
        {
            var immediate = Request.QueryString["immediate"];//立即上传模式，仅为演示用
            byte[] fileBytes;
            var disposition = Request.ServerVariables["HTTP_CONTENT_DISPOSITION"];
            string localname;
            if (disposition != null)
            {
                fileBytes = Request.BinaryRead(Request.TotalBytes);
                localname = Regex.Match(disposition, "filename=\"(.+?)\"").Groups[1].Value;// 读取原始文件名
            }
            else
            {
                var postedfile = Request.Files.Get(Inputname);
                if (postedfile == null || postedfile.ContentLength <= 0)
                    return new NewtonsoftJsonResult() { Data = new { err = "无数据提交", msg = "无数据提交!" } };
                fileBytes = new byte[postedfile.ContentLength];
                postedfile.InputStream.Read(fileBytes, 0, postedfile.ContentLength);
                localname = postedfile.FileName;
            }
            var extension = GetFileExt(localname);
            //在小校验
            if (fileBytes.Length > Maxattachsize)
            {
                var err = "文件大小不能超过" + ((double)Maxattachsize / (1024 * 1024)).ToString("#0.00") + "M";
                return new NewtonsoftJsonResult() { Data = new { err, msg = err } };
            }
            //扩展校验
            if (("," + Upext + ",").IndexOf("," + extension + ",", System.StringComparison.CurrentCultureIgnoreCase) < 0)
            {
                var err = "上传文件扩展名必需为：" + Upext;
                return new NewtonsoftJsonResult() { Data = new { err, msg = err } };
            }

            // 生成随机文件名
            var random = new Random(DateTime.Now.Millisecond);
            var folder = CreateFolder();
            var newFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + random.Next(10000) + "." + extension;
            var fullPath = System.IO.Path.Combine(folder, newFileName);
            try
            {
                using (var fs = new System.IO.FileStream(Server.MapPath(fullPath), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    fs.Write(fileBytes, 0, fileBytes.Length);
                    fs.Flush();
                }
            }
            catch (Exception e)
            {
                return new NewtonsoftJsonResult() { Data = new { err = e.Message, msg = e.Message } };
            }
            return new NewtonsoftJsonResult { Data = new { err = "", msg = new { url = fullPath, localname, id = "1" } } };
        }

        private string GetFileExt(string fullPath)
        {
            return fullPath != "" ? fullPath.Substring(fullPath.LastIndexOf('.') + 1).ToLower() : "";
        }

        private string CreateFolder()
        {
            var attachDir = "Attachments" + "/" + "day_" + DateTime.Now.ToString("yyyyMMdd") + "/";
            attachDir = System.IO.Path.Combine(HostingEnvironment.ApplicationHost.GetVirtualPath(), attachDir);
            if (!System.IO.Directory.Exists(HttpContext.Server.MapPath(attachDir)))
                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(attachDir));
            return attachDir;
        }

        protected int PageIndex
        {
            get
            {
                int pageIndex;
                if (!string.IsNullOrEmpty(Request["p"]) && int.TryParse(Request["p"], out pageIndex))
                    return pageIndex;
                return 1;
            }
        }

        public ActionResult RedirectToMessage(string message, string returnUrl = "", string textOfReturnUrl = "")
        {
            return new RedirectResult(Url.Action("Message", "System") + string.Format("?message={0}&returnUrl={1}&textOfReturnUrl={2}", message, returnUrl, textOfReturnUrl));
        }
    }
}
