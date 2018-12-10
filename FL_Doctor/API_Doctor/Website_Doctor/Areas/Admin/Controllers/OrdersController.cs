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
    }
}