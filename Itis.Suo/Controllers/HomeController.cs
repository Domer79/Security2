using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Itis.Suo.Attributes;

namespace Itis.Suo.Controllers
{
    public class HomeController : Controller
    {
        [SecurityAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}