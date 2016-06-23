using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XinTuo.Finance.Models
{
    public class MSummary
    {
        public int SId
        {
            get;set;
        }

        public string Summary
        {
            get;set;
        }

        public Guid CompanyId
        {
            get;set;
        }

        public string Creator
        {
            get;set;
        }

        public DateTime CreateTime
        {
            get;set;
        }
    }
}