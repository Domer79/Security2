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

        public ActionResult UserProfile()
        {
            return PartialView("Security/AdminPanel/_UserProfile");
        }

        public ActionResult AdminPanelGroups()
        {
            return PartialView("Security/AdminPanel/_Groups");
        }

        public ActionResult RoleList()
        {
            return PartialView("Security/AdminPanel/_RoleList");
        }
    }
}