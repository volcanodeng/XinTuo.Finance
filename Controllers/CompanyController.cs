using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Themes;
using Orchard.Mvc;
using Orchard;
using Orchard.Localization;
using XinTuo.Finance.Services;
using XinTuo.Finance.Models;
using System.Data;

namespace XinTuo.Finance.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IOrchardServices _services;
        private readonly ICompanyService _company;
        private readonly IWorkContextAccessor _context;

        public CompanyController(IOrchardServices services, ICompanyService company,IWorkContextAccessor context)
        {
            _services = services;
            _company = company;
            _context = context;
        }

        [Themed]
        public ActionResult Company()
        {
            if(_context.GetContext()== null || _context.GetContext().CurrentUser== null || _context.GetContext().HttpContext.Request.LogonUserIdentity.IsAnonymous)
            {
                return new HttpUnauthorizedResult();
            }

            MCompany com = _company.GetCompanyWithCurrentUser();

            return new ShapeResult(this, _services.New.Regist_Company(
                Company: com
                ));

        }

        public Localizer T { get; set; }
    }
}