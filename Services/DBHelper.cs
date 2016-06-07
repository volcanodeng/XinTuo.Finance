using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Orchard;
using Orchard.Environment.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace XinTuo.Finance.Services
{
    public  class DBHelper : ISingletonDependency
    {
        private readonly ShellSettings _shellSettings;

        public DBHelper(ShellSettings shellSettings)
        {
            _shellSettings = shellSettings;
        }

        private static Database _database;
        public  Database GetDB()
        {
            if(_database == null)
            {
                _database = new SqlDatabase(_shellSettings.DataConnectionString);
            }

           return _database;
        }

        public  Database GetDB(string connectionstring)
        {
            if (_database == null)
            {
                _database = new SqlDatabase(connectionstring);
            }

            return _database;
        }

        public  int UpdateDatatable(DataTable dt,DbCommand cmd)
        {
            SqlDataAdapter ada = new SqlDataAdapter(cmd as SqlCommand);
            ada.SelectCommand.Connection = GetDB().CreateConnection() as SqlConnection;
            SqlCommandBuilder builder = new SqlCommandBuilder(ada);
            int res = 0;
            try
            {
                ada.InsertCommand = builder.GetInsertCommand();
                ada.UpdateCommand = builder.GetUpdateCommand();
                res = ada.Update(dt);
            }
            finally
            {
                ada.SelectCommand.Connection.Close();
            }
            return res;
        }
        public  int UpdateDatatable(DataTable dt, string cmdText)
        {
            SqlCommand cmd = _database.GetSqlStringCommand(cmdText) as SqlCommand;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.SelectCommand.Connection = GetDB().CreateConnection() as SqlConnection;
            SqlCommandBuilder builder = new SqlCommandBuilder(ada);
            int res = 0;
            try
            {
                ada.InsertCommand = builder.GetInsertCommand();
                ada.UpdateCommand = builder.GetUpdateCommand();
                res = ada.Update(dt);
            }
            finally
            {
                ada.SelectCommand.Connection.Close();
            }
            return res;
        }

        public  object ExecuteScalar(string sql)
        {
            DbCommand cmd = GetDB().GetSqlStringCommand(sql);
            return _database.ExecuteScalar(cmd);
        }

        public  DataTable ExecuteDataTable(string sql)
        {
            DbCommand cmd = GetDB().GetSqlStringCommand(sql);
            DataSet ds = _database.ExecuteDataSet(cmd);

            if(ds.Tables.Count>0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public  DataRow ExecuteDataRow(string sql)
        {
            DbCommand cmd = GetDB().GetSqlStringCommand(sql);
            DataSet ds = _database.ExecuteDataSet(cmd);

            if(ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
            {
                return ds.Tables[0].Rows[0];
            }

            return null;
        }

        public  int ExecuteNonQuery(string sql)
        {
            DbCommand cmd = GetDB().GetSqlStringCommand(sql);
            return _database.ExecuteNonQuery(cmd);
        }

       
    }

}

