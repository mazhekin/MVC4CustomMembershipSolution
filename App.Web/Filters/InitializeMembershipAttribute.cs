using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace App.Core.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeMembershipAttribute : ActionFilterAttribute
    {
        private static MembershipInitializer initializer;
        private static object initializerLock = new object();
        private static bool isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(
                ref InitializeMembershipAttribute.initializer, 
                ref InitializeMembershipAttribute.isInitialized, 
                ref InitializeMembershipAttribute.initializerLock);
        }

        private class MembershipInitializer
        {
            public MembershipInitializer()
            {
                try
                {
                    WebSecurity.InitializeDatabaseConnection("Entities", "UserProfile", "UserId", "UserName", autoCreateTables: false);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }

    }
}