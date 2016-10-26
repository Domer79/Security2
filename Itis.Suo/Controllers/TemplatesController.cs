using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Itis.Suo.Controllers
{
    public class TemplatesController : Controller
    {
        public ActionResult AdminPanel()
        {
            return PartialView("Security/AdminPanel");
        }

        public ActionResult AdminPanelUsers()
        {
            return PartialView("Security/AdminPanel/_users");
        }

        public ActionResult UserDetails()
        {
            return PartialView("Security/AdminPanel/_UserDetails");
        }
    }
}