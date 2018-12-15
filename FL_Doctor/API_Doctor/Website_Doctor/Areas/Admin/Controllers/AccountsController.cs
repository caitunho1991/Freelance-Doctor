using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Website_Doctor.Areas.Admin.Helpers;
using Website_Doctor.Areas.Admin.Models;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class AccountsController : BaseController
    {
        // GET: Admin/Accounts
        public ActionResult Index()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Title = "Danh sách tài khoản";
            var accounts = _context.Accounts.OrderByDescending(x => x.DateCreate);
            return View("Index", accounts);
        }

        public ActionResult GetListDoctor()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Title = "Danh sách tài khoản bác sỹ";
            var accounts = _context.Accounts.Where(x=>x.Group.Code.Equals("doctor")).OrderByDescending(x => x.DateCreate);
            return View("Index", accounts);
        }

        public ActionResult GetListPatient()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Title = "Danh sách tài khoản bệnh nhân";
            var accounts = _context.Accounts.Where(x => x.Group.Code.Equals("patient")).OrderByDescending(x => x.DateCreate);
            return View("Index", accounts);
        }
        
        [HttpGet]
        public ActionResult ApproveDoctor(string GUID)
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var doctor = _context.Accounts.SingleOrDefault(x=>x.GUID.Equals(GUID) && x.IsApprove == false);
            if(doctor != null)
            {
                doctor.IsApprove = true;
                _context.SaveChanges();
                TempData["MsgErr"] = "Tài khoản đã được xác thực.";
                return RedirectToAction("Index");
            }
            TempData["MsgErr"] = "Tài khoản không có hoặc đã được xác thực. Vui lòng kiểm tra lại.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Banned(string GUID)
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var doctor = _context.Accounts.SingleOrDefault(x => x.GUID.Equals(GUID));
            if (doctor != null)
            {
                if (doctor.IsBanned == true)
                {
                    doctor.IsBanned = false;
                }
                else
                {
                    doctor.IsBanned = true;
                }
                _context.SaveChanges();
                TempData["MsgErr"] = "Tài khoản đã bị banned.";
                return RedirectToAction("Index");
            }
            TempData["MsgErr"] = "Tài khoản không có hoặc đã bị banned. Vui lòng kiểm tra lại.";
            return RedirectToAction("Index");
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(VM_Accounts a)
        {
            if (!ModelState.IsValid)
            {
                return View(a);
            }
            a.Password = CMS_Lib.MD5(a.Password);
            if (_context.Accounts.Any(x=>x.Email == a.Username && x.VerifyEmail == true && x.Password.Equals(a.Password)))
            {
                var acc = _context.Accounts.Single(x => x.Email == a.Username && x.VerifyEmail == true && x.ID == 1);
                if (acc.ID == 1)
                {
                    if (Request.Cookies["TokenLogin"] != null)
                    {
                        Session.Clear();
                    }
                    acc.TokenLogin = CMS_Lib.GenerateGUID();
                    int token_time = 0;
                    int.TryParse(_context.Resources.Single(x => x.Code.Equals("cms_token_time")).Value, out token_time);
                    acc.ExpireTokenLogin = DateTime.Now.AddDays(token_time);
                    _context.SaveChanges();
                    HttpCookie user = new HttpCookie("TokenLogin");
                    user.Value = acc.TokenLogin;
                    user.Expires = DateTime.UtcNow.AddDays(1);
                    Response.Cookies.Add(user);
                    this.Fullname = acc.FullName;
                    this.ID = acc.ID;
                    this.URL = acc.Thumbnail;
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            ViewBag.Err = "Tên đăng nhập hoặc mật khẩu không đúng. Vui lòng kiểm tra lại";
            return View(a);
        }

        public ActionResult Logout()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var token = HttpContext.Request.Cookies["TokenLogin"].Value;
            if (_context.Accounts.Any(x=>x.TokenLogin.Equals(token)))
            {
                var acc = _context.Accounts.Single(x => x.TokenLogin.Equals(token));
                acc.TokenLogin = "";
                acc.ExpireTokenLogin = DateTime.Now;
                _context.SaveChanges();
            }
            Request.Cookies.Remove("TokenLogin");
            return RedirectToAction("Login", "Accounts");
        }

        public ActionResult Profile(int ID)
        {
            ViewBag.Title = "Thông tin tài khoản";
            var acc = _context.Accounts.Single(x => x.ID==ID);
            VM_Account_Edit a = new VM_Account_Edit();
            a = a.ConvertDataToModel(acc);
            return View("CreateOrEdit", a);
        }
        [HttpPost]
        public ActionResult Profile(VM_Account_Edit a)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateOrEdit", a);
            }
            var acc = _context.Accounts.Find(a.ID);
            acc.FirstName = a.FirstName;
            acc.LastName = a.LastName;
            acc.FullName = a.LastName + " " + a.FirstName;
            acc.Sex = a.Sex;
            acc.BirthDay = a.DateOfBirth;
            _context.SaveChanges();
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult ChangePassword(int ID)
        {
            ViewBag.Title = "Đổi mật khẩu tài khoản";
            var acc = _context.Accounts.Single(x => x.ID==ID);
            VM_ChangePassword a = new VM_ChangePassword();
            a = a.ConvertDataToModel(acc);
            return View(a);
        }
        [HttpPost]
        public ActionResult ChangePassword(VM_ChangePassword a)
        {
            if (!ModelState.IsValid)
            {
                return View(a);
            }
            var acc = _context.Accounts.Find(a.ID);
            var tmp = acc.Password.Equals(CMS_Lib.MD5(a.OldPassword));
            if (!tmp)
            {
               TempData["MsgErr"] = "Mật khẩu cũ không đúng";
                return View(a);
            }
            acc.Password = CMS_Lib.MD5(a.NewPassword);
            _context.SaveChanges();
            return RedirectToAction("Index", "Dashboard");
        }
    }
}