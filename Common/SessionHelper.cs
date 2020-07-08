using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class SessionHelper
    {
        /// <summary>
        /// Build Folder Code, Class Helper using function, extension
        /// </summary>
        /// <param name="userSession"></param>
        public static void SetSession(UserSession userSession)
        {
            HttpContext.Current.Session["login"] = userSession;
        }
        public static UserSession GetSession()
        {
            var session = HttpContext.Current.Session["login"];
            if(session == null)
            {
                return null;
            }
            else
            {
                return session as UserSession;
            }
        }
    }
    public class UserSession
    {
        public string Name { get; set; }
        public string PasssWork { get; set; }
    }
}
