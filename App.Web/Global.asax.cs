using App.Core;
using App.Web.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication, IUnityContainerAccessor
    {
        #region IUnityContainerAccessor

        private static UnityContainer container;

        public static IUnityContainer Container
        {
            get { return container; }
        }

        IUnityContainer IUnityContainerAccessor.Container
        {
            get { return Container; }
        }

        #endregion

        protected void Application_Start()
        {
            if (container == null)
            {
                container = new UnityContainer();
                ContainerConfig.RegisterTypes(container);
            }
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AuthConfig.RegisterAuth();
        }


    }
}