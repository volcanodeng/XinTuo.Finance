﻿using System;
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

        List<MSubject> GetSubjectsForCompany();

        List<MEasyuiTree> GetCompanySubjectsForTree();

        int SaveSubject(MSubject subject);

        List<MSubjectCategory> GetMainCategory();

        List<MSubjectCategory> GetAllCategory();

        List<MSubjectCategory> GetCategorySelectable();

        List<MAuxAccounting> GetCommonAuxAcc();

        List<MAuxAccounting> GetCompanyAuxAcc(Guid comId);

        List<MAuxAccounting> GetCurrentCompanyAuxAcc();
    }

    public class SubjectService : ISubjectService
    {
        private readonly DBHelper _dbHelper;
        private readonly ICompanyService _company;

        private MCompany _curCompany;

        public SubjectService(DBHelper dbHelper,ICompanyService company)
        {
            _dbHelper = dbHelper;
            _company = company;

            _curCompany = _company.GetCompanyWithCurrentUser();
        }

        #region private
        private List<MSubject> BindCategory(List<MSubject> subjects)
        {
            List<MSubjectCategory> cate = GetAllCategory();
            subjects.ForEach(s => s.Category = cate.Where(c => c.SubjectCategory == s.SubjectCategory).FirstOrDefault());
            return subjects;
        }

        private int SubjectLevel(int subjectCode)
        {
            switch(subjectCode.ToString().Length)
            {
                case 4:
                    return 1;
                case 6:
                    return 2;
                case 8:
                    return 3;
                case 10:
                    return 4;
                default:
                    return 1;
            }
        }

        private MEasyuiTree BuildSubjectTree(MSubject[] subjects,MEasyuiTree tree,MSubject[] source)
        {
            foreach(MSubject s in subjects)
            {
                MEasyuiTree node = new MEasyuiTree();
                node.id = s.SubjectCode;
                node.text = string.Format("{0} {1}", s.SubjectCode, s.SubjectName);
                node.state = "open";
                tree.children.Add(node);

                MSubject[] sArr = source.Where(src => src.ParentSubjectCode == s.SubjectCode).ToArray();
                if (sArr.Length > 0) BuildSubjectTree(sArr, node, source);
            }

            return tree;
        }

        #endregion

        public MSubject GetSubjectByCode(int subjectCode)
        {
            string sql = string.Format("select * from [Finance_SubjectsRecord] where [SubjectCode] = {0} and [CompanyId] = '{1}' ", subjectCode,_curCompany.CompanyId);
            DataTable dt = _dbHelper.ExecuteDataTable(sql);

            List<MSubject> subjects = Utility.Convert<DataTable, List<MSubject>>(dt);
            return BindCategory(subjects).FirstOrDefault();
        }

        public List<MSubject> GetSubjectsByCategory(int categoryId)
        {
            string sql = "select * from [Finance_SubjectsRecord] sr " +
                         string.Format("where ([SubjectCategory]={0} ", categoryId) +
                         string.Format("or exists(select 1 from Finance_SubjectCategoryRecord where sr.[SubjectCategory]=[SubjectCategory] and [ParentSubjectCategory]={0})) and [CompanyId] = '{1}' ", categoryId,_curCompany.CompanyId)+
                         " order by cast(subjectcode as nvarchar(50))";
            List<MSubject> subjects = Utility.Convert<MSubject>(_dbHelper.ExecuteDataTable(sql));
            return BindCategory(subjects);
        }

        public List<MSubject> GetSubjectsByCode(int subjectCode)
        {
            string sql = "with subjects([SubjectCode],[ParentSubjectCode],[SubjectName]) " +
                         "as " +
                         "(" +
                         string.Format("select [SubjectCode],[ParentSubjectCode],[SubjectName] from [Finance_SubjectsRecord] where subjectcode={0} and [CompanyId] = '{1}' ", subjectCode,_curCompany.CompanyId.ToString()) +
                         "union all " +
                         string.Format( "select sr.[SubjectCode],sr.[ParentSubjectCode],sr.[SubjectName] from [Finance_SubjectsRecord] sr,subjects s where sr.[ParentSubjectCode]=s.[SubjectCode] and [CompanyId] = '{0}'  ",_curCompany.CompanyId) +
                         ") " +
                         "select * from subjects ";

            List<MSubject> subjects = Utility.Convert<MSubject>( _dbHelper.ExecuteDataTable(sql));
            return BindCategory(subjects);
        }

        public int SaveSubject(MSubject subject)
        {
            MCompany com = _company.GetCompanyWithCurrentUser();
            string sql = string.Format("select * from [Finance_SubjectsRecord] where [SubjectCode] = {0} and [CompanyId] = '{1}'", subject.SubjectCode,com.CompanyId.ToString());
            DataTable dt = _dbHelper.ExecuteDataTable(sql);
            DataRow dr;
            if (dt.Rows.Count == 0)
            {

                dr = dt.NewRow();
                dr["SubjectCode"] = subject.SubjectCode;
                dr["CompanyId"] = com.CompanyId;
                subject.SubjectState = 1;
                dt.Rows.Add(dr);
            }
            else
            {
                dr = dt.Rows[0];
            }
            if (subject.ParentSubjectCode.HasValue)
                dr["ParentSubjectCode"] = subject.ParentSubjectCode;
            else
                dr["ParentSubjectCode"] = DBNull.Value;
            dr["Level"] = SubjectLevel(subject.SubjectCode);
            dr["SubjectName"] = subject.SubjectName;
            dr["SubjectCategory"] = subject.SubjectCategory;
            dr["BalanceDirection"] = subject.BalanceDirection;
            dr["BeginBalance"] = subject.BeginBalance;
            dr["EndBalance"] = subject.EndBalance;
            dr["SubjectState"] = subject.SubjectState;

            dr["NamePath"] = dr["NamePath"].ToString() + "," + subject.SubjectName;
            dr["CodePath"] = dr["CodePath"].ToString() + "," + subject.SubjectCode; 

            int res = _dbHelper.UpdateDatatable(dt, sql);
            return res;
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

        public List<MSubjectCategory> GetCategorySelectable()
        {
            string sql = "select * from [Finance_SubjectCategoryRecord] where [ParentSubjectCategory] in (select [SubjectCategory] from [Finance_SubjectCategoryRecord] where [ParentSubjectCategory] is null)";
            return Utility.Convert<MSubjectCategory>(_dbHelper.ExecuteDataTable(sql));
        }

        public List<MAuxAccounting> GetCommonAuxAcc()
        {
            string sql = "select * from [Finance_AuxiliaryAccountingRecord] where [CompanyId] is null and creator='admin'";
            return Utility.Convert<MAuxAccounting>(_dbHelper.ExecuteDataTable(sql));
        }

        public List<MAuxAccounting> GetCompanyAuxAcc(Guid comId)
        {
            string sql = string.Format("select * from [Finance_AuxiliaryAccountingRecord] where [CompanyId] = '{0}'",comId.ToString("D"));
            return Utility.Convert<MAuxAccounting>(_dbHelper.ExecuteDataTable(sql));
        }

        public List<MAuxAccounting> GetCurrentCompanyAuxAcc()
        {
            MCompany curCom = _company.GetCompanyWithCurrentUser();
            if (curCom != null)
                return this.GetCompanyAuxAcc(curCom.CompanyId);
            else
                return new List<MAuxAccounting>();
        }

        public List<MEasyuiTree> GetCompanySubjectsForTree()
        {
            MEasyuiTree root = new MEasyuiTree();
            List<MSubject> subjects = GetSubjectsForCompany();
            MSubject[] sArr = subjects.Where(s => !s.ParentSubjectCode.HasValue).ToArray();

            this.BuildSubjectTree(sArr, root, subjects.ToArray());

            return root.children;
        }

        public List<MSubject> GetSubjectsForCompany()
        {
            MCompany com = _company.GetCompanyWithCurrentUser();
            if(com!=null)
            {
                DataTable dt = _dbHelper.ExecuteDataTable(string.Format("select * from [Finance_SubjectsRecord] where [CompanyId]='{0}'",com.CompanyId.ToString("D")));
                return Utility.Convert<MSubject>(dt);
            }

            return new List<MSubject>();
        }
    }
}