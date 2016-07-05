using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using XinTuo.Finance.Services;
using XinTuo.Finance.Models;


namespace XinTuo.Finance.Controllers.API
{
    public class SubjectApiController : ApiController
    {
        private readonly ISubjectService _subjectService;

        public SubjectApiController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpPost]
        public IHttpActionResult GetSubjectByCode([FromBody]MSubject subject)
        {
            return Ok(_subjectService.GetSubjectByCode(subject.SubjectCode));
        }

        [HttpPost]
        public IHttpActionResult GetSubjectsByCategory([FromBody]MSubjectCategory cate)
        {
            return Ok(_subjectService.GetSubjectsByCategory(cate.SubjectCategory));
        }

        [HttpGet]
        [HttpPost]
        public IHttpActionResult GetAllCategory()
        {
            return Ok(_subjectService.GetAllCategory());
        }

        [HttpGet]
        [HttpPost]
        public IHttpActionResult GetComSubjectsForTree()
        {
            return Ok(_subjectService.GetCompanySubjectsForTree());
        }

        [HttpGet]
        [HttpPost]
        public IHttpActionResult GetSelectableCategory()
        {
            return Ok(_subjectService.GetCategorySelectable());
        }

        [HttpPost]
        public IHttpActionResult SaveSubject([FromBody]MSubject subject)
        {
            return Ok(_subjectService.SaveSubject(subject));
        }

        [HttpPost]
        public IHttpActionResult GetCommonAuxAcc()
        {
            return Ok(_subjectService.GetCommonAuxAcc());
        }

        [HttpPost]
        public IHttpActionResult GetCurCompanyAuxAcc()
        {
            return Ok(_subjectService.GetCurrentCompanyAuxAcc());
        }
    }
}