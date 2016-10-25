using System.Linq;
using System.Web.Mvc;
using Itis.Suo.App_Start;
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
    }
}