using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_Doctor.Areas.Admin.Data;
using Website_Doctor.Areas.Admin.Models;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class CouponController : BaseController
    {
        // GET: Admin/Coupon
        public ActionResult Index()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Title = "Danh sách mã giảm giá";
            var coupon = _context.Coupons.OrderByDescending(x => x.DateCreate);
            return View("Index", coupon);
        }

        public ActionResult CreateOrEdit(int? ID)
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ID != null && ID > 0)
            {
                var c = _context.Coupons.SingleOrDefault(x => x.ID == ID);
                VM_Coupons prod = new VM_Coupons();
                prod = prod.ConvertDataToModel(c);
                return View("CreateOrEdit", prod);
            }
            return View("CreateOrEdit");
        }
        [HttpPost]
        public ActionResult CreateOrEdit(VM_Coupons p)
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (!ModelState.IsValid)
            {
                return View("CreateOrEdit", p);
            }
            if (p.ID != null && p.ID > 0)
            {
                //edit
                Coupon prod = _context.Coupons.Find(p.ID);
                prod.DateStart = p.StartDate;
                prod.DateEnd = p.EndDate;
                if (p.CouponType == "true")
                {
                    prod.Value = p.Value;
                    prod.Percent = null;
                }
                else
                {
                    prod.Percent = p.Percent;
                    prod.Value = null;
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                //create
                if (_context.Products.Any(x => x.code.Equals(p.Code)))
                {
                    TempData["Err"] = "Chuyên khoa khám bệnh đã tồn tại trong hệ thống";
                    return View("CreateOrEdit", p);
                }
                var prod = p.ConvertModelToData();
                _context.Coupons.Add(prod);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

        }
    }
}