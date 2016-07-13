using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using XinTuo.Finance.Services;
using XinTuo.Finance.Models;

namespace XinTuo.Finance.Controllers.API
{
    public class VoucherApiController : ApiController
    {
        private readonly IVoucherService _voucherService;
        private readonly ICertWordService _certWordService;

        public VoucherApiController(IVoucherService voucherService,ICertWordService certWordService)
        {
            _voucherService = voucherService;
            _certWordService = certWordService;
        }

        [HttpPost]
        public IHttpActionResult SaveVoucher(MVoucher voucher)
        {
            return Ok(_voucherService.SaveVoucher(voucher));
        }

        [HttpPost]
        [HttpGet]
        public IHttpActionResult GetCertWords()
        {
            return Ok(_certWordService.GetCompanyCertWords());
        }

        [HttpPost]
        [HttpGet]
        public IHttpActionResult GetComVouchers()
        {
            return Ok(_voucherService.GetVouchers());
        }

        [HttpGet]
        public IHttpActionResult GetNewCertWordSn()
        {
            return Ok(_certWordService.GetCompanyNewCertWordSn());
        }
    }
}