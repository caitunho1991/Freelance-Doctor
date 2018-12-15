using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_Doctor.Areas.Admin.Data;
using Website_Doctor.Areas.Admin.Models;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class ProductsController : BaseController
    {
        // GET: Admin/Products
        public ActionResult Index()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Title = "Danh sách chuyên khoa khám bệnh";
            var product = _context.Products.OrderByDescending(x=>x.dateCreate);
            return View("Index", product);
        }

        public ActionResult CreateOrEdit(string Code)
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (!string.IsNullOrEmpty(Code))
            {
                var p = _context.Products.SingleOrDefault(x => x.code.Equals(Code));
                VM_Products prod = new VM_Products();
                prod = prod.ConvertDataToModel(p);
                return View("CreateOrEdit", prod);
            }
            return View("CreateOrEdit");
        }
        [HttpPost]
        public ActionResult CreateOrEdit(VM_Products p)
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (!ModelState.IsValid)
            {
                return View("CreateOrEdit",p);
            }
            if (p.ID != null && p.ID > 0 && p.Delete != true)
            {
                //edit
                Product prod = _context.Products.Find(p.ID);
                prod.name = p.Name;
                prod.shortDesc = p.Desc;
                prod.price = p.Fee;
                prod.isActive = p.Active; 
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
                _context.Products.Add(prod);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }

        [HttpGet]
        public ActionResult Delete(string code)
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            try
            {
                var prod = _context.Products.SingleOrDefault(x => x.code.Equals(code));
                if (prod != null)
                {
                    prod.isActive = false;
                    prod.isDelete = true;
                    prod.dateDelete = DateTime.Now;
                    _context.SaveChanges();
                    TempData["Err"] = "Xóa chuyên khoa khám bệnh thành công.";
                    return RedirectToAction("Index");
                }
                TempData["Err"] = "Chuyên khoa khám bệnh không có hoặc không tồn tại. Vui lòng kiểm tra lại";
                return View();
            }
            catch(Exception e)
            {
                TempData["Err"] = "Có lỗi trong quá trình xử lý.";
                return View();
            }
        }
    }
}