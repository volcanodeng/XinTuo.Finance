using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Themes;
using Orchard.Mvc;
using Orchard;

namespace XinTuo.Finance.Controllers
{
    public class SubjectController : Controller
    {
        private readonly IOrchardServices _services;

        public SubjectController(IOrchardServices services)
        {
            _services = services;
        }

        [Themed]
        public ActionResult Subject()
        {
            return new ShapeResult(this, _services.New.Setting_Subject());
        }
    }
}