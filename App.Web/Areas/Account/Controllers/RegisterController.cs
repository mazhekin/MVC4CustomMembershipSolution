using App.Core.Models;
using App.Core.Services;
using App.Web.Areas.Account.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace App.Web.Areas.Account.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IEmailService emailService;

        public RegisterController(IUsersService usersService, IEmailService emailService)
        {
            this.usersService = usersService;
            this.emailService = emailService;
        }

        //
        // GET: /Account/Register/

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Index(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    var token = WebSecurity.CreateUserAndAccount(model.Email, model.Password, null, requireConfirmationToken: true);

                    this.usersService.SendAccountActivationMail(model.Email);

                    return RedirectToAction("success", "register", new { email = model.Email, area = "account" });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register/Success

        [AllowAnonymous]
        public ActionResult Success(string email)
        {
            ViewData["email"] = email;
            return View();
        }

        //
        // POST: /Account/Register/Success

        [HttpPost, AllowAnonymous]
        // [CaptchaValidation("captcha")]
        public ActionResult Success(string email, string foo/*, bool captchaValid*/)
        {
            ViewData["email"] = email;
            // to do: captcha
            /*if (!captchaValid)
            {
                //ModelState.AddModelError("captcha", "Введен неверный код безопасности.");
            }
            else*/
            {
                try
                {
                    this.usersService.SendAccountActivationMail(email);
                    ViewBag.IsSent = true;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("_FORM", "Error is occured during sending email message. " + ex.Message);
                }
            }
            return View();
        }

        // 
        // GET:  /Account/Register/Confirmation

        [AllowAnonymous]
        public ActionResult Confirmation(Guid? guid)
        {
            if (!guid.HasValue)
            {
                ViewBag.Message = "Activation code is incorrect.";
                return View();
            }
            try
            {
                WebSecurity.ConfirmAccount(guid.Value.ToString());
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }

            ViewBag.Message = "Your account is activated.";

            var membership = this.usersService.GetMembershipByConfirmToken(guid.Value.ToString(), withUserProfile: true);
            WebSecurity.Login(membership.UserProfile.UserName, membership.ConfirmationToken);
            
            return RedirectToAction("Index", "Home", new { area = "" });
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
