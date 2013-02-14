using App.Core.Services;
using App.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Areas.Account.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IUsersService usersService;

        public ForgotPasswordController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        //
        // GET: /Account/ForgotPassword/

        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword/

        [HttpPost]
        public ActionResult Index(string email/*, bool captchaValid*/)
        {
            email = email.Trim();
            if (String.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("email", "Email is required.");
            }
            else
            {
                if (!Regex.IsMatch(email, RegularExpressions.EmailAddress))
                {
                    ModelState.AddModelError("email", "Incorrect Email.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.usersService.SentChangePasswordEmail(email);
                    return RedirectToAction("Sent");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("email", ex.Message);
                }
            }

            return View();
        }

        //
        // GET: /Account/ForgotPassword/Sent

        public ActionResult Sent()
        {
            return View();
        }

    }
}
