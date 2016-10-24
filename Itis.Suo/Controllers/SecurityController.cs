using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Itis.Suo.Models;

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
            var list = new List<User>()
            {
                new User {Login = "user1"},
                new User {Login = "user2"},
                new User {Login = "user3"},
                new User {Login = "user4"},
            };

            return Json(list);
        }
    }
}