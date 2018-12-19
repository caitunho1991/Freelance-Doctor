using API_Doctor.Data;
using API_Doctor.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_Doctor.Models
{
    #region Order
    public class VM_Order
    {
        public string GuidDoctor { get; set; }
        public string Coupon { get; set; }
    }
    public class VM_Order_Respone
    {
        public string OrderNumber { get; set; }
        public string DateCreate { get; set; }
        public string DoctorName { get; set; }
        public string MajorDescription { get; set; }
        public string PatientName { get; set; }
        public int Sex { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string OrderStatusName { get; set; }
        public string OrderStatusCode { get; set; }

    }
    #endregion

    #region CashWithDrawal
    public class VM_CashWithDrawal
    {
        [Required(ErrorMessage = "Vui lòng nhập tên chủ thẻ")]
        public string Card_FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số tài khoản")]
        public string Card_Number { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thông tin ngân hàng.")]
        public string Card_Bank { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số tiền cần giao dịch")]
        [DataType(DataType.Currency, ErrorMessage = "Số tiền phải lơn hơn 0 và ko có chứa ký tự alphabet. Vui lòng kiểm tra lại.")]
        public decimal Total { get; set; }

        public string Note { get; set; }

        public VM_CashWithDrawal()
        {
            FL_DoctorEntities _context = new FL_DoctorEntities();
            decimal total = 0;
            decimal.TryParse(_context.Resources.FirstOrDefault(x => x.Code.Equals("global_card_fee")).Value, out total);
            this.Card_FullName = _context.Resources.FirstOrDefault(x => x.Code.Equals("global_card_fullname")).Value;
            this.Card_Number = _context.Resources.FirstOrDefault(x => x.Code.Equals("global_card_number")).Value;
            this.Card_Bank = _context.Resources.FirstOrDefault(x => x.Code.Equals("global_card_bank")).Value;
            this.Note = _context.Resources.FirstOrDefault(x => x.Code.Equals("global_card_note")).Value;
            this.Total = total;
        }
        public Order ConvertModel()
        {
            Order order = new Order();
            order.card_fullname = this.Card_FullName;
            order.card_number = this.Card_Number;
            order.card_bank = this.Card_Bank;
            order.total = this.Total;
            order.discount = 0;
            order.totalPay = order.total - order.discount;
            order.note = this.Note;
            order.idOrderStatus = 4;
            order.dateCreate = DateTime.Now;
            return order;
        }
    }
    public class VN_Response_CashWithDrawal : VM_CashWithDrawal
    {
        public string OrderNumber { get; set; }
        public string DisplayTotal { get; set; }
    }
    #endregion
}