using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Website_Doctor.Areas.Admin.Data;

namespace Website_Doctor.Areas.Admin.Models
{
    public class VM_Products
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên chuyên khoa khám bệnh")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mã chuyên khoa khám bệnh")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả chuyên khoa khám bệnh")]
        public string Desc { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập phí khám bệnh")]
        public Decimal Fee { get; set; }
        public bool Active { get; set; }
        public bool Delete { get; set; }

        public Product ConvertModelToData()
        {
            Product prod = new Product();
            prod.name = this.Name;
            prod.code = this.Code;
            prod.shortDesc = this.Desc;
            prod.price = this.Fee;
            prod.dateCreate = DateTime.Now;
            prod.isActive = this.Active;
            return prod;
        }

        public VM_Products ConvertDataToModel(Product p)
        {
            this.ID = p.id;
            this.Name = p.name;
            this.Code = p.code;
            this.Desc = p.shortDesc;
            this.Fee = (Decimal)p.price;
            this.Active = p.isActive != true?false: (bool)p.isActive;
            this.Delete = p.isDelete != true ? false : (bool)p.isDelete;
            return this;
        }
    }
}