using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Itis.Suo.App_Start;
using Security;
using Security.Interfaces.Model;
using User = Itis.Suo.Models.User;

namespace Itis.Suo.Controllers
{
    public class SecurityController : Controller
    {
        // GET: Security
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Hello()
        {
            ViewBag.Title = "Hello";
            return View();
        }

        public ActionResult GetUserList()
        {
            var users = SecurityConfig.Security.UserCollection.Select(u => new User() {Login = u.Login});
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGroupListByUser(string id)
        {
            var groups = SecurityConfig.Security.UserCollection.Include("Groups").Where(u => u.Login == id).Select(u => u.Groups).FirstOrDefault();
            if (groups == null)
                return Json(new object[] {}, JsonRequestBehavior.AllowGet);

            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRoleListByUser(string id)
        {
            using (var security = new BaseSecurity())
            {
                var roles = security.MemberCollection.Include("Roles").Where(u => u.Name == id).Select(m => m.Roles).FirstOrDefault();
                if (roles == null)
                    return Json(new object[] { }, JsonRequestBehavior.AllowGet);

                return Json(roles, JsonRequestBehavior.AllowGet);
            }
        }
    }
}