using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PartsController : Controller
    {
        [ChildActionOnly]
        public ActionResult _Toolbar()
        {
            return View();
        }

    }
}
