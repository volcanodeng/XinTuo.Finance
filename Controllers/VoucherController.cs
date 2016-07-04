using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Themes;
using Orchard.Mvc;
using Orchard;
using XinTuo.Finance.Services;
using XinTuo.Finance.Models;

namespace XinTuo.Finance.Controllers
{
    public class VoucherController : Controller
    {
        private readonly IOrchardServices _services;
        private readonly IVoucherService _voucher;
        private readonly ICertWordService _certWord;

        public VoucherController(IOrchardServices services,IVoucherService voucherService, ICertWordService certWord)
        {
            _services = services;
            _voucher = voucherService;
            _certWord = certWord;
        }

        [Themed]
        public ActionResult Voucher()
        {
            int newCertWordSn = _certWord.GetCompanyNewCertWordSn();
            return new ShapeResult(this,_services.New.Voucher_Entry(
                NewCertWordSn : newCertWordSn
                ));
        }
    }
}