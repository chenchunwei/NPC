using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;

namespace NPC.Domain.Models.NpcMmses
{
    public class NpcMmsContent
    {
        public virtual Guid Id { get; set; }
        public virtual string Content { get; set; }
        public virtual string UrlOfVoice { get; set; }
        public virtual string UrlOfPic { get; set; }
        public virtual int DueTime { get; set; }
        public virtual LayoutType LayoutType { get; set; }
        public virtual int ByteSize { get; set; }
        public virtual int OrderSort { get; set; }

        public virtual int CalculateSize(string baseDirectory)
        {
            int size = 0;
            if (!string.IsNullOrEmpty(UrlOfVoice))
            {
                var path = System.IO.Path.Combine(baseDirectory, UrlOfVoice.TrimStart(new[] { '/', '\\' }));
                if (File.Exists(path))
                {
                    size += (int)(new FileInfo(path).Length);
                }
            }
            if (!string.IsNullOrEmpty(UrlOfPic))
            {
                var path = System.IO.Path.Combine(baseDirectory, UrlOfPic.TrimStart(new []{'/','\\'}));
                if (File.Exists(path))
                {
                    size += (int)(new FileInfo(path).Length);
                }
            }
            if (!string.IsNullOrEmpty(Content))
            {
                size += System.Text.Encoding.GetEncoding("GB2312").GetBytes(Content).Length;
            }
            return size;
        }
    }
}
