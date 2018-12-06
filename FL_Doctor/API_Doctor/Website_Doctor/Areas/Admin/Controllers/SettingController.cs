using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class SettingController : Controller
    {
        // GET: Admin/Settings
        public ActionResult Index()
        {
            return View();
        }
    }
}