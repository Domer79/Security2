using System.Web.Mvc;
using System.Web.Security;
using Security.Manager.Models;
using System.Reflection;

namespace Security.Manager.Controllers 
{ 
    public class AuthController : BaseSecurityController
    {
        private const string ENTER_ERROR_MRSSAGE = "Не удалось произвести вход. Возможно, учетная запись неактивна, или были указаны неверные логин и/или пароль.";
        public ActionResult Login()
        {
            ViewBag.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return View(ViewBag); 
        }


        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            var model = new LoginModel{ IsSuccessfull = false, ErrorMessage = ENTER_ERROR_MRSSAGE, Url = string.Empty };
            using (var security = new CoreSecurity())
            {
                if (security.LogIn(login, password))
                {
                    FormsAuthentication.SetAuthCookie(login, true);                    
                    model.ErrorMessage = string.Empty;
                    model.IsSuccessfull = true;
                    model.Url = "/Main/Index";
                    return JsonByNewtonsoft(model);                    
                }
               
               return JsonByNewtonsoft(model);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    } 
} 