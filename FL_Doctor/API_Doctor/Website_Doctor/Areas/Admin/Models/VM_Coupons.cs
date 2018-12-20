using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Website_Doctor.Areas.Admin.Data;

namespace Website_Doctor.Areas.Admin.Models
{
    public class VM_Coupons
    {
        public int? ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên mã giảm giá")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mã giảm giá")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượt sử dụng")]
        public int Count { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày bắt đầu")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày kết thúc")]
        public DateTime EndDate { get; set; }
        public Decimal Value { get; set; }
        public Decimal Percent { get; set; }
        public string CouponType { get; set; }

        public Coupon ConvertModelToData()
        {
            Coupon prod = new Coupon();
            prod.Name = this.Name;
            prod.Description = this.Description;
            prod.Code = this.Code;
            prod.Count = this.Count;
            prod.DateStart = this.StartDate;
            prod.DateEnd = this.EndDate;
            prod.DateCreate = DateTime.Now;
            if (this.CouponType == "true")
            {
                prod.Value = this.Value;
            }
            else
            {
                prod.Percent = this.Percent;
            }
            return prod;
        }

        public VM_Coupons ConvertDataToModel(Coupon p)
        {
            VM_Coupons prod = new VM_Coupons();
            prod.ID = p.ID;
            prod.Name = p.Name;
            prod.Description = p.Description;
            prod.Code = p.Code;
            prod.Count = (int)p.Count;
            prod.StartDate = (DateTime)p.DateStart;
            prod.EndDate = (DateTime)p.DateEnd;
            prod.Value = p.Value != null ? (Decimal)p.Value : -1;
            prod.Percent = p.Percent != null ? (Decimal)p.Percent : -1;
            return prod;
        }
    }
}