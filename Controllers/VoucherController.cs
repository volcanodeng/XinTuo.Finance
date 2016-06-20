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

        public VoucherController(IOrchardServices services,IVoucherService voucherService)
        {
            _services = services;
            _voucher = voucherService;
        }

        [Themed]
        public ActionResult Voucher()
        {
            return new ShapeResult(this,_services.New.Voucher_Entry());
        }
    }
}