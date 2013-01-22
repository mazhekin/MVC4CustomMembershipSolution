using App.Core;
using App.Core.Models;
using App.Core.Services;
using App.Web.Models;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace App.Web.Controllers
{
    [Authorize]
    public partial class AccountController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IEmailService emailService;

        public AccountController(IUsersService usersService, IEmailService emailService)
        {
            this.usersService = usersService;
            this.emailService = emailService;
        }

        //
        // GET: /Account/Login/

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }


        //
        // POST: /Account/LogOff

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    var token = WebSecurity.CreateUserAndAccount(model.Email, model.Password, null, requireConfirmationToken: true);

                    SendActivationMail(model.Email);

                    //WebSecurity.Login(model.Email, model.Password);
                    return RedirectToAction("RegisterSuccess", "Account", new { email = model.Email });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void SendActivationMail(string email)
        {
            var userProfile = this.usersService.GetUserProfile(email);
            if (userProfile == null)
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.ProviderError);
            }

            var membership = this.usersService.GetMembership(userProfile.UserId);
            if (membership == null)
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.ProviderError);
            }

            var websiteUrlName = WebConfigurationManager.AppSettings["WebsiteUrlName"];
            var viewData = new ViewDataDictionary { Model = userProfile };
            viewData.Add("Membership", membership);
            this.emailService.SendEmail(
                new SendEmailModel
                {
                    EmailAddress = email,
                    Subject = websiteUrlName + ": Confirm your registration",
                    WebsiteUrlName = websiteUrlName,
                    WebsiteTitle = WebConfigurationManager.AppSettings["WebsiteTitle"],
                    WebsiteURL = WebConfigurationManager.AppSettings["WebsiteURL"]
                },
                "ConfirmRegistration",
                viewData
                );
        }

        //
        // GET: /Account/RegisterSuccess

        public ActionResult RegisterSuccess(string email)
        {
            ViewData["email"] = email;
            return View();
        }

        [HttpPost]
       // [CaptchaValidation("captcha")]
        public ActionResult RegisterSuccess(string email, string foo/*, bool captchaValid*/)
        {
            ViewData["email"] = email;
            /*if (!captchaValid)
            {
                //ModelState.AddModelError("captcha", "Введен неверный код безопасности.");
            }
            else*/
            {
                try
                {
                    SendActivationMail(email);
                    ViewData["IsSent"] = true;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("_FORM", "Error is occured during sending email message. " + ex.Message);
                }
            }
            return View();
        }

        #region Helpers

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion

    }
}
