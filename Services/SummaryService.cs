using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Orchard;
using XinTuo.Finance.Models;


namespace XinTuo.Finance.Services
{
    public interface ISummaryService : IDependency
    {
        List<MSummary> GetSummaryOfCompany();

        int SaveSummary(MSummary summary);

        int DeleteSummary(int sid);
    }

    public class SummaryService : ISummaryService
    {
        private DBHelper _dbHelper;
        private ICompanyService _company;
        private IWorkContextAccessor _context;

        public SummaryService(DBHelper dbHelper,ICompanyService company,IWorkContextAccessor context)
        {
            _dbHelper = dbHelper;
            _company = company;
            _context = context;
        }

        public int DeleteSummary(int sid)
        {
            return _dbHelper.ExecuteNonQuery(string.Format("delete from [Finance_SummaryRecord] where sid={0}",sid));
        }

        public List<MSummary> GetSummaryOfCompany()
        {
            MCompany com = _company.GetCompanyWithCurrentUser();
            if (com == null) return new List<MSummary>();

            DataTable dt = _dbHelper.ExecuteDataTable(string.Format("select * from [Finance_SummaryRecord] where company = '{0}'",com.CompanyId.ToString("N")));

            List<MSummary> summarys = Utility.Convert<MSummary>(dt);
            return summarys;
        }

        public int SaveSummary(MSummary summary)
        {
            string sql = string.Format("select * from [Finance_SummaryRecord] where sid={0}",summary.SId);
            DataTable dt = _dbHelper.ExecuteDataTable(sql);

            DataRow dr = dt.NewRow();
            if(dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }
            else
            {
                dr["CompanyId"] = summary.CompanyId;
                dr["Creator"] = _context.GetContext().CurrentUser.UserName;
                dr["CreateTime"] = DateTime.Now;
                dt.Rows.Add(dr);
            }

            dr["Summary"] = summary.Summary;
            int res = _dbHelper.UpdateDatatable(dt,sql);
            return res;
        }
    }
}