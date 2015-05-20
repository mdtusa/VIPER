using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices.AccountManagement;

namespace VIPER.Tools
{
    public class AuthorizeADAttribute : AuthorizeAttribute
    {
        public string Groups { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (base.AuthorizeCore(httpContext))
            {
                /* Return true immediately if the authorization is not 
                locked down to any particular AD group */
                if (String.IsNullOrEmpty(Groups))
                    return true;

                // Get the AD groups
                var groups = Groups.Split(',').ToList<string>();

                // Verify that the user is in the given AD group (if any)
                PrincipalContext context = new PrincipalContext(ContextType.Domain, "md-man.biz");
                var userPrincipal = UserPrincipal.FindByIdentity(context, HttpContext.Current.User.Identity.Name);

                foreach (var group in groups)
                    if (userPrincipal.IsMemberOf(context, IdentityType.Name, group))
                        return true;
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var result = new ViewResult();
                result.ViewName = "NotAuthorized";
                result.MasterName = "_Layout";
                filterContext.Result = result;
            }
            else
                base.HandleUnauthorizedRequest(filterContext);
        }
    }
}
