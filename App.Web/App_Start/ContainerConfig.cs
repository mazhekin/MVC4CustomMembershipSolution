using App.Core;
using App.Core.Data;
using App.Core.Services;
using App.Web.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace App.Web
{
    public static class ContainerConfig
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container.RegisterInstance<IUnityContainer>(container);

            container.RegisterType<Entities>(new HttpContextLifetimeManager<Entities>(), new InjectionConstructor());

            container.RegisterType<IDatabaseContext, Entities>();

            container.RegisterType<IUsersService, UsersService>();

            container.RegisterType<SmtpClient>(new InjectionConstructor());
            container.RegisterType<IEmailService, EmailService>();

            container.RegisterType<IConfigService, ConfigService>();

            ControllerBuilder.Current.SetControllerFactory(typeof(UnityControllerFactory));
        }
    }
}