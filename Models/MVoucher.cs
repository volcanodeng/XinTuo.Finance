﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XinTuo.Finance.Models
{
    public class MVoucher
    {
        public int VId
        {
            get;set;
        }

        public string CertWord
        {
            get;set;
        }

        public int CertWordSn
        {
            get;set;
        }

        /// <summary>
        /// 暂不使用
        /// </summary>
        //public string PaymentTerms
        //{
        //    get;set;
        //}

        public DateTime VoucherTime
        {
            get;set;
        }

        public int AttachedInvoices
        {
            get;set;
        }

        public Guid CompanyId
        {
            get;set;
        }

        public VoucherStatus Status
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

        public string Reviewer
        {
            get;set;
        }

        public DateTime? ReviewTime
        {
            get;set;
        }

        public MVoucherDetail[] VoucherDetails
        {
            get;set;
        }

    }

    public enum VoucherStatus
    {
        /// <summary>
        /// 已保存
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 已审核
        /// </summary>
        Review = 2,
        /// <summary>
        /// 反审核
        /// </summary>
        AntiAudit =3
    }
}