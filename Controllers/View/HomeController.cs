using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KKT_APP_FA.Controllers.View
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}