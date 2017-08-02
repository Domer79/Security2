using System.Web.Mvc;
using Security.Extensions;

namespace Security.Manager.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return Redirect($"/{this.GetSecurityInfo().ApplicationName}/Security/Index");
        }
    }
}