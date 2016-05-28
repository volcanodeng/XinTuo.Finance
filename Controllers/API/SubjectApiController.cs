using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using XinTuo.Finance.Services;
using System.Web.Http;

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
        public IHttpActionResult GetSubjectByCode(int subjectCode)
        {
            return Ok(_subjectService.GetSubjectByCode(subjectCode));
        }

        [HttpPost]
        public IHttpActionResult GetSubjectsByCategory(int categoryId)
        {
            return Ok(_subjectService.GetSubjectsByCategory(categoryId));
        }

    }
}