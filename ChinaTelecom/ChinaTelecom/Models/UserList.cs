using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChinaTelecom.Models
{
    public class UserList
    {
        /// <summary>
        /// 公司集合
        /// </summary>
        public List<string> Company = new List<string>();
        /// <summary>
        /// 分支局集合
        /// </summary>
        public List<string> BranchOffice = new List<string>();
        public List<User> list = new List<User>();
        public int Count { get; set; }
        public int pageSize { get; set; }
    }
}