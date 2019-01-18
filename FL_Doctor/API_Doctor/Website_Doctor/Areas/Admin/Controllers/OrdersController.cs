using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_Doctor.Areas.Admin.Models;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        // GET: Admin/Orders
        public ActionResult Index()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Title = "Danh sách giao dịch";
            return View();
        }

        public  ActionResult GetListTransactions()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Title = "Danh sách giao dịch đặt hẹn";
            var transactions = _context.Orders.Where(x=>x.OrderType.Code.Equals("order_doctor")).OrderByDescending(x=>x.dateCreate);
            return View("Index", transactions);
        }

        public ActionResult GetListCashWithDrawal()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Title = "Danh sách giao dịch rút tiền";
            var cashwithdrawal = _context.Orders.Where(x=>x.OrderType.Code.Equals("cashwithdrawal")).OrderByDescending(x => x.dateCreate);
            return View("Index", cashwithdrawal);
        }

        public ActionResult GetListRecharge()
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Title = "Danh sách giao dịch nạp tiền";
            var recharge = _context.Orders.Where(x=>x.OrderType.Code.Equals("recharge")).OrderByDescending(x => x.dateCreate);
            return View("Index", recharge);
        }

        public ActionResult ApproveCashWithDrawal(string OrderNumber)
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var c = _context.Orders.Single(x=>x.code.Equals(OrderNumber));
            if (c.OrderStatus.Count == 1)
            {
                var status = _context.OrderStatus.Single(x => x.code.Equals("cashwithdrawal_confirm"));
                c.OrderStatus.Add(status);
                c.idOrderStatus = status.id;
                _context.SaveChanges();
                TempData["Err"] = "Xác nhận giao dịch rút tiền thành công.";
                return RedirectToAction("GetListCashWithDrawal");
            }
            else
            {
                TempData["Err"] = "Xác nhận giao dịch rút tiền không thành công.";
                return RedirectToAction("ApproveCashWithDrawal");
            }
        }

        public ActionResult CancelCashWithDrawal(string OrderNumber)
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var c = _context.Orders.Single(x => x.code.Equals(OrderNumber));
            if (c.OrderStatus.Count == 1)
            {
                var status = _context.OrderStatus.Single(x => x.code.Equals("cashwithdrawal_cancel"));
                c.OrderStatus.Add(status);
                var client = _context.Accounts.Single(x=>x.ID == c.idBuyer);
                client.Balance += c.totalPay;
                c.idOrderStatus = status.id;
                _context.SaveChanges();
                TempData["Err"] = "Hủy giao dịch rút tiền thành công.";
                return RedirectToAction("GetListCashWithDrawal");
            }
            else
            {
                TempData["Err"] = "Hủy giao dịch rút tiền không thành công.";
                return RedirectToAction("CancelCashWithDrawal");
            }
        }

        [HttpGet]
        public ActionResult GetOrderRecharge(string orderNumber)
        {
            var order = _context.Orders.Where(x => x.OrderType.Code.Equals("recharge") && x.code.Equals(orderNumber)).Select(x=> new VM_Order_Recharge {
                OrderNumber = x.code,
                Total = (decimal)x.totalPay
            }).Single();
            return PartialView("Recharge", order);
        }

        [HttpPost]
        public ActionResult ApproveRecharge(VM_Order_Recharge rc)
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var c = _context.Orders.Single(x => x.code.Equals(rc.OrderNumber));
            if (c.OrderStatus.Count == 1)
            {
                var status = _context.OrderStatus.Single(x => x.code.Equals("recharge_confirm"));
                c.OrderStatus.Add(status);
                c.total = rc.Total;
                c.totalPay = rc.Total;
                var client = _context.Accounts.Single(x => x.ID == c.idBuyer);
                client.Balance += rc.Total;
                c.idOrderStatus = status.id;
                _context.SaveChanges();
                TempData["Err"] = "Xác nhận giao dịch nạp tiền thành công.";
                return RedirectToAction("GetListCashWithDrawal");
            }
            else
            {
                TempData["Err"] = "Xác nhận giao dịch nạp tiền không thành công.";
                return RedirectToAction("ApproveCashWithDrawal");
            }
        }

        public ActionResult CancelRecharge(string OrderNumber)
        {
            if (this.CheckAuth() == false)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var c = _context.Orders.Single(x => x.code.Equals(OrderNumber));
            if (c.OrderStatus.Count == 1)
            {
                var status = _context.OrderStatus.Single(x => x.code.Equals("recharge_cancel"));
                c.OrderStatus.Add(status);
                c.idOrderStatus = status.id;
                _context.SaveChanges();
                TempData["Err"] = "Hủy giao dịch nạp tiền thành công.";
                return RedirectToAction("GetListCashWithDrawal");
            }
            else
            {
                TempData["Err"] = "Hủy giao dịch nạp tiền không thành công.";
                return RedirectToAction("CancelCashWithDrawal");
            }
        }
    }
}