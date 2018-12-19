using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class CouponController : BaseController
    {
        // GET: Admin/Coupon
        public ActionResult Index()
        {
            //var coupons = _context.Coupons.Where(x=>x.);
            return View();
        }
    }
}