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
            MCompany com = _company.GetCompanyWithCurrentUser();
            if (com != null)
            {
                DataTable dt = _dbHelper.ExecuteDataTable(string.Format("select * from [Finance_CertificateWordRecord] where [CompanyId] = '{0}'", com.CompanyId.ToString("D")));
                return Utility.Convert<MCertWord>(dt);
            }
            else
            {
                return new List<MCertWord>();
            }
        }
    }
}