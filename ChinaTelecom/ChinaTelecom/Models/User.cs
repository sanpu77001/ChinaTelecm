using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChinaTelecom.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Company { get; set; }
        public string BranchOffice { get; set; }
        public string Worknumber { get; set; }
        public string UserName { get; set; }
        public Nullable<int> State { get; set; }
        public string stateMsg { get; set; }
        public int pageSize { get; set; }
        public int Count { get; set; }
    }
}