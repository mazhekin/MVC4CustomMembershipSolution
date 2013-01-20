using App.Core;
using App.Core.Services;
using App.Core.Web;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace App.Web.Controllers
{
    [Authorize]
    [InitializeMembership]
    public partial class AccountController : Controller
    {
        private readonly IMembershipService membershipService;

        public AccountController(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

    }
}
