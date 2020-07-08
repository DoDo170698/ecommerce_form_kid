using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Web.Routing;
using OnlineShop.Areas.Admin.Controllers;
using System.Web.Mvc;

namespace OnlineShop
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }
        HttpContextBase httpContext;
        public HasCredentialAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (UserLogin)HttpContext.Current.Session[Common.CommonConstants.USER_SESSION]; // đã set_name cho session
            if (session == null)
            {
                return false;
            }

            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(session.UserName); // Call another method to get rights of the user from DB

            if (privilegeLevels.Contains(this.RoleID) || session.GroupID == CommonConstants.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Context của BaseController
            var session = (UserLogin)HttpContext.Current.Session[Common.CommonConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Areas/Admin/Views/Login/Index.cshtml"
                };
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Areas/Admin/Views/Shared/401.cshtml"
                };
            }
            
        }
        
        private List<string> GetCredentialByLoggedInUser(string userName)
        {
            var credentials = (List<string>)HttpContext.Current.Session[Common.CommonConstants.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}