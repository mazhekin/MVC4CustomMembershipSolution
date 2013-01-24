using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace App.Web.Areas.Account.Controllers
{
    public class LogoffController : Controller
    {
        //
        // POST: /Account/LogOff

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home", new { area = "" });
        }

    }
}
