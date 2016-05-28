using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using System.Data;

namespace XinTuo.Finance.Services
{
    public interface ISubjectService : IDependency
    {
        DataTable GetSubjectsByCategory(int categoryId);

        DataRow GetSubjectByCode(int subjectCode);

        DataTable GetSubjectsByCode(int subjectCode);
    }

    public class SubjectService : ISubjectService
    {
        private readonly DBHelper _dbHelper;
        private readonly IWorkContextAccessor _context;

        public SubjectService(DBHelper dbHelper, IWorkContextAccessor context)
        {
            _dbHelper = dbHelper;
            _context = context;
        }

        public DataRow GetSubjectByCode(int subjectCode)
        {
            string sql = string.Format("select * from [Finance_SubjectsRecord] where [SubjectCode] = {0}",subjectCode);
            return _dbHelper.ExecuteDataRow(sql);
        }

        public DataTable GetSubjectsByCategory(int categoryId)
        {
            string sql = "select * from [Finance_SubjectsRecord] sr " +
                         string.Format("where [SubjectCategory]={0} ", categoryId) +
                         string.Format("or exists(select 1 from Finance_SubjectCategoryRecord where sr.[SubjectCategory]=[SubjectCategory] and [ParentSubjectCategory]={0})", categoryId);
            return _dbHelper.ExecuteDataTable(sql);
        }

        public DataTable GetSubjectsByCode(int subjectCode)
        {
            string sql = "with subjects([SubjectCode],[ParentSubjectCode],[SubjectName]) " +
                         "as " +
                         "(" +
                         string.Format("select [SubjectCode],[ParentSubjectCode],[SubjectName] from [Finance_SubjectsRecord] where subjectcode={0} ",subjectCode) +
                         "union all " +
                         "select sr.[SubjectCode],sr.[ParentSubjectCode],sr.[SubjectName] from [Finance_SubjectsRecord] sr,subjects s where sr.[ParentSubjectCode]=s.[SubjectCode] " +
                         ") " +
                         "select * from subjects ";

            return _dbHelper.ExecuteDataTable(sql);
        }
    }
}