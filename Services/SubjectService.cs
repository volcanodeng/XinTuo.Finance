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
    }

    public class SubjectService : ISubjectService
    {
        public DataRow GetSubjectByCode(int subjectCode)
        {
            throw new NotImplementedException();
        }

        public DataTable GetSubjectsByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}