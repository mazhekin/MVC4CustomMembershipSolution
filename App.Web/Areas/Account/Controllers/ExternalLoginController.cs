using App.Core.Data;
using App.Core.Services;
using App.Web.Areas.Account.Models;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Areas.Account.Controllers
{
    public class ExternalLoginController : Controller
    {
        private readonly IUsersService usersService;

        public ExternalLoginController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        //
        // POST: /Account/ExternalLogin/

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Index(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("Callback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLogin/Callback

        [AllowAnonymous]
        public ActionResult Callback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("Callback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("Failure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                var oAuthClientData = OAuthWebSecurity.GetOAuthClientData(result.Provider);
                ViewBag.ProviderDisplayName = oAuthClientData.DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("Confirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLogin/Confirmation

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Confirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                var userName = provider + "_" + providerUserId;
                var userProfile = this.usersService.GetUserProfile(userName);
                if (userProfile == null)
                {
                    userProfile = new UserProfile { UserName = provider + "_" + providerUserId, DisplayName = model.UserName };
                    this.usersService.Save(userProfile);
                }

                OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, userProfile.UserName);

                OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                return RedirectToLocal(returnUrl);
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLogin/Failure

        [AllowAnonymous]
        public ActionResult Failure()
        {
            return View();
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

    }
}
