﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Orchard;
using XinTuo.Finance.Models;

namespace XinTuo.Finance.Services
{
    public interface IVoucherService : IDependency
    {
        MVoucher GetVoucher(int vid);

        MVoucher[] GetVouchers(Guid companyId);

        MVoucher GetVoucher(Guid companyId, string certWord, int certWordSn);

        int SaveVoucher(MVoucher voucher);
    }


    public class VoucherService : IVoucherService
    {
        private DBHelper _dbHelper;
        private IWorkContextAccessor _context;

        public VoucherService(DBHelper dbHelper,IWorkContextAccessor context)
        {
            _dbHelper = dbHelper;
            _context = context;
        }

        #region 私有方法

        private MVoucher BindVoucherDetail(MVoucher voucher)
        {
            string sql = string.Format("select * from [Finance_VoucherDetailRecord] where [VId] = {0}",voucher.VId);
            DataTable dt = _dbHelper.ExecuteDataTable(sql);

            List<MVoucherDetail> detail = Utility.Convert<MVoucherDetail>(dt);
            voucher.VoucherDetails = detail.ToArray();

            return voucher;
        }

        #endregion

        public MVoucher[] GetVouchers(Guid companyId)
        {
            string sql = string.Format("select * from [Finance_VoucherRecord] where [CompanyId]='{0}' ", companyId.ToString("N"));
            DataTable dt = _dbHelper.ExecuteDataTable(sql);

            List<MVoucher> vouchers = Utility.Convert<DataTable, List<MVoucher>>(dt);
            foreach(MVoucher v in vouchers)
            {
                BindVoucherDetail(v);
            }
            return vouchers.ToArray();
        }

        public MVoucher GetVoucher(int vid)
        {
            string sql = string.Format("select * from [Finance_VoucherRecord] where [VId] = {0}",vid);
            DataTable dt = _dbHelper.ExecuteDataTable(sql);

            List<MVoucher> vouchers = Utility.Convert<DataTable, List<MVoucher>>(dt);
            if(vouchers.Count>0)
            {
                BindVoucherDetail(vouchers.First());
            }
            return vouchers.FirstOrDefault();
        }

        public MVoucher GetVoucher(Guid companyId,string certWord, int certWordSn)
        {
            if (certWord.Length > 2) throw new FormatException("凭证字格式无效");

            string sql = string.Format("select * from [Finance_VoucherRecord] where [CompanyId]='{0}' and [CertWord]='{1}' and [CertWordSn]={2}", companyId.ToString("N"), certWord, certWordSn);
            DataTable dt = _dbHelper.ExecuteDataTable(sql);

            List<MVoucher> vouchers = Utility.Convert<DataTable, List<MVoucher>>(dt);
            if (vouchers.Count > 0)
            {
                BindVoucherDetail(vouchers.First());
            }
            return vouchers.FirstOrDefault();
        }

        public int SaveVoucher(MVoucher voucher)
        {
            string sql = string.Format("select * from [Finance_VoucherRecord] where [VId] = {0}",voucher.VId);
            DataTable dt = _dbHelper.ExecuteDataTable(sql);

            DataRow dr = dt.NewRow();
            if(dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }
            else
            {
                dr["CompanyId"] = voucher.CompanyId;
                dr["Creator"] = _context.GetContext().CurrentUser.UserName;
                dr["CreateTime"] = DateTime.Now;

                dt.Rows.Add(dr);
            }

            dr["CertWord"] = voucher.CertWord;
            dr["CertWordSn"] = voucher.CertWordSn;
            dr["VoucherTime"] = voucher.VoucherTime;
            dr["AttachedInvoices"] = voucher.AttachedInvoices;

            int res = _dbHelper.UpdateDatatable(dt, sql);

            if(res > 0)
            {
                res += SaveVoucherDetail(Convert.ToInt32(dr["vid"]), voucher.VoucherDetails);
            }

            return res;
        }

        private int SaveVoucherDetail(int vid,MVoucherDetail[] details)
        {
            string sql = string.Format("select * from [Finance_VoucherDetailRecord] where [VId]={0}",vid);
            DataTable dt = _dbHelper.ExecuteDataTable(sql);

            foreach(MVoucherDetail vd in details)
            {
                DataRow dr = dt.AsEnumerable().Where(r => r.Field<int>("VdId") == vd.VdId).FirstOrDefault();
                if(dr == null)
                {
                    dr = dt.NewRow();
                    dr["VId"] = vid;
                    dt.Rows.Add(dr);
                }

                dr["Abstracts"] = vd.Abstracts;
                dr["SubjectCode"] = vd.SubjectCode;
                dr["Debit"] = vd.Debit;
                dr["Credit"] = vd.Credit;
                
            }

            int res = _dbHelper.UpdateDatatable(dt, sql);
            return res;
        }
    }
}