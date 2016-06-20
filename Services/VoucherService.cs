using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XinTuo.Finance.Models;

namespace XinTuo.Finance.Services
{
    public interface IVoucherService
    {
        MVoucher GetVoucher(int vid);

        MVoucher[] GetVouchers(Guid companyId);

        MVoucher GetVoucher(Guid companyId, string certWord, int certWordSn);
    }


    public class VoucherService : IVoucherService
    {
        private DBHelper _dbHelper;

        public VoucherService(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
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
    }
}