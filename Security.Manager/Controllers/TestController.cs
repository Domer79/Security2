using Security.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Security.Manager.Attributes;

namespace Security.Manager.Controllers
{
    public class TestController : ApiController
    {
        private string _appName;

        public TestController()
        {
            _appName = this.GetSecurityInfo().ApplicationName;
        }

        [SecurityHttpAuthorize("SecurityApi.GetAllowSecurityObjects")]
        [Route("api/AllowSecurityObjects/{appName}/{login}/{accessType}")]
        public IEnumerable<string> GetAllowSecurityObjects(string appName, string login, string accessType)
        {
            using(var security = new CoreSecurity(appName))
            {
                return security.GetAllowSecurityObjects(login, accessType).ToArray();
            }
        }

        [Route("api/AllowSecurityObjects/{appName}/{accessType}")]
        public IEnumerable<string> GetAllowAllSecurityObjects(string appName, string accessType)
        {
            using(var security = new CoreSecurity(appName))
            {
                return security.GetAllowAllSecurityObjects(accessType).ToArray();
            }
        }
    }
}
