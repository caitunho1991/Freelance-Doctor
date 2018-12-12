using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_Doctor.Areas.Admin.Data;
using Website_Doctor.Areas.Admin.Models;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class BlogsController : BaseController
    {
        // GET: Admin/Products
        public ActionResult Index()
        {
            ViewBag.Title = "Danh sách bài viết";
            var blogs = _context.Blogs.OrderByDescending(x => x.DateCreate);
            return View("Index", blogs);
        }

        public ActionResult Create()
        {
            return View("CreateOrEdit");
        }
        [HttpPost]
        public ActionResult CreateOrEdit(VM_Blogs p)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateOrEdit", p);
            }
            if (p.ID != null && p.ID > 0)
            {
                //edit
                Blog blog = _context.Blogs.Find(p.ID);
                blog.Title = p.Title;
                blog.ShortDescription = p.ShortDesscription;
                blog.Content = p.Content;
                blog.Alias= p.Alias;
                if (_context.Blogs.Any(x=>x.Alias.Equals(blog.Alias)))
                {
                    TempData["Err"] = "Mã bài viết đã tồn tại";
                    return View("CreateOrEdit", p);
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                //create
                var blog = p.ConvertModelToData();
                if (_context.Blogs.Any(x => x.Alias.Equals(blog.Alias)))
                {
                    TempData["Err"] = "Mã bài viết đã tồn tại";
                    return View("CreateOrEdit", p);
                }
                _context.Blogs.Add(blog);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int ID)
        {
            var p = _context.Blogs.SingleOrDefault(x => x.ID == ID);
            VM_Blogs blogs = new VM_Blogs();
            blogs = blogs.ConvertDataToModel(p);
            return View("CreateOrEdit", blogs);
        }
    }
}