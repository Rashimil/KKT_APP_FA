using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKT_APP_FA.Services.Helpers;
using KKT_APP_FA.StaticValues;
using Microsoft.AspNetCore.Mvc;

namespace KKT_APP_FA.Controllers.View
{
    public class HomeController : Controller
    {
        ILoggerHelper logger;
        public HomeController(ILoggerHelper logger)
        {
            this.logger = logger;
        }

        // [Route("api/v4/")]

        public IActionResult Index()
        {
            return RedirectToAction("ShowInfo");
        }

        [Route("api/v4/")]
        public IActionResult ShowInfo()
        {
            // KktStaticValues.kktRegistrationReport
            //var model = KktStaticValues();
            string registretionReport = logger.BuildResponseString(KktStaticValues.kktRegistrationReport);
            ViewBag.RegistretionReportString = registretionReport;
            ViewBag.RegistretionReport = KktStaticValues.kktRegistrationReport;

            return View("Index");
        }
    }
}