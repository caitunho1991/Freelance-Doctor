using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Doctor.Models
{
    public class VM_Response_Coupon
    {
        public string CouponDescription { get; set; }
        public decimal CouponValue { get; set; }
        public bool IsPercent { get; set; }
    }
}