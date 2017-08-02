using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Security.Manager.Attributes
{
    public class SecurityHttpAuthorizeAttribute: Security.Web.Http.AuthorizeAttribute
    {
        public SecurityHttpAuthorizeAttribute(string objectName) 
            : base(typeof(SecurityHttpAuthorizeAttribute).Assembly)
        {
            if (string.IsNullOrWhiteSpace(objectName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(objectName));

            ObjectName = objectName;
        }
    }
}