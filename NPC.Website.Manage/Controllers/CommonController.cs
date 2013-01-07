using System;
using System.Collections.Generic;
using System.IO;
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
        #region SwfUpload
        public ActionResult SwfUpload()
        {
            System.Drawing.Image thumbnailImage = null;
            System.Drawing.Image originalImage = null;
            System.Drawing.Bitmap finalImage = null;
            System.Drawing.Graphics graphic = null;
            MemoryStream ms = null;

            try
            {
                HttpPostedFileBase jpegImageUpload = Request.Files["Filedata"];

                // Retrieve the uploaded image
                if (jpegImageUpload == null)
                {
                    Response.StatusCode = 500;
                    Response.Write("An error occured");
                    return new ContentResult();
                }
                originalImage = System.Drawing.Image.FromStream(jpegImageUpload.InputStream);

                // Calculate the new width and height
                int width = originalImage.Width;
                int height = originalImage.Height;
                const int targetWidth = 300;
                const int targetHeight = 300;
                int newWidth, newHeight;

                const float targetRatio = (float)targetWidth / (float)targetHeight;
                var imageRatio = (float)width / (float)height;

                if (targetRatio > imageRatio)
                {
                    newHeight = targetHeight;
                    newWidth = (int)Math.Floor(imageRatio * (float)targetHeight);
                }
                else
                {
                    newHeight = (int)Math.Floor((float)targetWidth / imageRatio);
                    newWidth = targetWidth;
                }

                newWidth = newWidth > targetWidth ? targetWidth : newWidth;
                newHeight = newHeight > targetHeight ? targetHeight : newHeight;

                // Create the thumbnail
                // Old way
                //thumbnail_image = original_image.GetThumbnailImage(new_width, new_height, null, System.IntPtr.Zero);
                // We don't have to create a Thumbnail since the DrawImage method will resize, but the GetThumbnailImage looks better
                // I've read about a problem with GetThumbnailImage. If a jpeg has an embedded thumbnail it will use and resize it which
                // can result in a tiny 40x40 thumbnail being stretch up to our target size
                finalImage = new System.Drawing.Bitmap(targetWidth, targetHeight);
                graphic = System.Drawing.Graphics.FromImage(finalImage);
                graphic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), new System.Drawing.Rectangle(0, 0, targetWidth, targetHeight));
                int pasteX = (targetWidth - newWidth) / 2;
                int pasteY = (targetHeight - newHeight) / 2;
                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; /* new way */
                //graphic.DrawImage(thumbnail_image, paste_x, paste_y, new_width, new_height);
                graphic.DrawImage(originalImage, pasteX, pasteY, newWidth, newHeight);
                // Store the thumbnail in the session (Note: this is bad, it will take a lot of memory, but this is just a demo)
                ms = new MemoryStream();
                finalImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Store the data in my custom Thumbnail object
                var thumbnailID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var thumb = new Thumbnail(thumbnailID, ms.GetBuffer());

                // Put it all in the Session (initialize the session if necessary)			
                var thumbnails = Session["file_info"] as List<Thumbnail>;
                if (thumbnails == null)
                {
                    thumbnails = new List<Thumbnail>();
                    Session["file_info"] = thumbnails;
                }
                thumbnails.Add(thumb);
                finalImage.Save("c:\\aa.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Response.StatusCode = 200;
                Response.Write(thumbnailID);
            }
            catch
            {
                // If any kind of error occurs return a 500 Internal Server error
                Response.StatusCode = 500;
                Response.Write("An error occured");
                Response.End();
            }
            finally
            {
                // Clean up
                if (finalImage != null) finalImage.Dispose();
                if (graphic != null) graphic.Dispose();
                if (originalImage != null) originalImage.Dispose();
                if (ms != null) ms.Close();
                Response.End();
            }
            return new ContentResult();
        }
        #endregion

        #region plUpload
        public JsonResult PlUpload()
        {
            HttpPostedFileBase upload = Request.Files["uploaderFile"];
            if (upload == null)
                return new NewtonsoftJsonResult()
                {
                    Data = new { status = "error", msg = "不存在上传文件！" }
                };

            var attachDir = HttpContext.Server.MapPath(CreateFolder4PlUpload());
            FileInfo fileInfo = new FileInfo(upload.FileName);
            var name = Guid.NewGuid() + fileInfo.Extension;
            var fileName = Path.Combine(attachDir, name);
            upload.SaveAs(fileName);
            var relativeFileName = CreateRelativePath(name);

            return new NewtonsoftJsonResult()
            {
                Data = new { status = "success", msg = "上传成功！", fileName = relativeFileName }
            };
        }
        private string CreateFolder4PlUpload()
        {
            var attachDir = "Attachments/";
            attachDir = System.IO.Path.Combine(HostingEnvironment.ApplicationHost.GetVirtualPath(), attachDir);
            if (!System.IO.Directory.Exists(HttpContext.Server.MapPath(attachDir)))
                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(attachDir));
            return attachDir;
        }
        private string CreateRelativePath(string fileName)
        {
            var attachDir = "Attachments/";
            attachDir = System.IO.Path.Combine(attachDir, fileName);
            return attachDir;
        }
        #endregion

       
    }

    public class Thumbnail
    {
        public Thumbnail(string id, byte[] data)
        {
            Id = id;
            Data = data;
        }
        public string Id { get; set; }
        public byte[] Data { get; set; }
    }

}
