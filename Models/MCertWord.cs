using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XinTuo.Finance.Models
{
    public class MCertWord
    {
        public int CwId
        {
            get;set;
        }


        public string CertificateWord
        {
            get;set;
        }

        public Guid? CompanyId
        {
            get;set;
        }
    }
}