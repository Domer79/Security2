using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Security.Manager.Models.Security;
using Security.Manager.Attributes;
using Security.Model;
using Group = Security.Model.Group;
using System.Reflection;
using Security.Manager.App_Start;

namespace Security.Manager.Controllers
{
    public class SecurityController : BaseSecurityController
    {

        [SecurityAuthorize("Security. Index")]
        public ActionResult Index()
        {
            ViewBag.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ViewBag.User = HttpContext.User.Identity.Name;
            return View();
        }

        [SecurityAuthorize("Security. Hello")]
        public ActionResult Hello()
        {
            ViewBag.Title = "Hello";
            return View();
        }

        #region Users

        [SecurityAuthorize("Security. GetUserList")]
        public ActionResult GetUserList()
        {
            var security = new CoreSecurity();
            return Json(security.UserCollection /*.Select(e => new UserModel { Login = e.Login })*/,
                JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. GetUserByLogin")]
        public ActionResult GetUserByLogin(string id)
        {
            var user =
                CoreSecurity.UserCollection.FirstOrDefault(e => e.Login.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                throw new InvalidOperationException($"User {id} is not found");

            return JsonByNewtonsoft(user, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [SecurityAuthorize("Security. AddNewUserId")]
        public ActionResult AddNewUserId(User user, string password)
        {
            using (var transaction = CoreSecurity.BeginTransaction())
            {
                try
                {
                    CoreSecurity.UserCollection.Add(user);
                    CoreSecurity.UserCollection.SaveChanges();
                    CoreSecurity.Tools.SetPassword(user.Login, password);
                    transaction.Commit();
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        [HttpPost]
        [SecurityAuthorize("Security. UpdateUser")]
        public async Task<ActionResult> UpdateUser(User user)
        {
            CoreSecurity.UserCollection.Update(user);
            await CoreSecurity.SaveChangesAsync();

            return JsonByNewtonsoft(user);
        }

        [SecurityAuthorize("Security. RemoveUsers")]
        public ActionResult RemoveUsers(User[] users)
        {
            CoreSecurity.UserCollection.RemoveRange(users);
            CoreSecurity.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SecurityAuthorize("Security. SetUserPassword")]
        public ActionResult SetUserPassword(string login, string password)
        {
            CoreSecurity.Tools.SetPassword(login, password);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region Groups

        [SecurityAuthorize("Security. GetGroupList")]
        public ActionResult GetGroupList()
        {
            return JsonByNewtonsoft(CoreSecurity.GroupCollection, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. GetGroupByName")]
        public ActionResult GetGroupByName(string id)
        {
            var group =
                CoreSecurity.GroupCollection.FirstOrDefault(e => e.Name.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (group == null)
                throw new InvalidOperationException($"Group {id} is not found");

            return JsonByNewtonsoft(group, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [SecurityAuthorize("Security. AddGroup")]
        public ActionResult AddGroup(string name, string description)
        {
            CoreSecurity.GroupCollection.Add(new Group() {Name = name, Description = description});
            CoreSecurity.GroupCollection.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [SecurityAuthorize("Security. UpdateGroup")]
        public ActionResult UpdateGroup(Group group)
        {
            CoreSecurity.GroupCollection.Update(group);
            CoreSecurity.SaveChanges();

            return JsonByNewtonsoft(group);
        }

        [HttpPost]
        [SecurityAuthorize("Security. RemoveGroups")]
        public ActionResult RemoveGroups(Group[] groups)
        {
            CoreSecurity.GroupCollection.RemoveRange(groups);
            CoreSecurity.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region Roles

        [SecurityAuthorize("Security. GetRoleList")]
        public ActionResult GetRoleList()
        {
            return JsonByNewtonsoft(CoreSecurity.RoleCollection, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. GetRoleByName")]
        public ActionResult GetRoleByName(string id)
        {
            var role =
                CoreSecurity.RoleCollection.FirstOrDefault(e => e.Name.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (role == null)
                throw new InvalidOperationException($"Role {id} is not found");

            return JsonByNewtonsoft(role, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. AddRole")]
        [HttpPost]
        public ActionResult AddRole(string name, string description)
        {
            CoreSecurity.RoleCollection.Add(new Role() {Name = name, Description = description});
            CoreSecurity.RoleCollection.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SecurityAuthorize("Security. UpdateRole")]
        public ActionResult UpdateRole(Role role)
        {
            using (var security = new CoreSecurity(ApplicationName))
            {
                security.RoleCollection.Update(role);
                security.SaveChanges();

                return JsonByNewtonsoft(role);
            }
        }

        [SecurityAuthorize("Security. RemoveRoles")]
        [HttpPost]
        public ActionResult RemoveRoles(Role[] roles)
        {
            CoreSecurity.RoleCollection.RemoveRange(roles);
            CoreSecurity.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region User profile

        [SecurityAuthorize("Security. GetGroupListByUser")]
        public ActionResult GetGroupListByUser(string id)
        {
            var groups = CoreSecurity.UserCollection.Include("Groups")
                .Where(u => u.Login == id)
                .SelectMany(u => u.Groups);

            return JsonByNewtonsoft(groups, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. GetNonUserGroups")]
        public ActionResult GetNonUserGroups(string id)
        {
            var userGroups =
                CoreSecurity.UserCollection.Include("Groups")
                    .Where(u => u.Login == id)
                    .SelectMany(e => e.Groups)
                    .Select(e => e.IdMember);
            var nonUserGroups = CoreSecurity.GroupCollection.Where(e => !userGroups.Contains(e.IdMember));

            return JsonByNewtonsoft(nonUserGroups, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. AddGroupsToUser")]
        public ActionResult AddGroupsToUser(string userId, Group[] groups)
        {
            CoreSecurity.Tools.AddGroupsToUser(userId, groups.Select(g => g.Name).ToArray());
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SecurityAuthorize("Security. DeleteGroupsFromUser")]
        public ActionResult DeleteGroupsFromUser(string user, Group[] groups)
        {
            CoreSecurity.Tools.DeleteGroupsFromUser(user, groups.Select(e => e.Name).ToArray());
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region Group profile

        [SecurityAuthorize("Security. GetUserListByGroup")]
        public ActionResult GetUserListByGroup(string id)
        {
            var users = CoreSecurity.GroupCollection.Include("Users")
                .Where(u => u.Name == id)
                .SelectMany(u => u.Users);

            return JsonByNewtonsoft(users, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. GetNonGroupUsers")]
        public ActionResult GetNonGroupUsers(string id)
        {
            var groupUsers =
                CoreSecurity.GroupCollection.Include("Users")
                    .Where(e => e.Name == id)
                    .SelectMany(e => e.Users)
                    .Select(e => e.IdMember);
            var nonGroupUsers = CoreSecurity.UserCollection.Where(e => !groupUsers.Contains(e.IdMember));

            return JsonByNewtonsoft(nonGroupUsers, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. AddUsersToGroup")]
        public ActionResult AddUsersToGroup(string group, User[] users)
        {
            CoreSecurity.Tools.AddUsersToGroup(group, users.Select(e => e.Login).ToArray());
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SecurityAuthorize("Security. DeleteUsersFromGroup")]
        public ActionResult DeleteUsersFromGroup(string group, User[] users)
        {
            CoreSecurity.Tools.DeleteUsersFromGroup(group, users.Select(e => e.Login).ToArray());
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region User&Group profile common methods

        [SecurityAuthorize("Security. GetRoleListByMember")]
        public ActionResult GetRoleListByMember(string id)
        {
            var idApplication = CoreSecurity.CurrentApplication.IdApplication;
            var roles = CoreSecurity.MemberCollection.Include("Roles")
                .Where(u => u.Name == id)
                .SelectMany(m => m.Roles)
                .Where(r => r.IdApplication == idApplication);

            return JsonByNewtonsoft(roles, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. GetNonMemberRoles")]
        public ActionResult GetNonMemberRoles(string id)
        {
            var idApplication = CoreSecurity.CurrentApplication.IdApplication;
            var roles =
                CoreSecurity.MemberCollection.Include("Roles")
                    .Where(u => u.Name == id)
                    .SelectMany(m => m.Roles)
                    .Where(r => r.IdApplication == idApplication)
                    .Select(e => e.IdRole);

            var nonMemberRoles = CoreSecurity.RoleCollection.Where(e => !roles.Contains(e.IdRole));

            return JsonByNewtonsoft(nonMemberRoles, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. AddRolesToMember")]
        public ActionResult AddRolesToMember(string memberId, Role[] roles, string appName)
        {
            CoreSecurity.Tools.AddRolesToMember(memberId, roles.Select(e => e.Name).ToArray(), appName);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SecurityAuthorize("Security. DeleteRolesFromMember")]
        public ActionResult DeleteRolesFromMember(string member, Role[] roles, string appName)
        {
            CoreSecurity.Tools.DeleteRolesFromMember(member, roles.Select(e => e.Name).ToArray(), appName);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region Role Profile

        [SecurityAuthorize("Security. GetMembersByRole")]
        public ActionResult GetMembersByRole(string id)
        {
            var roles =
                CoreSecurity.RoleCollection.Include("Members")
                    .Where(u => u.Name == id)
                    .SelectMany(m => m.Members);

            return JsonByNewtonsoft(roles, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. GetGrantsByRole")]
        public ActionResult GetGrantsByRole(string id)
        {
            var idRole = CoreSecurity.RoleCollection.Where(e => e.Name == id).Select(e => e.IdRole).Single();
            var idAccessType = CoreSecurity.GetAccessTypes().Where(e => e.Name == EAccessType.Exec.ToString()).Select(e => e.IdAccessType).Single();

            var grants = CoreSecurity.GrantCollection.Include("SecObject")
                .Include("Role")
                .Include("AccessType")
                .Where(e => e.Role.IdRole == idRole && e.AccessType.IdAccessType == idAccessType)
                .OrderBy(e => e.SecObject.ObjectName)
                .Select(
                    e =>
                        new GrantModel
                        {
                            Role = e.Role.Name,
                            SecObject = e.SecObject.ObjectName,
                            AccessType = e.AccessType.Name
                        });

            return JsonByNewtonsoft(grants, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. GetNonRoleGrants")]
        public ActionResult GetNonRoleGrants(string id)
        {
            var idRole = CoreSecurity.RoleCollection.Where(e => e.Name == id).Select(e => e.IdRole).Single();
            var idAccessType = CoreSecurity.GetAccessTypes().Where(e => e.Name == EAccessType.Exec.ToString()).Select(e => e.IdAccessType).Single();

            if (id == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            var nonSecObjects = CoreSecurity.GrantCollection
                .Include("SecObject")
                .Include("Role")
                .Include("AccessType")
                .Where(g => g.Role.IdRole == idRole)
                .Select(e => e.SecObject.ObjectName);

            var secObjects = CoreSecurity.SecObjectCollection.Where(e => e.IdAccessType == idAccessType).Where(e => !nonSecObjects.Contains(e.ObjectName));

            return JsonByNewtonsoft(secObjects, JsonRequestBehavior.AllowGet);
        }

        [SecurityAuthorize("Security. SetGrants")]
        public ActionResult SetGrants(string role, SecObject[] secObjects)
        {
            var grantCollection = CoreSecurity.GrantCollection;

            var accessTypes = CoreSecurity.GetAccessTypes().Where(e => e.Name == EAccessType.Exec.ToString()).ToArray();

            grantCollection.AddRange(role, secObjects, accessTypes, ApplicationName);
            CoreSecurity.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SecurityAuthorize("Security. RemoveGrants")]
        public ActionResult RemoveGrants(GrantModel[] grants)
        {
            var grantCollection = CoreSecurity.GrantCollection;

            grantCollection.RemoveRange(grants.Select(m => m.Role).ToArray(), grants.Select(m => m.SecObject).ToArray(), grants.Select(m => m.AccessType).ToArray(), ApplicationName);
            CoreSecurity.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region SecurityObjects

        [SecurityAuthorize("Security. GetSecurityObjects")]
        public ActionResult GetSecurityObjects()
        {
            var securityObjects = CoreSecurity.SecObjectCollection.Include("AccessType").Select(so => new {so.AccessType, so.Application, so.IdAccessType, so.IdSecObject, so.ObjectName}).ToArray();
            return JsonByNewtonsoft(securityObjects, JsonRequestBehavior.AllowGet);
        }
//
//        [HttpPost]
//        [SecurityAuthorize("Security. AddSecObjects")]
//        public ActionResult AddSecObjects(SecObject[] secObjects)
//        {
//            foreach (var secObject in secObjects)
//            {
//                CoreSecurity.SecObjectCollection.Add(secObject);
//            }
//
//            CoreSecurity.SaveChanges();
//            return new HttpStatusCodeResult(HttpStatusCode.OK);
//        }
//
//        [HttpPost]
//        [SecurityAuthorize("Security. DeleteSecObjects")]
//        public ActionResult DeleteSecObjects(SecObject[] secObjects)
//        {
//            CoreSecurity.SecObjectCollection.RemoveRange(secObjects);
//
//            CoreSecurity.SaveChanges();
//            return new HttpStatusCodeResult(HttpStatusCode.OK);
//        }

//        [SecurityAuthorize("Security. SetUpSecObjects")]
//        public ActionResult SetUpSecObjects()
//        {
//            SetUpSecurityObjects(CoreSecurity);
//            return new HttpStatusCodeResult(HttpStatusCode.OK);
//        }
//
//        [SecurityAuthorize("Security. SetUpSecurityObjects")]
//        private static void SetUpSecurityObjects(CoreSecurity security)
//        {
//            var securityObjects = Web.Config.GetSecurityObjects().Select(s => s.ObjectName).ToList();
//            var sameInstalledObjects =
//                security.SecObjectCollection.Where(e => securityObjects.Contains(e.ObjectName))
//                    .Select(e => e.ObjectName)
//                    .ToList();
//            var newSecObjects = securityObjects.Except(sameInstalledObjects, StringComparer.OrdinalIgnoreCase);
//
//            foreach (var secObject in newSecObjects.Select(s => new SecObject() {ObjectName = s}))
//            {
//                security.SecObjectCollection.Add(secObject);
//            }
//
//            security.SaveChanges();
//        }

        #endregion

        #region Log service

        [SecurityAuthorize("Security. Log")]
        [HttpPost]
        public async Task<ActionResult> Log(string message)
        {
            var log = await CoreSecurity.Tools.LogAsync(message);
            return JsonByNewtonsoft(new {idLog = log.IdLog, dateCreated = log.DateCreated});
        }

        [SecurityAuthorize("Security. GetLog")]
        public ActionResult GetLog(int id)
        {
            var log = CoreSecurity.Tools.GetLogById(id);

            var match = Regex.Match(log.Message, @"<html[.\w\S\s]+</html>|<!DOCTYPE[.\w\S\s]+</html>");
            if (!match.Success)
                return Json(log.Message, JsonRequestBehavior.AllowGet);

            var htmlString = match.Groups[0].Value;

            return Content(htmlString);
        }

        #endregion

        #region Application service

        [SecurityAuthorize("Security. GetApplications")]
        public ActionResult GetApplications()
        {
            return JsonByNewtonsoft(CoreSecurity.ApplicationCollection, JsonRequestBehavior.AllowGet);
        }

//        public ActionResult CreateApplication(Application application)
//        {
//            CoreSecurity.ApplicationCollection.Add(application);
//            CoreSecurity.SaveChanges();
//            return JsonByNewtonsoft(application, JsonRequestBehavior.AllowGet);
//        }

        [SecurityAuthorize("Security. DeleteApplication")]
        public ActionResult DeleteApplication(Application app)
        {
            CoreSecurity.ApplicationCollection.Remove(app);
            CoreSecurity.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SecurityAuthorize("Security. GetApplication")]
        public ActionResult GetApplication(string appname)
        {
            return JsonByNewtonsoft(CoreSecurity.ApplicationCollection.First(e => e.AppName == appname), JsonRequestBehavior.AllowGet);
        }

        #endregion               
    }
}

//todo: Добавить метод поиска html-страницы в тексте лога по регулярному выражению /<html[.\w\S\s]+</html>|<!DOCTYPE[.\w\S\s]+</html>/