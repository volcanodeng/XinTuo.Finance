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
    public class SubjectController : Controller
    {
        private readonly IOrchardServices _services;
        private readonly ISubjectService _subjectService;

        public SubjectController(IOrchardServices services,ISubjectService subjectService)
        {
            _services = services;
            _subjectService = subjectService;
        }

        [Themed]
        public ActionResult Subject()
        {
            List<MSubjectCategory> sc = _subjectService.GetMainCategory();
            return new ShapeResult(this, _services.New.Setting_Subject(
                GenCategory: sc,
                AllCategory: _subjectService.GetAllCategory()
                ));
        }
    }
}