
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XinTuo.Finance.Models
{
    public class MVoucherAbstracts
    {
        public int AId
        {
            get;set;
        }

        public string Abstracts
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