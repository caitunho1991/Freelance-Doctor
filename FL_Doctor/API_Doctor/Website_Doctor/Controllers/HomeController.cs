using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website_Doctor.Controllers
{
    public class HomeController : Controller
    {
        Website_Doctor.Areas.Admin.Data.FL_DoctorEntities _context = new Areas.Admin.Data.FL_DoctorEntities();
        public ActionResult About()
        {
            var a = _context.Blogs.Find(1);
            return View("Blogs", a);
        }
        public ActionResult Terms()
        {
            var a = _context.Blogs.Find(2);
            return View("Blogs", a);
        }
        public ActionResult Support()
        {
            var a = _context.Blogs.Find(3);
            return View("Blogs", a);
        }
    }
}