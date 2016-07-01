using System;
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

        MVoucher[] GetVouchers();

        MVoucher GetVoucher(Guid companyId, string certWord, int certWordSn);

        int SaveVoucher(MVoucher voucher);


        List<MVoucherAbstracts> GetCompanyVoucherAbstracts();

        int SaveVoucherAbstracts(MVoucherAbstracts abstracts);

        int DeleteVoucherAbstracts(MVoucherAbstracts abstracts);
    }


    public class VoucherService : IVoucherService
    {
        private DBHelper _dbHelper;
        private IWorkContextAccessor _context;
        private ICompanyService _company;

        public VoucherService(DBHelper dbHelper,IWorkContextAccessor context,ICompanyService company)
        {
            _dbHelper = dbHelper;
            _context = context;
            _company = company;
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
            string sql = string.Format("select * from [Finance_VoucherRecord] where [CompanyId]='{0}' ", companyId.ToString("D"));
            DataTable dt = _dbHelper.ExecuteDataTable(sql);

            List<MVoucher> vouchers = Utility.Convert<DataTable, List<MVoucher>>(dt);
            foreach(MVoucher v in vouchers)
            {
                BindVoucherDetail(v);
            }
            return vouchers.ToArray();
        }

        public MVoucher[] GetVouchers()
        {
            MCompany com = _company.GetCompanyWithCurrentUser();
            if (com != null) return GetVouchers(com.CompanyId);
            else
                return new MVoucher[] { };
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

            string sql = string.Format("select * from [Finance_VoucherRecord] where [CompanyId]='{0}' and [CertWord]='{1}' and [CertWordSn]={2}", companyId.ToString("D"), certWord, certWordSn);
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

            object newId = null;
            DataRow dr = dt.NewRow();
            if(dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];

                newId = Convert.ToInt32(dr["VId"]);
            }
            else
            {
                MCompany com = _company.GetCompanyWithCurrentUser();
                dr["CompanyId"] = com.CompanyId;
                dr["Creator"] = _context.GetContext().CurrentUser.UserName;
                dr["CreateTime"] = DateTime.Now;

                dt.Rows.Add(dr);
            }

            dr["CertWord"] = voucher.CertWord;
            dr["CertWordSn"] = voucher.CertWordSn;
            dr["VoucherTime"] = voucher.VoucherTime;
            dr["AttachedInvoices"] = voucher.AttachedInvoices;

            int res = _dbHelper.UpdateDatatable(dt, sql);
            if (newId == null) newId = _dbHelper.ExecuteScalar("SELECT IDENT_CURRENT('Finance_VoucherRecord')");

            if(res > 0)
            {
                res += SaveVoucherDetail(Convert.ToInt32(newId), voucher.VoucherDetails);
            }

            return res;
        }

        private int SaveVoucherDetail(int vid,MVoucherDetail[] details)
        {
            string sql = string.Format("select * from [Finance_VoucherDetailRecord] where [VId]={0}",vid);
            DataTable dt = _dbHelper.ExecuteDataTable(sql);
            int dtRowCount = dt.Rows.Count;
            DataRow dr;
            foreach (MVoucherDetail vd in details)
            {
                if (dtRowCount == 0 || (dr = dt.AsEnumerable().Where(r => r.Field<int>("VdId") == vd.VdId).FirstOrDefault()) == null)
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

        public List<MVoucherAbstracts> GetCompanyVoucherAbstracts()
        {
            MCompany com = _company.GetCompanyWithCurrentUser();
            if (com == null) return new List<MVoucherAbstracts>();

            DataTable dt = _dbHelper.ExecuteDataTable(string.Format("select * from [Finance_VoucherAbstractsRecord] where [CompanyId] = '{0}'",com.CompanyId.ToString("D")));
            return Utility.Convert<MVoucherAbstracts>(dt);
        }

        public int SaveVoucherAbstracts(MVoucherAbstracts abstracts)
        {
            if (abstracts == null) return 0;

            string sql = string.Format("select * from [Finance_VoucherAbstractsRecord] where [AId] = {0}",abstracts.AId);
            DataTable dt = _dbHelper.ExecuteDataTable(sql);

            DataRow dr;
            if(dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }
            else
            {
                MCompany com = _company.GetCompanyWithCurrentUser();
                if (com == null) return -1;

                dr = dt.NewRow();
                dr["CompanyId"] = com.CompanyId;
                dr["Creator"] = _context.GetContext().CurrentUser.UserName;
                dr["CreateTime"] = DateTime.Now;
                dt.Rows.Add(dr);
            }

            dr["Abstracts"] = abstracts.Abstracts;

            return _dbHelper.UpdateDatatable(dt, sql);
        }

        public int DeleteVoucherAbstracts(MVoucherAbstracts abstracts)
        {
            if (abstracts == null || abstracts.AId <=0) return 0;

            return _dbHelper.ExecuteNonQuery(string.Format("delete from [Finance_VoucherAbstractsRecord] where [AId] = {0}",abstracts.AId));
        }
    }
}