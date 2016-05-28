using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using XinTuo.Finance.Services;
using XinTuo.Finance.Models;

namespace XinTuo.Finance.Controllers.API
{
    public class CompanyApiController : ApiController
    {
        private readonly ICompanyService _companyService;

        public CompanyApiController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        [HttpGet]
        [ActionName("GetRegion")]
        public IHttpActionResult GetRegion(int id)
        {
            return Ok(_companyService.GetRegions(id));
        }

        [HttpGet]
        [ActionName("GetProvinces")]
        public IHttpActionResult GetRegion()
        {
            return Ok(_companyService.GetRegions(null));
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody]MCompany com)
        {
            if (com == null) return BadRequest("不能保存空内容");
            
            return Ok(_companyService.Save(com));
        }
    }
}