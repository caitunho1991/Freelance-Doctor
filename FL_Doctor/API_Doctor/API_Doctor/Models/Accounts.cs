using API_Doctor.Data;
using API_Doctor.Helper;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace API_Doctor.Models
{
    public class VM_Account_Phone
    {
        public string PhoneNumber { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
    public class VM_Account_Thumb
    {
        public string ImageAvatar { get; set; }
    }
    public class VM_Account_Login
    {
        [Required(ErrorMessage = "Vui lòng nhập username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp vị trí hiện tại của bạn")]
        public string Lat { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp vị trí hiện tại của bạn")]
        public string Lng { get; set; }
        public bool IsDoctor { get; set; }
        public string TokenFacebook { get; set; }
        public string TokenDevice { get; set; }
    }
    public class VM_Res_Account_Short
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public int Sex { get; set; }
        public string IDCardNumber { get; set; }
        public string ImgIdCard { get; set; }
        public string ImgLicenseId { get; set; }
        public string ImgAvatar { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public bool IsDoctor { get; set; }
        public bool Active { get; set; }
        public int? MajorCode { get; set; }
        public string MajorName { get; set; }
    }
    public class VM_Res_Account_Balance : VM_Res_Account_Short{
        public decimal Balance { get; set; }
        public string DisplayBalance { get; set; }
    }
    public class VM_Res_Account
    {
        public string FullName { get; set; }
        public string MajorName { get; set; }
        public string Fee { get; set; }
        public int OrderCount { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ThumbnailIDCard { get; set; }
        public string ThumbnailLicense { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string GUID { get; set; }
        public int MajorCode { get; set; }
        public double Radius { get; set; }
    }
    public class VM_Req_GetDoctor : VM_Req_Status_Doctor
    {
        public int MajorCode { get; set; }
    }
    public class VM_Req_Status_Doctor
    {
        [Required(ErrorMessage = "Vui lòng cung cấp vị trí hiện tại của bạn")]
        public string Lat { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp vị trí hiện tại của bạn")]
        public string Lng { get; set; }

        public Account ConvertModelToData(Account a)
        {
            if(a.IsActive == true)
            {
                a.IsActive = false;
            }
            else
            {
                a.IsActive = true;
                a.ExpireTokenLogin = DateTime.Now.AddMinutes(10);
            }
            return a;
        }
    }
    public class VM_Account_Register
    {
        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ và tên đệm.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh.")]
        public string DateofBirth { get; set; }

        /* 1- male, 2-female, 3-other*/
        public int Sex { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        public string Email { get; set; }
        public string TokenFacebook { get; set; }
        public string IdCardNumber { get; set; }/*CMND*/
        public string IdCard { get; set; }/*Img CMND*/
        public string LicenseId { get; set; }/*Img Giấy phép hành nghề*/
        //[Required(ErrorMessage = "Vui lòng chọn chuyên khoa khám bệnh.")]
        public int MajorId { get; set; }/**/
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [StringLength(255, ErrorMessage = "Mật khẩu tối thiểu 5 ký tự", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int TypeRegister { get; set; }/*type=0 => email, type=1 => phone*/
        public bool IsDoctor { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp vị trí hiện tại của bạn")]
        public string Lat { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp vị trí hiện tại của bạn")]
        public string Lng{ get; set; }
        public string TokenDevice { get; set; }

        public Account convertModelToData()
        {
            Account acc = new Account();
            acc.Address = null;
            acc.BirthDay = DateTime.ParseExact(this.DateofBirth, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            acc.DateCreate = DateTime.Now;
            //acc.DeviceToken = null;
            acc.Email = this.Email;
            acc.ExpireTokenLogin = DateTime.Now;
            acc.FirstName = this.FirstName;
            acc.LastName = this.LastName;
            acc.FullName = this.LastName + " " + this.FirstName;
            acc.GUID = CMS_Security.GenerateGUID().ToString();
            acc.IDCard = this.IdCardNumber;
            acc.IsActive = false;
            acc.IsApprove = false;
            acc.TokenFacebook = this.TokenFacebook;
            acc.Lat = this.Lat;
            acc.Lng = this.Lng;
            acc.Password = CMS_Security.MD5(this.Password);

            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
            var a = phoneUtil.Parse(this.PhoneNumber, "VN");
            PhoneNumber phoneNumber = phoneUtil.ParseAndKeepRawInput(this.PhoneNumber, "VN");
            acc.PhoneNumber = phoneUtil.Format(phoneNumber, PhoneNumberFormat.E164);

            acc.Sex = this.Sex;
            acc.Thumbnail = null;
            acc.ThumbnailIDCard = string.IsNullOrEmpty(this.IdCard) == true ? null : CMS_Image.ConvertBase64ToImage(this.IdCard, "CardID-" + this.PhoneNumber);
            acc.ThumbnailLicense = string.IsNullOrEmpty(this.LicenseId) == true ? null : CMS_Image.ConvertBase64ToImage(this.LicenseId, "LicenseId-" + this.PhoneNumber);
            
            acc.VerifyCode = null;
            if (this.IsDoctor==true)
            {
                //doctor
                acc.GroupId = 1;
                acc.ProductId = this.MajorId;
                acc.TokenRegister = CMS_Security.GenerateGUID().ToString();
                acc.ExpireTokenRegister = DateTime.Now;
            }
            else
            {
                //patient
                acc.GroupId = 2;
                acc.TokenRegister = CMS_Security.GenerateGUID().ToString();
                acc.ExpireTokenRegister = DateTime.Now.AddMinutes(30);
                acc.ExpireTokenLogin = DateTime.Now.AddMinutes(30);

            }
            acc.DeviceToken = this.TokenDevice;
            if (this.TypeRegister == 0)
            {
                //register by email
                acc.VerifyEmail = true;
                acc.TokenLogin = CMS_Security.Base64Encode(acc.DeviceToken + " - " + acc.Password + " - " + acc.Email);
            }
            else
            {
                //register by phone
                acc.VerifyPhone = true;
                acc.TokenLogin = CMS_Security.Base64Encode(acc.DeviceToken + " - " + acc.Password + " - " + acc.PhoneNumber);
            }
            acc.Balance = 0;
            acc.OrderCount = 0;
            return acc;
        }

        public Account UpdateAccount(Account acc)
        {
            acc.Address = null;
            acc.BirthDay = DateTime.ParseExact(this.DateofBirth, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            acc.FirstName = this.FirstName;
            acc.LastName = this.LastName;
            acc.FullName = this.LastName + " " + this.FirstName;
            acc.IDCard = this.IdCardNumber;
            acc.Sex = this.Sex;
            acc.Thumbnail = null;
            acc.ThumbnailIDCard = string.IsNullOrEmpty(this.IdCard) == true ? null : CMS_Image.ConvertBase64ToImage(this.IdCard, "CardID-" + this.PhoneNumber);
            acc.ThumbnailLicense = string.IsNullOrEmpty(this.LicenseId) == true ? null : CMS_Image.ConvertBase64ToImage(this.LicenseId, "LicenseId-" + this.PhoneNumber);
            return acc;
        }
    }
}