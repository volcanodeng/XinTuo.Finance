using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using System.Data;
using XinTuo.Finance.Models;

namespace XinTuo.Finance.Services
{
    public interface ICertWordService : IDependency
    {
        List<MCertWord> GetCompanyCertWords();

        int GetCompanyNewCertWordSn();


    }

    public class CertWordService : ICertWordService
    {
        private DBHelper _dbHelper;
        private readonly ICompanyService _company;

        public CertWordService(ICompanyService company, DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
            _company = company;
        }

        public List<MCertWord> GetCompanyCertWords()
        {
            DataTable comDt = _dbHelper.ExecuteDataTable("select * from [Finance_CertificateWordRecord] where [CompanyId] is null");

            MCompany com = _company.GetCompanyWithCurrentUser();
            if (com != null)
            {
                DataTable dt = _dbHelper.ExecuteDataTable(string.Format("select * from [Finance_CertificateWordRecord] where  [CompanyId] = '{0}'", com.CompanyId.ToString("D")));

                foreach(DataRow dr in dt.Rows)
                {
                    comDt.ImportRow(dr);
                }
                comDt.AcceptChanges();
            }
            
            return Utility.Convert<MCertWord>(comDt);
            
        }

        public int GetCompanyNewCertWordSn()
        {
            MCompany com = _company.GetCompanyWithCurrentUser();
            if (com != null)
            {
                return Convert.ToInt32(_dbHelper.ExecuteScalar(string.Format("select max([CertWordSn])+1 from [Finance_VoucherRecord] where [CompanyId] = '{0}'", com.CompanyId.ToString("D"))));
            }

            return 1;
        }
    }
}