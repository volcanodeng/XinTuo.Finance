using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XinTuo.Finance.Models
{
    public class MSubjectCategory
    {
        public int SubjectCategory
        {
            get;set;
        }

        public int? ParentSubjectCategory
        {
            get;set;
        }

        public string CategoryFullName
        {
            get;set;
        }

        public string CategoryShortName
        {
            get;set;
        }

    }
}