using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class CommonController : BaseController
    {
        // GET: Admin/Common
        public ActionResult HeadProfile()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.FullName = Fullname;
            ViewBag.Avata = URL;
            return PartialView();
        }

        public ActionResult NavMenu()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.FullName = Fullname;
            ViewBag.Avata = URL;
            ViewBag.ID = ID;
            return PartialView();
        }
    }
}