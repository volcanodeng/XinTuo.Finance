using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XinTuo.Finance.Models;
using System.Data;
using System.Data.Common;
using Orchard;
using Orchard.Users;
using AutoMapper;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace XinTuo.Finance.Services
{
    public interface ICompanyService : IDependency
    {
        List<MRegion> GetRegions(int? RegionId);

        int Save(MCompany com);

        MCompany GetCompany(Guid comId);

        MCompany GetCompanyWithCurrentUser();

        string Message { get; set; }
    }

    public class CompanyService : ICompanyService
    {
        private readonly DBHelper _dbHelper;
        private readonly Database _db;
        private readonly IWorkContextAccessor _context;

        //private readonly IMapper _mapper;
        //public CompanyService(DBHelper dbHelper,IMapper mapper)
        public CompanyService(DBHelper dbHelper,IWorkContextAccessor context)
        {
            _dbHelper = dbHelper;
            //_mapper = mapper;
            _context = context;

            _db = _dbHelper.GetDB();
        }

        public List<MRegion> GetRegions(int? RegionId)
        {
            string sql = string.Empty;

            //id没有值则返回所有省份
            if (!RegionId.HasValue)
            {
                sql = "  select [ProvinceId],[ProvinceName] from [Finance_RegionRecord] group by [ProvinceId],[ProvinceName]";
            }


            //值包含0000，说明是一个省份的id
            //返回该省份包含的城市
            if (string.IsNullOrEmpty(sql) && RegionId.HasValue && RegionId.Value.ToString().EndsWith("0000"))
            {
                sql = string.Format("  select [CityId],[CityName] from [Finance_RegionRecord] where [ProvinceId]={0} and [CityId] <> {0} group by [CityId],[CityName]", RegionId.Value);
            }

            //值以00结尾，说明是一个城市的id
            //返回该城市包含的县/城区
            if (string.IsNullOrEmpty(sql) && RegionId.HasValue && RegionId.Value.ToString().EndsWith("00"))
            {
                sql = string.Format("  select [RegionId],[CountyName] from [Finance_RegionRecord] where [CityId]={0} and [RegionId] <> {0} group by [RegionId],[CountyName]", RegionId.Value);
            }

            if(sql == string.Empty)
            {
                sql = string.Format("select * from [Finance_RegionRecord] where [RegionId] = {0}",RegionId.Value);
            }

            DataTable dt = _dbHelper.ExecuteDataTable(sql);
            return Utility.Convert<DataTable, List<MRegion>>(dt);
            //DataTableReader dr = dt.CreateDataReader();
            //if (dr.HasRows)
            //{   
            //    return _mapper.Map<IDataReader, List<MRegion>>(dr);
            //}
            //else
            //    return new List<MRegion>();
        }

        public MRegion GetRegion(int regionId)
        {
            DbCommand cmd = _db.GetSqlStringCommand("select * from [Finance_RegionRecord] where [RegionId]=@RegionId");
            _db.AddInParameter(cmd, "@RegionId", DbType.Int32, regionId);
            DataSet ds = _db.ExecuteDataSet(cmd);

            if(ds.Tables.Count>0 )
            {
                return Utility.Convert<DataTable, List<MRegion>>(ds.Tables[0]).FirstOrDefault();
            }

            return null;
        }

        public int Save(MCompany com)
        {
            string sql = string.Format("select * from [Finance_CompanyRecord] where [CompanyId] = '{0}'",com.CompanyId);
            DataTable dt = _dbHelper.ExecuteDataTable(sql);
            if(dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
                dt.Rows[0]["CompanyId"] = Guid.NewGuid().ToString("N");
            }
            else
            {
                if(dt.Rows[0]["ContactsUserAccount"].ToString() != _context.GetContext().CurrentUser.Id.ToString())
                {
                    Message = "无权修改";
                    return 0;
                }
            }
            dt.Rows[0]["ComFullName"] = com.ComFullName;
            dt.Rows[0]["ComShortName"] = com.ComShortName;
            dt.Rows[0]["RegionId"] = com.RegionId;
            dt.Rows[0]["ComAddress"] = com.ComAddress;
            dt.Rows[0]["ComTel"] = com.ComTel;
            dt.Rows[0]["ContactsName"] = com.ContactsName;
            dt.Rows[0]["ContactsMobile"] = com.ContactsMobile;
            dt.Rows[0]["ContactsEmail"] = com.ContactsEmail;
            dt.Rows[0]["ContactsUserAccount"] = _context.GetContext().CurrentUser.Id;

            int res = _dbHelper.UpdateDatatable(dt, sql);
            return res;
        }

        public MCompany GetCompany(Guid comId)
        {
            DataTable dt = _dbHelper.ExecuteDataTable(string.Format("select * from [Finance_CompanyRecord] where [CompanyId]='{0}'", comId.ToString("N")));
            if (dt.Rows.Count == 0) return new MCompany();

            MCompany com = Utility.Convert<DataTable, List<MCompany>>(dt).FirstOrDefault();
            com.Region = this.GetRegion(com.RegionId);

            return com;
        }

        public MCompany GetCompanyWithCurrentUser()
        {
            if (_context.GetContext() == null || _context.GetContext().CurrentUser == null) return new MCompany(); 

            DataTable dt = _dbHelper.ExecuteDataTable(string.Format("select * from [Finance_CompanyRecord] where [ContactsUserAccount] = {0}", _context.GetContext().CurrentUser.Id));

            if (dt.Rows.Count == 0) return new MCompany();

            MCompany com = Utility.Convert<DataTable, List<MCompany>>(dt).FirstOrDefault();
            com.Region = this.GetRegion(com.RegionId);

            return com;
        }

        public string Message
        {
            get;set;
        }
    }
}