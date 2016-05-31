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

        

    }
}