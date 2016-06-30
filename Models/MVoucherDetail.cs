using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XinTuo.Finance.Models
{
    public class MVoucherDetail
    {
        public int VdId
        {
            get; set;
        }

        public int VId
        {
            get; set;
        }

        public string Abstracts
        {
            get; set;
        }

        public int SubjectCode
        {
            get; set;
        }

        public decimal Debit
        {
            get; set;
        }

        public decimal Credit
        {
            get;set;
        }

        public int? Quantity
        {
            get;set;
        }

        public MVoucher Voucher
        {
            get;set;
        }
    }
}