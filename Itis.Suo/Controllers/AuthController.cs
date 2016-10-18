using System;
using System.Collections.Generic; 
using System.Linq; 
using System.Web; 
using System.Web.Mvc;
using System.Web.Security;
 
namespace Itis.Suo.Controllers 
{ 
    public class AuthController : Controller
    { 
        public ActionResult Login()
        { 
            return View(); 
        }


        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            if (true)
            {
                FormsAuthentication.SetAuthCookie(login, true);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    } 
} 