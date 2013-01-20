using App.Core;
using App.Core.Services;
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
    public partial class AccountController : Controller
    {
        private readonly IUsersService usersService;

        public AccountController(IUsersService usersService)
        {
            this.usersService = usersService;
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
