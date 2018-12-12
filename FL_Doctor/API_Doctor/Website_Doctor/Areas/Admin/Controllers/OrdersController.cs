using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        // GET: Admin/Orders
        public ActionResult Index()
        {
            return View();
        }

        public  ActionResult GetListTransactions()
        {
            var transactions = _context.Orders.Where(x=>x.OrderType.Code.Equals("order_doctor"));
            return View("Index", transactions);
        }

        public ActionResult GetListCashWithDrawal()
        {
            var cashwithdrawal = _context.Orders.Where(x=>x.OrderType.Code.Equals("cashwithdrawal"));
            return View("Index", cashwithdrawal);
        }

        public ActionResult GetListRecharge()
        {
            var recharge = _context.Orders.Where(x=>x.OrderType.Code.Equals("recharge"));
            return View("Index", recharge);
        }

        public ActionResult ApproveCashWithDrawal(string OrderNumber)
        {
            var c = _context.Orders.Single(x=>x.code.Equals(OrderNumber));
            if (c.OrderStatus.Count == 1)
            {
                var status = _context.OrderStatus.Single(x => x.code.Equals("cashwithdrawal_confirm"));
                c.OrderStatus.Add(status);
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
            var c = _context.Orders.Single(x => x.code.Equals(OrderNumber));
            if (c.OrderStatus.Count == 1)
            {
                var status = _context.OrderStatus.Single(x => x.code.Equals("cashwithdrawal_cancel"));
                c.OrderStatus.Add(status);
                var client = _context.Accounts.Single(x=>x.ID == c.idBuyer);
                client.Balance += c.totalPay;
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

        public ActionResult ApproveRecharge(string OrderNumber)
        {
            var c = _context.Orders.Single(x => x.code.Equals(OrderNumber));
            if (c.OrderStatus.Count == 1)
            {
                var status = _context.OrderStatus.Single(x => x.code.Equals("recharge_confirm"));
                c.OrderStatus.Add(status);
                var client = _context.Accounts.Single(x => x.ID == c.idBuyer);
                client.Balance += c.totalPay;
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
            var c = _context.Orders.Single(x => x.code.Equals(OrderNumber));
            if (c.OrderStatus.Count == 1)
            {
                var status = _context.OrderStatus.Single(x => x.code.Equals("recharge_cancel"));
                c.OrderStatus.Add(status);
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