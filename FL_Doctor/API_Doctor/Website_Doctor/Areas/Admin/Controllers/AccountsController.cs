using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Admin/Accounts
        public ActionResult Index()
        {
            return View();
        }
    }
}