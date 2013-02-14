using App.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Areas.Account.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly IUsersService usersService;

        public ChangePasswordController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        //
        // GET: /Account/ChangePassword/

        public ActionResult Index(Guid? guid)
        {
            if (!guid.HasValue)
            {
                return RedirectToAction("BadLink");
            }
            var user = this.usersService.GetMembershipByVerificationToken(guid.Value.ToString(), false);
            if (user == null)
            {
                return RedirectToAction("BadLink");
            }
            return View();
        }

        //
        // POST: /Account/ChangePassword/

        [HttpPost]
        public ActionResult Index(Guid? guid, string newPassword, string confirmPassword)
        {
            // guid
            if (!guid.HasValue)
            {
                return RedirectToAction("BadLink");
            }
            var membership = this.usersService.GetMembershipByVerificationToken(guid.Value.ToString(), false);
            if (membership == null)
            {
                return RedirectToAction("BadLink");
            }

            // passwords
            if (String.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError("newPassword", "Password is required.");
                return View();
            }
            if (newPassword.Length < 4)
            {
                ModelState.AddModelError("newPassword", "Password is too short.");
                return View();
            }
            if (String.IsNullOrEmpty(confirmPassword))
            {
                ModelState.AddModelError("confirmPassword", "Confirm password.");
                return View();
            }
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Passwords are mismatched.");
                return View();
            }

            try
            {
                this.usersService.ChangePassword(membership, newPassword);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("_FORM", "Error is occurred. " + ex.Message);
            }

            return RedirectToAction("Success");
        }

        //
        // GET: /Account/ChangePassword/BadLink

        public ActionResult BadLink()
        {
            return View();
        }

        //
        // GET: /Account/ChangePassword/Success

        public ActionResult Success()
        {
            return View();
        }

    }
}
