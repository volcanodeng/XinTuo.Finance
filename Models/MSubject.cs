using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XinTuo.Finance.Models
{
    public class MSubject
    {
        public int SubjectCode
        {
            get;set;
        }

        public int ParentSubjectCode
        {
            get;set;
        }

        public int Level
        {
            get;set;
        }

        public string SubjectName
        {
            get;set;
        }

        public int SubjectCategory
        {
            get;set;
        }

        public MSubjectCategory Category
        {
            get;set;
        }

        public int CompanyId
        {
            get;set;
        }

        public string BalanceDirection
        {
            get;set;
        }

        public decimal BeginBalance
        {
            get;set;
        }

        public decimal EndBalance
        {
            get;set;
        }

        public int SubjectState
        {
            get;set;
        }

        public string NamePath
        {
            get;set;
        }


        public string CodePath
        {
            get;set;
        }

        public decimal Debit
        {
            get;set;
        }

        public decimal Credit
        {
            get;set;
        }

        public decimal SubjectAmount
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

        public string LastUpdate
        {
            get;set;
        }

        public DateTime LastUpdateTime
        {
            get;set;
        }
    }
}