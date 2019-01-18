using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website_Doctor.Areas.Admin.Models
{
    public class VM_Order_Recharge
    {
        public string OrderNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số tiền cần giao dịch")]
        public decimal Total { get; set; }
    }
}