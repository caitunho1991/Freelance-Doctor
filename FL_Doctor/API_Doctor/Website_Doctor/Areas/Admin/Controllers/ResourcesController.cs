using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_Doctor.Areas.Admin.Data;
using Website_Doctor.Areas.Admin.Models;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class ResourcesController : BaseController
    {
        public ActionResult Index()
        {
            var product = _context.Resources.OrderByDescending(x => x.ID);
            return View("Index", product);
        }

        public ActionResult Create()
        {
            return View("CreateOrEdit");
        }
        [HttpPost]
        public ActionResult CreateOrEdit(VM_Resources p)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateOrEdit", p);
            }
            if (p.ID != null && p.ID > 0)
            {
                //edit
                Resource re = _context.Resources.Find(p.ID);
                re.Name = p.Name;
                re.Value = p.Value;
                re.IsActive = p.Active;
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
                var re = p.ConvertModelToData();
                _context.Resources.Add(re);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        public ActionResult Edit(string Code)
        {
            var p = _context.Resources.SingleOrDefault(x => x.Code.Equals(Code));
            VM_Resources re = new VM_Resources();
            re = re.ConvertDataToModel(p);
            return View("CreateOrEdit", re);
        }
    }
}