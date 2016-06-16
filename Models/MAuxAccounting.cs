﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XinTuo.Finance.Models
{
    public class MAuxAccounting
    {
        public int AId
        {
            get;set;
        }

        public string AuxName
        {
            get;set;
        }

        public Nullable<Guid> CompanyId
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