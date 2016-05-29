using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using System.Data;
using XinTuo.Finance.Models;

namespace XinTuo.Finance.Services
{
    public interface ISubjectService : IDependency
    {
        List<MSubject> GetSubjectsByCategory(int categoryId);

        MSubject GetSubjectByCode(int subjectCode);

        List<MSubject> GetSubjectsByCode(int subjectCode);

        List<MSubjectCategory> GetMainCategory();

        List<MSubjectCategory> GetAllCategory();
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

        public MSubject GetSubjectByCode(int subjectCode)
        {
            string sql = string.Format("select * from [Finance_SubjectsRecord] where [SubjectCode] = {0}",subjectCode);
            DataTable dt = _dbHelper.ExecuteDataTable(sql);

            return Utility.Convert<DataTable, List<MSubject>>(dt).FirstOrDefault();
        }

        public List<MSubject> GetSubjectsByCategory(int categoryId)
        {
            string sql = "select * from [Finance_SubjectsRecord] sr " +
                         string.Format("where [SubjectCategory]={0} ", categoryId) +
                         string.Format("or exists(select 1 from Finance_SubjectCategoryRecord where sr.[SubjectCategory]=[SubjectCategory] and [ParentSubjectCategory]={0})", categoryId);
            return Utility.Convert<MSubject>(_dbHelper.ExecuteDataTable(sql));
        }

        public List<MSubject> GetSubjectsByCode(int subjectCode)
        {
            string sql = "with subjects([SubjectCode],[ParentSubjectCode],[SubjectName]) " +
                         "as " +
                         "(" +
                         string.Format("select [SubjectCode],[ParentSubjectCode],[SubjectName] from [Finance_SubjectsRecord] where subjectcode={0} ",subjectCode) +
                         "union all " +
                         "select sr.[SubjectCode],sr.[ParentSubjectCode],sr.[SubjectName] from [Finance_SubjectsRecord] sr,subjects s where sr.[ParentSubjectCode]=s.[SubjectCode] " +
                         ") " +
                         "select * from subjects ";

            return Utility.Convert<MSubject>( _dbHelper.ExecuteDataTable(sql));
        }

        public List<MSubjectCategory> GetMainCategory()
        {
            string sql = "select * from [Finance_SubjectCategoryRecord] where [ParentSubjectCategory] is null order by [SubjectCategory]";
            return Utility.Convert<MSubjectCategory>( _dbHelper.ExecuteDataTable(sql));
        }

        public List<MSubjectCategory> GetAllCategory()
        {
            string sql = "select * from [Finance_SubjectCategoryRecord]";
            return Utility.Convert<MSubjectCategory>(_dbHelper.ExecuteDataTable(sql));
        }
    }
}