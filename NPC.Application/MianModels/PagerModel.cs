using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using NPC.Domain.Models;

namespace NPC.Application.MianModels
{
    public class PagerModel
    {
        public Pagination Pagination { get; set; }

        public string CurrentUrl
        {
            get { return HttpContext.Current.Request.Url.PathAndQuery; }
        }

        public string PageUrl(int pageIndex)
        {
            if (CurrentUrl != null && CurrentUrl.IndexOf("?", System.StringComparison.CurrentCultureIgnoreCase) > 0)
            {
                Regex regex = new Regex(@"([\?&]?pageIndex)=\d+", RegexOptions.IgnoreCase);
                if (regex.IsMatch(CurrentUrl))
                {
                    return regex.Replace(CurrentUrl, "$1=" + pageIndex);
                }
                return CurrentUrl + "&pageIndex=" + pageIndex;
            }

            return CurrentUrl + "?pageIndex=" + pageIndex;
        }
    }
}
