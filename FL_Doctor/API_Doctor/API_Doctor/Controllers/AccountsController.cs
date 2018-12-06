using API_Doctor.Data;
using API_Doctor.Helper;
using API_Doctor.Models;
using Newtonsoft.Json;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace API_Doctor.Controllers
{
    public class AccountsController : BaseController
    {
        public Response<VM_Res_Account_Short> response = new Response<VM_Res_Account_Short>();
        public Response<List<VM_Res_Account>> responseAccount = new Response<List<VM_Res_Account>>();
        #region Service

        /// <summary>
        /// Lấy số dư hiện tại của bác sỹ
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("API/GetBalance/{token}")]
        [HttpGet]
        public IHttpActionResult GetBalance(string token)
        {
            try
            {
                Response<VM_Res_Account_Balance> response = new Response<VM_Res_Account_Balance>();
                if (!ModelState.IsValid)
                {
                    return Ok(response.BadRequest("Tên đăng nhập hoặc mật khẩu không đúng. Vui lòng kiểm tra lại."));
                }
                var acc = _context.Accounts.Where(x => x.TokenLogin.Equals(token)).SingleOrDefault();
                if (acc != null && acc.Group.Code == "doctor")
                {
                    var DateOfBirth = ((DateTime)acc.BirthDay).ToString("dd/MM/yyyy");
                    var displayBalance = string.Format("{0:0,000 vnd}", acc.Balance);
                    var res = _context.Accounts.Where(x => x.TokenLogin.Equals(token)).Select(x => new VM_Res_Account_Balance
                    {
                        FullName = x.FullName,
                        Token = x.TokenLogin,
                        IsDoctor = x.GroupId == 1 ? true : false,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        IDCardNumber = x.IDCard,
                        DateOfBirth = DateOfBirth,
                        Sex = (int)x.Sex,
                        UserName = x.VerifyEmail == true ? x.Email : x.PhoneNumber,
                        Active = (bool)x.IsActive,
                        Balance = (decimal)x.Balance,
                        DisplayBalance = displayBalance,
                        ImgIdCard = string.IsNullOrEmpty(x.ThumbnailIDCard) == true ? ""  : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailIDCard,
                        ImgLicenseId = string.IsNullOrEmpty(x.ThumbnailLicense) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailLicense,
                        ImgAvatar = string.IsNullOrEmpty(x.Thumbnail) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.Thumbnail,
                        MajorCode = (int)x.ProductId
                    }).SingleOrDefault();
                    return Ok(response.Ok(res, "Lấy số dư tài khoản thành công."));
                }
                return Ok(response.BadRequest("Tên đăng nhập hoặc mật khẩu không đúng. Vui lòng kiểm tra lại."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Đăng nhập tài khoản
        /// </summary>
        /// <param name="req">
        /// 1. Username => Phone / Email
        /// 2. Password
        /// 3. Lat => require if it's a account doctor
        /// 4. Lng => require if it's a account doctor
        /// 5.IsDoctor = true => account doctor
        /// </param>
        /// <returns></returns>
        [Route("API/Accounts/Login")]
        [HttpPost]
        public IHttpActionResult Login(VM_Account_Login req)
        {
            try
            {
                if (!string.IsNullOrEmpty(req.TokenFacebook))
                {
                    ModelState["req.Username"].Errors.Clear();
                    ModelState["req.Password"].Errors.Clear();
                }
                if (!ModelState.IsValid)
                {
                    return Ok(response.BadRequest(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
                }
                
                var GroupId = req.IsDoctor == true ? 1 : 2;
                Account acc = new Account();
                if (req.TokenFacebook == null || string.IsNullOrEmpty(req.TokenFacebook) || req.TokenFacebook.ToUpper().Equals("STRING"))
                {
                    acc = _context.Accounts.Where(x => (x.Email.ToUpper().Equals(req.Username.ToUpper()) && x.VerifyEmail == true) || (x.PhoneNumber.Equals(req.Username) && x.VerifyPhone == true)).Where(x => x.Password.Equals(req.Password)).Where(x => x.GroupId == GroupId).SingleOrDefault();
                }
                else{
                    acc = _context.Accounts.Where(x => x.TokenFacebook.Contains(req.TokenFacebook)).Where(x => x.GroupId == GroupId).SingleOrDefault();
                }
                if (acc != null )
                {
                    if (acc.ExpireTokenLogin == null || DateTime.Compare((DateTime)acc.ExpireTokenLogin, DateTime.Now) <= 0)
                    {
                        acc.TokenLogin = CMS_Security.GenerateGUID().ToString();
                        acc.ExpireTokenLogin = DateTime.Now.AddMinutes(10);
                        acc.IsActive = true;
                        acc.Lat = req.Lat != null ? req.Lat : "0";
                        acc.Lng = req.Lng != null ? req.Lng : "0";
                        acc.DeviceToken = req.TokenDevice;
                        acc.TokenFacebook = req.TokenFacebook;
                    }
                    else
                    {
                        acc.Lat = req.Lat;
                        acc.Lng = req.Lng;
                        acc.DeviceToken = req.TokenDevice;
                        acc.TokenFacebook = req.TokenFacebook;
                    }
                    var tmpToken = CMS_Security.Base64Encode(req.TokenDevice + " - " + req.Password + " - " + req.Username);
                    acc.TokenAutoLogin = tmpToken;
                    _context.SaveChanges();

                    var DateOfBirth = ((DateTime)acc.BirthDay).ToString("dd/MM/yyyy");
                    var res = _context.Accounts.Where(x => (x.Email.ToUpper().Equals(req.Username.ToUpper()) && x.VerifyEmail == true) || (x.PhoneNumber.Equals(req.Username) && x.VerifyPhone == true)).Where(x => x.Password.Equals(req.Password) && x.GroupId == GroupId).Select(x => new VM_Res_Account_Short
                    {
                        FullName = x.FullName,
                        Token = x.TokenLogin,
                        IsDoctor = GroupId == 1 ? true : false,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        IDCardNumber = x.IDCard,
                        DateOfBirth = DateOfBirth,
                        Sex = (int)x.Sex,
                        UserName = req.Username,
                        Active = (bool)x.IsActive,
                        ImgIdCard = string.IsNullOrEmpty(x.ThumbnailIDCard) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailIDCard,
                        ImgLicenseId = string.IsNullOrEmpty(x.ThumbnailLicense) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailLicense,
                        ImgAvatar = string.IsNullOrEmpty(x.Thumbnail) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.Thumbnail,
                        MajorCode = x.ProductId,
                        MajorName = x.Product.name,
                        TokenAutoLogin = tmpToken
                    }).SingleOrDefault();
                    return Ok(response.Ok(res, "Đăng nhập thành công."));
                }                
                return Ok(response.BadRequest("Tên đăng nhập hoặc mật khẩu không đúng. Vui lòng kiểm tra lại."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Đăng ký tài khoản
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("API/Accounts/Register")]
        [HttpPost]
        public IHttpActionResult Register(VM_Account_Register req)
        {
            try
            {
                if (!string.IsNullOrEmpty(req.TokenFacebook))
                {
                    ModelState["req.Email"].Errors.Clear();
                    ModelState["req.PhoneNumber"].Errors.Clear();
                    ModelState["req.Password"].Errors.Clear();
                }
                if (!ModelState.IsValid)
                {
                    return Ok(response.BadRequest(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
                }
                if (!string.IsNullOrEmpty(req.TokenFacebook))
                {
                    if (_context.Accounts.Any(x => x.TokenFacebook.Contains(req.TokenFacebook)))
                    {
                        return Ok(response.Ok(null, "Tài khoản đã tồn tại trong hệ thống. Vui lòng kiểm tra lại email hoặc số điện thoại ", false));
                    }
                }
                else
                {
                    if (_context.Accounts.Any(x => x.Email.ToUpper().Equals(req.Email.ToUpper()) || x.PhoneNumber.Equals(req.PhoneNumber)))
                    {
                        return Ok(response.Ok(null, "Tài khoản đã tồn tại trong hệ thống. Vui lòng kiểm tra lại email hoặc số điện thoại ", false));
                    }
                }
                
                
                var acc = req.convertModelToData();
                if (req.IsDoctor==false || ( CMS_Lib.Resource("accept_register")!="1") && req.IsDoctor==true)
                {
                    acc.IsApprove = true;//just for doctor
                }
                var DateOfBirth = ((DateTime)acc.BirthDay).ToString("dd/MM/yyyy");

                string tmpToken = "";
                if (acc.VerifyEmail == true )
                {
                    tmpToken = CMS_Security.Base64Encode(req.TokenDevice + " - " + req.Password + " - " + req.Email);
                }
                if (acc.VerifyPhone == true)
                {
                    tmpToken = CMS_Security.Base64Encode(req.TokenDevice + " - " + req.Password + " - " + req.PhoneNumber);
                }

                acc.TokenAutoLogin = tmpToken;
                _context.Accounts.Add(acc);
                _context.SaveChanges();
                if (!string.IsNullOrEmpty(req.TokenFacebook))
                {
                    var dataResponse = _context.Accounts.Where(x => x.TokenFacebook.Contains(req.TokenFacebook)).Select(x => new VM_Res_Account_Short
                    {
                        FullName = x.FullName,
                        Token = x.TokenLogin,
                        IsDoctor = x.GroupId == 1 ? true : false,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        IDCardNumber = x.IDCard,
                        DateOfBirth = DateOfBirth,
                        Sex = (int)x.Sex,
                        UserName = string.IsNullOrEmpty(req.Email) == true ? req.PhoneNumber : req.Email,
                        Active = (bool)x.IsActive,
                        ImgIdCard = string.IsNullOrEmpty(x.ThumbnailIDCard) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailIDCard,
                        ImgLicenseId = string.IsNullOrEmpty(x.ThumbnailLicense) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailLicense,
                        ImgAvatar = string.IsNullOrEmpty(x.Thumbnail) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.Thumbnail,
                        MajorCode = (int)x.ProductId,
                        MajorName = x.Product.name,
                        TokenAutoLogin = tmpToken
                    }).SingleOrDefault();
                    return Ok(response.Ok(dataResponse, "Đăng ký tài khoản thành công."));
                }
                else
                {
                    var dataResponse = _context.Accounts.Where(x => x.Email.ToUpper().Equals(req.Email.ToUpper()) || x.PhoneNumber.Equals(req.PhoneNumber)).Select(x => new VM_Res_Account_Short
                    {
                        FullName = x.FullName,
                        Token = x.TokenLogin,
                        IsDoctor = x.GroupId == 1 ? true : false,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        IDCardNumber = x.IDCard,
                        DateOfBirth = DateOfBirth,
                        Sex = (int)x.Sex,
                        UserName = string.IsNullOrEmpty(req.Email) == true ? req.PhoneNumber : req.Email,
                        Active = (bool)x.IsActive,
                        ImgIdCard = string.IsNullOrEmpty(x.ThumbnailIDCard) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailIDCard,
                        ImgLicenseId = string.IsNullOrEmpty(x.ThumbnailLicense) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailLicense,
                        ImgAvatar = string.IsNullOrEmpty(x.Thumbnail) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.Thumbnail,
                        MajorCode = (int)x.ProductId,
                        MajorName = x.Product.name,
                        TokenAutoLogin = tmpToken
                    }).SingleOrDefault();
                    return Ok(response.Ok(dataResponse, "Đăng ký tài khoản thành công."));
                }
                
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }
        
        /// <summary>
        /// Đăng xuất tài khoản
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("API/Accounts/Logout/{token}")]
        public IHttpActionResult LogOut(string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth("Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                var acc = _context.Accounts.SingleOrDefault(x => x.TokenLogin.Equals(token));
                if (acc != null)
                {
                    acc.TokenLogin = "";
                    acc.ExpireTokenLogin = DateTime.Now;
                    _context.SaveChanges();
                    return Ok(response.Ok(null, "Đăng xuất tài khoản thành công."));
                }
                return Ok(response.BadRequest("Tài khoản của bạn không tồn tại."));
            }
            catch (Exception e)
            {
                return BadRequest("Có lỗi trong quá trình xử lý.");
            }
        }

        /// <summary>
        /// Cập nhật thông tin tài khoản
        /// </summary>
        /// <param name="req"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("API/Accounts/UpdateProfile/{token}")]
        public IHttpActionResult UpdateProfile(VM_Account_Register req, string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth("Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                var tmp = _context.Accounts.SingleOrDefault(x => x.TokenLogin.Contains(token));
                var acc = req.UpdateAccount(tmp);
                _context.SaveChanges();
                var DateOfBirth = ((DateTime)acc.BirthDay).ToString("dd/MM/yyyy");
                var dataResponse = _context.Accounts.Where(x => x.TokenLogin.Contains(token)).Select(x => new VM_Res_Account_Short
                {
                    FullName = x.FullName,
                    Token = x.TokenLogin,
                    IsDoctor = x.GroupId == 1 ? true : false,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    IDCardNumber = x.IDCard,
                    DateOfBirth = DateOfBirth,
                    Sex = (int)x.Sex,
                    UserName = string.IsNullOrEmpty(x.Email) == true ? x.PhoneNumber : x.Email,
                    Active = (bool) x.IsActive,
                    ImgIdCard = string.IsNullOrEmpty(x.ThumbnailIDCard) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailIDCard,
                    ImgLicenseId = string.IsNullOrEmpty(x.ThumbnailLicense) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailLicense,
                    ImgAvatar = string.IsNullOrEmpty(x.Thumbnail) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.Thumbnail,
                    MajorCode = (int)x.ProductId,
                    MajorName = x.Product.name
                }).SingleOrDefault();
                return Ok(response.Ok(dataResponse, "Cập nhật thông tin tài khoản thành công."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        ///// <summary>
        ///// Lấy danh sách bác sỹ
        ///// </summary>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("API/GetListDoctor/{token}")]
        //public IHttpActionResult GetListAccount(string token)
        //{
        //    try
        //    {
        //        if (!checkAuth(token))
        //        {
        //            return Ok(response.NoAuth("Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
        //        }
        //        #region Get list doctor with radius
        //        double condition_radius = 0;
        //        decimal global_min_fee_doctor = 0;
        //        decimal.TryParse(CMS_Lib.Resource("global_min_fee_doctor"), out global_min_fee_doctor);
        //        double.TryParse(CMS_Lib.Resource("global_condition_radius"), out condition_radius);
        //        var patient = _context.Accounts.SingleOrDefault(x=>x.TokenLogin.Equals(token));
        //        var lstDoctor = _context.Accounts.Where(x=>x.GroupId == 1 && x.Balance > global_min_fee_doctor && x.IsActive == true && x.IsApprove == true).ToList();
        //        List<Account> tmp = new List<Account>();
        //        var radius = 0.0;
        //        foreach(var item in lstDoctor)
        //        {
        //            //radius = CMS_Lib.Radius(patient.Lat, patient.Lng, item.Lat, item.Lng) * 0.621371192;
        //            radius = CMS_Lib.DistanceTo(double.Parse(patient.Lat), double.Parse(patient.Lng), double.Parse(item.Lat), double.Parse(item.Lng));

        //            if (radius <= condition_radius)
        //            {
        //                tmp.Add(item);
        //            }
        //        }
        //        #endregion

        //        var list = tmp.Select(x=> new VM_Res_Account {
        //            FullName = x.FullName,
        //            MajorName = x.Product.name,
        //            Fee = x.Product.price.ToString(),
        //            Lat = x.Lat,
        //            Lng = x.Lng,
        //            ThumbnailUrl = string.IsNullOrEmpty(x.Thumbnail) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.Thumbnail,
        //            OrderCount = (int)x.OrderCount,
        //            GUID = x.GUID,
        //            MajorCode = x.Product.id,
        //            ThumbnailIDCard = string.IsNullOrEmpty(x.ThumbnailIDCard) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailIDCard,
        //            ThumbnailLicense = string.IsNullOrEmpty(x.ThumbnailLicense) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailLicense,
        //            Radius = condition_radius
        //        }).ToList();
        //        return Ok(responseAccount.Ok(list, "Lấy danh sách bác sỹ thành công."));
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
        //    }
        //}

        /// <summary>
        /// Lấy danh sách bác sỹ theo loại bệnh
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("API/GetListDoctor/{token}")]
        public IHttpActionResult GetListAccountWithType(VM_Req_GetDoctor req, string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth("Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                #region Get list doctor with radius
                double condition_radius = 0;
                decimal global_min_fee_doctor = 0;
                decimal.TryParse(CMS_Lib.Resource("global_min_fee_doctor"), out global_min_fee_doctor);
                double.TryParse(CMS_Lib.Resource("global_condition_radius"), out condition_radius);
                var patient = _context.Accounts.SingleOrDefault(x => x.TokenLogin.Equals(token));
                //update lat lng patient
                patient.Lat = req.Lat;
                patient.Lng = req.Lng;
                _context.SaveChanges();

                List<Account> lstDoctor;
                if (req.MajorCode != null || req.MajorCode > 0)
                {
                    lstDoctor = _context.Accounts.Where(x => x.ProductId == req.MajorCode && x.GroupId == 1 && x.Balance >= global_min_fee_doctor && x.IsActive == true && x.IsApprove == true).ToList();
                }
                else
                {
                    lstDoctor = _context.Accounts.Where(x => x.GroupId == 1 && x.Balance > global_min_fee_doctor && x.IsActive == true && x.IsApprove == true).ToList();
                }
                List<Account> tmp = new List<Account>();
                var radius = 0.0;
                foreach (var item in lstDoctor)
                {
                    //radius = CMS_Lib.Radius(patient.Lat, patient.Lng, item.Lat, item.Lng) * 0.621371192;
                    radius = CMS_Lib.DistanceTo(double.Parse(patient.Lat), double.Parse(patient.Lng), double.Parse(item.Lat), double.Parse(item.Lng));

                    if (radius <= condition_radius)
                    {
                        tmp.Add(item);
                    }
                }
                #endregion
                var list = tmp.Select(x => new VM_Res_Account
                {
                    FullName = x.FullName,
                    MajorName = x.Product.name,
                    Fee = x.Product.price.ToString(),
                    Lat = x.Lat,
                    Lng = x.Lng,
                    ThumbnailUrl = string.IsNullOrEmpty(x.Thumbnail) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.Thumbnail,
                    OrderCount = (int)x.OrderCount,
                    GUID = x.GUID,
                    MajorCode = x.Product.id,
                    ThumbnailIDCard = string.IsNullOrEmpty(x.ThumbnailIDCard) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailIDCard,
                    ThumbnailLicense = string.IsNullOrEmpty(x.ThumbnailLicense) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailLicense,
                    Radius = condition_radius
                }).ToList();
                return Ok(responseAccount.Ok(list, "Lấy danh sách bác sỹ thành công."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Lấy thông tin bác sỹ
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("API/GetDoctor/GUID/{token}")]
        public IHttpActionResult GetDoctor(string GUID, string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth("Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                var list = _context.Accounts.Where(x => x.GroupId == 1 && x.IsActive == true && x.GUID.Equals(GUID) && x.IsApprove == true).Select(x => new VM_Res_Account
                {
                    FullName = x.FullName,
                    MajorName = x.Product.name,
                    Fee = x.Product.price.ToString(),
                    Lat = x.Lat,
                    Lng = x.Lng,
                    ThumbnailUrl = string.IsNullOrEmpty(x.Thumbnail) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.Thumbnail,
                    OrderCount = (int)x.OrderCount,
                    GUID = x.GUID,
                    MajorCode = x.Product.id,
                    ThumbnailIDCard = string.IsNullOrEmpty(x.ThumbnailIDCard) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailIDCard,
                    ThumbnailLicense = string.IsNullOrEmpty(x.ThumbnailLicense) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailLicense
                }).ToList();
                return Ok(responseAccount.Ok(list, "Lấy thông tin bác sỹ thành công"));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Đổi mật khẩu tài khoản
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("API/Accounts/ChangePassword/{token}")]
        public IHttpActionResult ChangePassword(string Password, string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth("Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                
                if (!_context.Accounts.Any(x => x.TokenLogin.Contains(token)))
                {
                    return Ok(response.BadRequest("Tài khoản không tồn tại trong hệ thống. Xin vui lòng kiểm tra lại email hoặc số điện thoại."));
                }
                var tmp = _context.Accounts.SingleOrDefault(x => x.TokenLogin.Equals(token));

                tmp.Password = Password;
                _context.SaveChanges();
                var DateOfBirth = ((DateTime)tmp.BirthDay).ToString("dd/MM/yyyy");
                var dataResponse = _context.Accounts.Where(x => x.TokenLogin.Equals(token)).Select(x => new VM_Res_Account_Short
                {
                    FullName = x.FullName,
                    Token = x.TokenLogin,
                    IsDoctor = x.GroupId == 1 ? true : false,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    IDCardNumber = x.IDCard,
                    DateOfBirth = DateOfBirth,
                    Sex = (int)x.Sex,
                    UserName = string.IsNullOrEmpty(x.Email) == true ? x.PhoneNumber : x.Email,
                    Active = (bool) x.IsActive,
                    ImgIdCard = string.IsNullOrEmpty(x.ThumbnailIDCard) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailIDCard,
                    ImgLicenseId = string.IsNullOrEmpty(x.ThumbnailLicense) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailLicense,
                    ImgAvatar = string.IsNullOrEmpty(x.Thumbnail) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.Thumbnail,
                    MajorCode = x.ProductId,
                    MajorName = x.Product.name
                }).SingleOrDefault();
                return Ok(response.Ok(dataResponse, "Thay đổi mật khẩu thành công."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Thay đổi trạng thái bác sỹ
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("API/Accounts/ChangeStatusDoctor/{token}")]
        public IHttpActionResult ChangeStatusDoctor(VM_Req_Status_Doctor req, string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth("Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                var acc = _context.Accounts.FirstOrDefault(x=>x.TokenLogin.Equals(token));
                req.ConvertModelToData(acc);
                _context.SaveChanges();
                var DateOfBirth = ((DateTime)acc.BirthDay).ToString("dd/MM/yyyy");
                var dataResponse = _context.Accounts.Where(x => x.TokenLogin.Equals(token)).Select(x => new VM_Res_Account_Short
                {
                    FullName = x.FullName,
                    Token = x.TokenLogin,
                    IsDoctor = x.GroupId == 1 ? true : false,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    IDCardNumber = x.IDCard,
                    DateOfBirth = DateOfBirth,
                    Sex = (int)x.Sex,
                    UserName = string.IsNullOrEmpty(x.Email) == true ? x.PhoneNumber : x.Email,
                    Active = (bool) x.IsActive,
                    ImgIdCard = string.IsNullOrEmpty(x.ThumbnailIDCard) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailIDCard,
                    ImgLicenseId = string.IsNullOrEmpty(x.ThumbnailLicense) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailLicense,
                    ImgAvatar = string.IsNullOrEmpty(x.Thumbnail) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.Thumbnail,
                    MajorCode = x.ProductId,
                    MajorName = x.Product.name
                }).SingleOrDefault();

                return Ok(response.Ok(dataResponse, "Thay trang thái của bác sỹ thành công."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Đổi ảnh đại diện người dùng
        /// </summary>
        /// <param name="strImg"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("API/Accounts/UpdateImage/{token}")]
        public IHttpActionResult UploadImg(VM_Account_Thumb strImg, string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth("Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                var acc = _context.Accounts.SingleOrDefault(x => x.TokenLogin.Equals(token));
                acc.Thumbnail = CMS_Image.ConvertBase64ToImage(strImg.ImageAvatar, "CardID-" + acc.PhoneNumber + "-" + CMS_Security.MD5(DateTime.Now.ToString()));
                _context.SaveChanges();
                var DateOfBirth = ((DateTime)acc.BirthDay).ToString("dd/MM/yyyy");
                var dataResponse = _context.Accounts.Where(x => x.TokenLogin.Equals(token)).Select(x => new VM_Res_Account_Short
                {
                    FullName = x.FullName,
                    Token = x.TokenLogin,
                    IsDoctor = x.GroupId == 1 ? true : false,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    IDCardNumber = x.IDCard,
                    DateOfBirth = DateOfBirth,
                    Sex = (int)x.Sex,
                    UserName = string.IsNullOrEmpty(x.Email) == true ? x.PhoneNumber : x.Email,
                    Active = (bool)x.IsActive,
                    ImgIdCard = string.IsNullOrEmpty(x.ThumbnailIDCard) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailIDCard,
                    ImgLicenseId = string.IsNullOrEmpty(x.ThumbnailLicense) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.ThumbnailLicense,
                    ImgAvatar = string.IsNullOrEmpty(x.Thumbnail) == true ? "" : "http://" + HttpContext.Current.Request.Url.Host + "/Uploads/" + x.Thumbnail,
                    MajorCode = x.ProductId,
                    MajorName = x.Product.name
                }).SingleOrDefault();
                return Ok(response.Ok(dataResponse, "Thay đổi ảnh đại diện thành công."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Kiểm tra số điện thoại trong hệ thống
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("API/Accounts/VerifyAccount")]
        [HttpPost]
        public IHttpActionResult VerifyAccount(VM_Account_Req_Verify req)
        {
            try
            {
                Response<VM_Account_Res_Verify> response = new Response<VM_Account_Res_Verify>();
                if (string.IsNullOrEmpty(req.Username))
                {
                    return Ok(response.BadRequest("Số điện thoại hoặc email không hợp lệ. Vui lòng kiểm tra lại"));
                }
                if (_context.Accounts.Any(x => x.PhoneNumber.Equals(req.Username) || x.Email.ToUpper().Equals(req.Username.ToUpper())))
                {
                    var acc = _context.Accounts.SingleOrDefault(x => x.PhoneNumber.Equals(req.Username) || x.Email.ToUpper().Equals(req.Username.ToUpper()));
                    if (acc != null)
                    {
                        acc.TokenAutoLogin = CMS_Security.SHA1(acc.DeviceToken + "-" + acc.Password + "-" + acc.PhoneNumber);
                        acc.TokenLogin = CMS_Security.GenerateGUID().ToString();
                        acc.ExpireTokenLogin = DateTime.Now.AddMinutes(10);
                        _context.SaveChanges();
                        
                    }
                    var res = _context.Accounts.Where(x => x.PhoneNumber.Equals(req.Username) || x.Email.ToUpper().Equals(req.Username.ToUpper())).Select(x=> new VM_Account_Res_Verify {
                        Token = x.TokenLogin
                    }).SingleOrDefault();
                    return Ok(response.Ok(res, "Xác thực tài khoản thành công."));
                }
                return Ok(response.Ok(null, "Tài khoản không tồn tại trong hệ thống.", false));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }
        #endregion
    }
}
