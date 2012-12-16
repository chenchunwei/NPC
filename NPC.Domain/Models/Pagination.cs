using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models
{
    public class Pagination
    {
        public Pagination()
        {
            PageIndex = 1;
            PageSize = 20;
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录大小
        /// </summary>
        public int TotalRecordsCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get { return (int)Math.Ceiling((double)TotalRecordsCount / PageSize); }
        }

        public bool IsFirstpage
        {
            get { return PageIndex == 1; }
        }

        public bool IsLastPage
        {
            get { return PageSize * PageIndex >= TotalRecordsCount; }
        }

    }
}