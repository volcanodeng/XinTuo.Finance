using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace XinTuo.Finance.Models
{
    public class MCompany
    {
        

        public Guid CompanyId
        {
            get;set;
        }

        public string ComFullName
        {
            get;set;
        }


        public string ComShortName
        {
            get;set;
        }

        public int RegionId
        {
            get;set;
        }

        public MRegion Region
        {
            get;
            set;
        }

        public string ComAddress
        {
            get;set;
        }

        public string ComTel
        {
            get;set;
        }

        public string ContactsName
        {
            get;set;
        }

        public string ContactsMobile
        {
            get;set;
        }

        public string ContactsEmail
        {
            get;set;
        }

        public string ContactsUserAccount
        {
            get;set;
        }
    }
}