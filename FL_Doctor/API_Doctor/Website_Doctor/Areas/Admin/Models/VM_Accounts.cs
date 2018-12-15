using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Website_Doctor.Areas.Admin.Data;

namespace Website_Doctor.Areas.Admin.Models
{
    public class VM_Accounts
    {
        [Required(ErrorMessage = "Vui lòng nhập username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
    }
    public class VM_Account_Edit
    { 
        public int ID { get;set;}
        [Required(ErrorMessage = "Vui lòng nhập họ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Sex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Balance { get; set; }

        public Account ConvertModelToData()
        {
            Account a = new Account();
            a.FirstName = this.FirstName;
            a.LastName = this.LastName;
            a.BirthDay = this.DateOfBirth;
            a.Sex = this.Sex;
            return a;
        }
        public VM_Account_Edit ConvertDataToModel(Account acc)
        {
            this.ID = acc.ID;
            this.FirstName = acc.FirstName;
            this.LastName = acc.LastName;
            this.DateOfBirth = (DateTime)acc.BirthDay;
            this.Balance = (decimal)acc.Balance;
            this.Email = acc.Email;
            this.PhoneNumber = acc.PhoneNumber;
            this.Sex = (int)acc.Sex;
            return this;
        }
    }
    public class VM_ChangePassword
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu mới không đúng vui lòng kiểm tra lại")]
        public string ConfirmPassword { get; set; }
        public Account ConvertModelToData()
        {
            Account a = new Account();
            a.Password = this.OldPassword;
            return a;
        }
        public VM_ChangePassword ConvertDataToModel(Account acc)
        {
            this.ID = acc.ID;
            this.OldPassword = acc.Password;
            return this;
        }
    }
}