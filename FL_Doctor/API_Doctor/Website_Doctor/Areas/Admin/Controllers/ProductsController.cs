using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class ProductsController : BaseController
    {
        // GET: Admin/Products
        public ActionResult Index()
        {
            var product = _context.Products.Where(x => x.isActive == true).OrderByDescending(x=>x.dateCreate);
            return View("Index", product);
        }
    }
}