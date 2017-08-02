using System.Reflection;
using System.Web.Mvc;

namespace Security.Manager.Controllers
{
    public class TemplatesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminPanel()
        {           
            return PartialView("Security/AdminPanel");
        }

        #region For User

        public ActionResult AdminPanelUsers()
        {
            return PartialView("Security/AdminPanel/_users");
        }

        public ActionResult UserProfile()
        {
            return PartialView("Security/AdminPanel/UserProfile/_UserProfile");
        }

        public ActionResult UserGroupsList()
        {
            return PartialView("Security/AdminPanel/UserProfile/_UserGroupList");
        }

        public ActionResult MemberRoleList()
        {
            return PartialView("Security/AdminPanel/MemberProfile/_MemberRoleList");
        }

        #endregion

        #region For Group

        public ActionResult AdminPanelGroups()
        {
            return PartialView("Security/AdminPanel/_groups");
        }

        public ActionResult GroupProfile()
        {
            return PartialView("Security/AdminPanel/GroupProfile/_GroupProfile");
        }

        public ActionResult GroupUsersList()
        {
            return PartialView("Security/AdminPanel/GroupProfile/_GroupUsersList");
        }

//        public ActionResult GroupRolesList()
//        {
//            return PartialView("Security/AdminPanel/GroupProfile/_GroupRolesList");
//        }

        #endregion

        #region For Role

        public ActionResult AdminPanelRoles()
        {
            return PartialView("Security/AdminPanel/_roles");
        }

        public ActionResult RoleProfile()
        {
            return PartialView("Security/AdminPanel/RoleProfile/_RoleProfile");
        }

        public ActionResult RoleMembersList()
        {
            return PartialView("Security/AdminPanel/RoleProfile/_RoleMemberList");
        }

        public ActionResult RoleGrantList()
        {
            return PartialView("Security/AdminPanel/RoleProfile/_RoleGrantList");
        }

        #endregion

        public ActionResult Log()
        {
            return PartialView("Security/Log");
        }
        public ActionResult SafeObjects()
        {
            return PartialView("Security/SafeObjects");
        }
        public ActionResult Settings()
        {
            return PartialView("Security/Settings");
        }
    }
}