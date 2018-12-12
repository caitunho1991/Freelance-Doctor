﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class AccountsController : BaseController
    {
        // GET: Admin/Accounts
        public ActionResult Index()
        {
            ViewBag.Title = "Danh sách tài khoản";
            var accounts = _context.Accounts.OrderByDescending(x => x.DateCreate);
            return View("Index", accounts);
        }

        public ActionResult GetListDoctor()
        {
            ViewBag.Title = "Danh sách tài khoản bác sỹ";
            var accounts = _context.Accounts.Where(x=>x.Group.Code.Equals("doctor")).OrderByDescending(x => x.DateCreate);
            return View("Index", accounts);
        }

        public ActionResult GetListPatient()
        {
            ViewBag.Title = "Danh sách tài khoản bệnh nhân";
            var accounts = _context.Accounts.Where(x => x.Group.Code.Equals("patient")).OrderByDescending(x => x.DateCreate);
            return View("Index", accounts);
        }
        
        [HttpGet]
        public ActionResult ApproveDoctor(string GUID)
        {
            var doctor = _context.Accounts.SingleOrDefault(x=>x.GUID.Equals(GUID) && x.IsApprove == false);
            if(doctor != null)
            {
                doctor.IsApprove = true;
                _context.SaveChanges();
                TempData["MsgErr"] = "Tài khoản đã được xác thực. Vui lòng kiểm tra lại.";
                return RedirectToAction("Index");
            }
            TempData["MsgErr"] = "Tài khoản không có hoặc đã được xác thực. Vui lòng kiểm tra lại.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Login()
        {
            return View();
        }
    }
}