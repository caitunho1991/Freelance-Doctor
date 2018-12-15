using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            return View();
        }
    }
}