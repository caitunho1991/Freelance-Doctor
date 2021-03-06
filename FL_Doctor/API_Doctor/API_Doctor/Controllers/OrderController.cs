﻿using API_Doctor.Data;
using API_Doctor.Helper;
using API_Doctor.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_Doctor.Controllers
{
    public class OrderController : BaseController
    {
        Response<List<VM_Order_Respone>> response = new Response<List<VM_Order_Respone>>();
        Response<VM_Order_Respone> responseSingle = new Response<VM_Order_Respone>();

        /// <summary>
        /// Kiểm tra tài khoản là bác sỹ hay bệnh nhân
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Boolean? CheckAccountType(string token)
        {
            var acc = _context.Accounts.First(x=>x.TokenLogin.Equals(token));
            if (acc.Group.Code.Equals("doctor"))
            {
                return true;
            }
            else
            {
                if (acc.Group.Code.Equals("patient"))
                {
                    return false;
                }
            }
            return null;
        }

        /// <summary>
        /// Lấy danh sách toàn bộ đơn hàng
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Order/{token}")]
        public IHttpActionResult GetListOrder(string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth(null, "Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                List<VM_Order_Respone> res = new List<VM_Order_Respone>();
                if (this.CheckAccountType(token) == true)
                {
                   res = _context.Database.SqlQuery<VM_Order_Respone>("exec sp_Doctor_GetListOrder @token={0}", token).ToList();
                }
                else
                {
                    if (this.CheckAccountType(token) == false)
                    {
                        res = _context.Database.SqlQuery<VM_Order_Respone>("exec sp_Patient_GetListOrder @token={0}", token).ToList();
                    }
                }
                
                return Ok(response.Ok(res, "Lấy danh sách đơn hàng thành công."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Lấy danh sách đơn hàng đã được xác nhận
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Order/GetListOrderConfirm/{token}")]
        public IHttpActionResult GetListOrderConfirm(string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth(null, "Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                List<VM_Order_Respone> res = new List<VM_Order_Respone>();
                if (this.CheckAccountType(token) == true)
                {
                    res = _context.Database.SqlQuery<VM_Order_Respone>("exec sp_Doctor_GetListOrderWithStatus @token={0}, @order_status={1}", token, "order_confirm").ToList();
                }
                else
                {
                    if (this.CheckAccountType(token) == false)
                    {
                        res = _context.Database.SqlQuery<VM_Order_Respone>("exec sp_Patient_GetListOrderWithStatus @token={0}, @order_status={1}", token, "order_confirm").ToList();
                    }
                }
                return Ok(response.Ok(res, "Lấy danh sách đơn hàng thành công."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Lấy danh sách đơn hàng hủy
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Order/GetListOrderCancel/{token}")]
        public IHttpActionResult GetListOrderCancel(string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth(null, "Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                List<VM_Order_Respone> res = new List<VM_Order_Respone>();
                if (this.CheckAccountType(token) == true)
                {
                    res = _context.Database.SqlQuery<VM_Order_Respone>("exec sp_Doctor_GetListOrderWithStatus @token={0}, @order_status={1}", token, "order_cancel").ToList();
                }
                else
                {
                    if (this.CheckAccountType(token) == false)
                    {
                        res = _context.Database.SqlQuery<VM_Order_Respone>("exec sp_Patient_GetListOrderWithStatus @token={0}, @order_status={1}", token, "order_cancel").ToList();
                    }
                }
                return Ok(response.Ok(res, "Lấy danh sách đơn hàng thành công."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Lấy danh sách đơn hàng chưa được xác nhận
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Order/GetListOrderNotConfirm/{token}")]
        public IHttpActionResult GetListOrderNotConfirm(string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth(null, "Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                List<VM_Order_Respone> res = new List<VM_Order_Respone>();
                if (this.CheckAccountType(token) == true)
                {
                    res = _context.Database.SqlQuery<VM_Order_Respone>("exec sp_Doctor_GetListOrderWithStatus @token={0}, @order_status={1}", token, "order_create").ToList();
                }
                else
                {
                    if (this.CheckAccountType(token) == false)
                    {
                        res = _context.Database.SqlQuery<VM_Order_Respone>("exec sp_Patient_GetListOrderWithStatus @token={0}, @order_status={1}", token, "order_create").ToList();
                    }
                }
                return Ok(response.Ok(res, "Lấy danh sách đơn hàng thành công."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Order/{OrderNumber}/{token}")]
        public IHttpActionResult GetOrder(string OrderNumber, string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth(null, "Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                var idAccount = _context.Accounts.SingleOrDefault(x => x.TokenLogin.Equals(token)).ID;
                var res = _context.Orders.Where(x => (int)x.idBuyer == idAccount && x.idOrderType == 1 && x.code.Equals(OrderNumber)).Select(x => new VM_Order_Respone
                {
                    OrderNumber = x.code,
                    DateCreate = ((DateTime)x.dateCreate).ToString("dd/MM/yyyy"),
                    DoctorName = x.Account1.FullName,
                    MajorDescription = x.Account1.Product.shortDesc,
                    PatientName = x.Account.FullName,
                    Sex = (int)x.Account.Sex,
                    DateOfBirth = ((DateTime)x.Account.BirthDay).ToString("dd/MM/yyyy"),
                    Address = x.Account.Address,
                    PhoneNumber = x.Account.PhoneNumber
                }).SingleOrDefault();

                return Ok(responseSingle.Ok(res));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Tạo mới đơn hàng
        /// </summary>
        /// <param name="req"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Order/{token}")]
        public IHttpActionResult Order(VM_Order req, string token)
        {
            try
            {
                CMS_Lib.CMS_Logs("API Orer", JsonConvert.SerializeObject(req), "");
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth(null, "Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                if (!_context.Accounts.Any(x=>x.GUID.Equals(req.GuidDoctor)))
                {
                    return Ok(response.BadRequest("Không tìm thấy thông tin bác sỹ"));
                }
                var doctor = _context.Accounts.SingleOrDefault(x => x.GUID.Equals(req.GuidDoctor) && x.IsBanned != true);
                if (doctor == null)
                {
                    return Ok(response.BadRequest("Tài khoản bác sỹ đã bị khóa vui lòng kiểm tra lại"));
                }
                var patient = _context.Accounts.SingleOrDefault(x=>x.TokenLogin.Equals(token));
                var coupon = _context.Coupons.SingleOrDefault(x=>x.Code.ToUpper().Equals(req.Coupon.ToUpper()) && x.Count > 0);
                decimal global_min_fee_doctor = 0;
                decimal.TryParse(CMS_Lib.Resource("global_min_fee_doctor"), out global_min_fee_doctor);
                if (doctor.Balance >= doctor.Product.price)
                {
                    Order o = new Order();
                    //tạo đơn hàng
                    o.idBuyer = patient.ID;
                    o.idReceive = doctor.ID;
                    o.dateCreate = DateTime.Now;
                    o.discount = 0;
                    o.idProduct = doctor.ProductId;
                    o.total = doctor.Product.price;
                    o.totalPay = o.total - o.discount;
                    o.idOrderType = 1;
                    _context.Orders.Add(o);
                    var orderStatus = _context.OrderStatus.SingleOrDefault(x => x.code.Contains("order_create"));
                    o.OrderStatus.Add(orderStatus);
                    _context.SaveChanges();

                    //check coupon
                    if (coupon != null && DateTime.Compare(DateTime.Now, (DateTime)coupon.DateStart) >= 0 && DateTime.Compare((DateTime)coupon.DateEnd, DateTime.Now) >= 0)
                    {
                        if (!_context.OrderCouponAccounts.Any(x => x.AccountID == patient.ID && x.CouponID == coupon.ID))
                        {
                            if (coupon.Value != null)
                            {
                                o.discount = coupon.Value;
                                o.totalPay = o.total - o.discount;
                            }
                            if (coupon.Percent != null)
                            {
                                o.discount = doctor.Product.price * coupon.Percent / 100;
                                o.totalPay = o.total - o.discount;
                            }
                            coupon.Count = coupon.Count - 1;
                            OrderCouponAccount o_tmp = new OrderCouponAccount();
                            o_tmp.AccountID = patient.ID;
                            o_tmp.CouponID = coupon.ID;
                            o_tmp.OrderID = o.id;
                            o_tmp.DateCreate = DateTime.Now;
                            _context.OrderCouponAccounts.Add(o_tmp);
                            _context.SaveChanges();
                        }
                    }
                    //trừ tiền bác sỹ
                    doctor.Balance = doctor.Balance - o.totalPay;
                    //tạo mã đơn hàng
                    o.code = CMS_Security.createTransactionIDString(o.id);
                    o.idOrderStatus = orderStatus.id;
                    _context.SaveChanges();
                    //CMS_Lib.PushNotify(patient.DeviceToken, "PCare", "Lịch hẹn đã được gửi đến bác sỹ.", "patient");
                    CMS_Lib.PushNotify(doctor.DeviceToken, "PCare", "Bạn vừa nhận được một lịch hẹn.", "doctor");
                    var tmp_dateofbirth = ((DateTime)o.dateCreate).ToString("dd/MM/yyyy");
                    var tmp_datecreate = ((DateTime)o.Account.BirthDay).ToString("dd/MM/yyyy");
                    var res = _context.Orders.Where(x => x.code.Contains(o.code)).Select(x => new VM_Order_Respone
                    {
                        OrderNumber = x.code,
                        DateCreate = tmp_datecreate,
                        DoctorName = x.Account1.FullName,
                        MajorDescription = x.Account1.Product.shortDesc,
                        PatientName = x.Account.FullName,
                        Sex = (int)x.Account.Sex,
                        DateOfBirth = tmp_dateofbirth,
                        Address = x.Account.Address,
                        PhoneNumber = x.Account.PhoneNumber
                    }).SingleOrDefault();
                    return Ok(responseSingle.Ok(res, "Đặt đơn hàng thành công."));
                }
                return Ok(response.BadRequest("Tài khoản bác sỹ không đủ để đặt hẹn. Vui lòng chọn bác sỹ khác."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }
        
        /// <summary>
        /// Xác nhận đơn hàng
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Order/Confirm/{OrderNumber}/{token}")]
        public IHttpActionResult ConfirmOrder(string OrderNumber ,string token)
        {
            try
            {

                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth(null, "Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                var idAccount = _context.Accounts.SingleOrDefault(x => x.TokenLogin.Equals(token)).ID;
                var order = _context.Orders.Where(x => (int)x.idReceive == idAccount && x.idOrderType == 1 && x.code.Equals(OrderNumber)).SingleOrDefault();
                var tmp_dateofbirth = ((DateTime)order.dateCreate).ToString("dd/MM/yyyy");
                var tmp_datecreate = ((DateTime)order.Account.BirthDay).ToString("dd/MM/yyyy");
                var res = _context.Orders.Where(x => (int)x.idReceive == idAccount && x.idOrderType == 1 && x.code.Equals(OrderNumber)).Select(x => new VM_Order_Respone
                {
                    OrderNumber = x.code,
                    DateCreate = tmp_datecreate,
                    DoctorName = x.Account1.FullName,
                    MajorDescription = x.Account1.Product.shortDesc,
                    PatientName = x.Account.FullName,
                    Sex = (int)x.Account.Sex,
                    DateOfBirth = tmp_dateofbirth,
                    Address = x.Account.Address,
                    PhoneNumber = x.Account.PhoneNumber
                }).SingleOrDefault();
                if (order.OrderStatus.Count() > 1){
                    return Ok(responseSingle.Ok(res, "Xác nhận đơn hàng không thành công. Vui lòng kiểm tra lại"));
                }
                var order_status = _context.OrderStatus.SingleOrDefault(x=>x.code.Contains("order_confirm"));
                order.OrderStatus.Add(order_status);
                order.idOrderStatus = order_status.id;
                _context.SaveChanges();

                var tokenPatient = _context.Accounts.SingleOrDefault(x=>x.ID == order.idBuyer).DeviceToken;
                var tokenDoctor = _context.Accounts.SingleOrDefault(x => x.ID == order.idReceive).DeviceToken;
                CMS_Lib.PushNotify(tokenPatient, "PCare", "Lịch hẹn "+order.code+" đã được xác nhận thành công.", "patient");
                //CMS_Lib.PushNotify(tokenDoctor, "PCare", "Lịch hẹn " + order.code + " đã được xác nhận thành công.", "doctor");
                return Ok(responseSingle.Ok(res, "Xác nhận đơn hàng thành công."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Hủy đơn hàng
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Order/Cancel/{OrderNumber}/{token}")]
        public IHttpActionResult CancelOrder(string OrderNumber, string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth(null, "Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                var acc = _context.Accounts.SingleOrDefault(x => x.TokenLogin.Equals(token));
                Order order = new Order();
                if (acc.Group.Code.Equals("doctor"))
                {
                    order = _context.Orders.FirstOrDefault(x => x.idReceive == acc.ID && x.idOrderType == 1 && x.code.Equals(OrderNumber));
                    
                }
                if (acc.Group.Code.Equals("patient"))
                {
                    order = _context.Orders.FirstOrDefault(x => x.idBuyer == acc.ID && x.idOrderType == 1 && x.code.Equals(OrderNumber));
                }
                var tmp_dateofbirth = ((DateTime)order.dateCreate).ToString("dd/MM/yyyy");
                var tmp_datecreate = ((DateTime)order.Account.BirthDay).ToString("dd/MM/yyyy");
                var res = _context.Orders.Where(x => x.idReceive == acc.ID && x.idOrderType == 1 && x.code.Equals(OrderNumber)).Select(x => new VM_Order_Respone
                {
                    OrderNumber = x.code,
                    DateCreate = tmp_datecreate,
                    DoctorName = x.Account1.FullName,
                    MajorDescription = x.Account1.Product.shortDesc,
                    PatientName = x.Account.FullName,
                    Sex = (int)x.Account.Sex,
                    DateOfBirth = tmp_dateofbirth,
                    Address = x.Account.Address,
                    PhoneNumber = x.Account.PhoneNumber
                }).SingleOrDefault();
                if (order.OrderStatus.Count() > 1)
                {
                    return Ok(responseSingle.Ok(res, "Hủy đơn hàng không thành công. Xin vui lòng kiểm tra lại.",false));
                }
                //check time ko cho hủy đơn hàng
                int timeout_cancel_order = 0 - int.Parse(CMS_Lib.Resource("global_timeout_cancel_order"));
                if (DateTime.Compare(DateTime.Now.AddMinutes(timeout_cancel_order), (DateTime)order.dateCreate) >= 0)
                {
                    return Ok(responseSingle.Ok(res, "Hủy đơn hàng không thành công. Quá thời gia hủy đơn hàng.", false));
                }
                var order_status = _context.OrderStatus.SingleOrDefault(x => x.code.Equals("order_cancel"));
                order.OrderStatus.Add(order_status);
                order.idOrderStatus = order_status.id;
                //hoàn tiền lại cho bác sỹ
                var account = _context.Accounts.SingleOrDefault(x=>x.ID == order.idReceive);
                account.Balance = account.Balance + order.totalPay;
                _context.SaveChanges();
                if (acc.GroupId == 1)
                {
                    var tokenPatient = _context.Accounts.SingleOrDefault(x => x.ID == order.idBuyer).DeviceToken;
                    CMS_Lib.PushNotify(tokenPatient, "PCare", "Lịch hẹn " + order.code + " đã được hủy", "patient");
                }
                if (acc.GroupId == 2)
                {
                    var tokenDoctor = _context.Accounts.SingleOrDefault(x => x.ID == order.idReceive).DeviceToken;
                    CMS_Lib.PushNotify(tokenDoctor, "PCare", "Lịch hẹn " + order.code + " đã được hủy", "doctor");
                }
                
                
                return Ok(responseSingle.Ok(res, "Hủy đơn hàng thành công."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Giao dịch rút tiền
        /// </summary>
        /// <param name="req"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Order/CashWithDrawal/{token}")]
        public IHttpActionResult CashWithDrawal(VM_CashWithDrawal req, string token)
        {
            try
            {
                Response<VN_Response_CashWithDrawal> response = new Response<VN_Response_CashWithDrawal>();
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth(null, "Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                if (!ModelState.IsValid)
                {
                    return Ok(response.BadRequest(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
                }
                var account = _context.Accounts.SingleOrDefault(x => x.TokenLogin.Equals(token));
                if (account.Group.Code.Equals("doctor"))
                {
                    var order = req.ConvertModel();
                    order.idBuyer = account.ID;
                    order.idReceive = 1;
                    order.idOrderType = 2;
                    if (order.totalPay <= 0)
                    {
                        return Ok(response.BadRequest("Số tiền rút phải lớn hơn 0. Xin vui lòng kiểm tra lại"));
                    }
                    if (order.totalPay > account.Balance)
                    {
                        return Ok(response.BadRequest("Tài khoản không đủ số dư để rút tiền. Xin vui lòng kiểm tra lại"));
                    }
                    _context.Orders.Add(order);
                    account.Balance = account.Balance - order.totalPay;
                    var orderStatus = _context.OrderStatus.SingleOrDefault(x => x.code.Contains("cashwithdrawal_create"));
                    order.OrderStatus.Add(orderStatus);
                    _context.SaveChanges();
                    order.code = CMS_Security.createTransactionIDString(order.id);
                    _context.SaveChanges();
                    var displayTotalPay = string.Format("{0:0,000 vnd}", order.totalPay);
                    var res = _context.Orders.Where(x => x.code.Equals(order.code)).Select(x => new VN_Response_CashWithDrawal
                    {
                        Card_Bank = x.card_bank,
                        Card_FullName = x.card_fullname,
                        Card_Number = x.card_number,
                        Total = (decimal)x.total,
                        DisplayTotal = displayTotalPay,
                        Note = x.note,
                        OrderNumber = x.code
                    }).SingleOrDefault();

                    return Ok(response.Ok(res, "Lệnh rút tiền đã được thực hiện. Xin cám ơn."));
                }
                return Ok(response.BadRequest("Tài khoản của bạn không thỏa điều kiện rút tiền. Xin vui lòng kiểm tra lại."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }

        /// <summary>
        /// Giao dịch nạp tiền
        /// </summary>
        /// <param name="req"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Order/Recharge/{token}")]
        public IHttpActionResult Recharge(string token)
        {
            try
            {
               Response<VN_Response_CashWithDrawal> response = new Response<VN_Response_CashWithDrawal>();
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth(null, "Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                if (!ModelState.IsValid)
                {
                    return Ok(response.BadRequest(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
                }
                var account = _context.Accounts.SingleOrDefault(x=>x.TokenLogin.Equals(token));
                if (account.Group.Code.Equals("doctor"))
                {
                    var req = new VM_CashWithDrawal();
                    var order = req.ConvertModel();
                    order.idOrderType = 3;
                    order.idBuyer = account.ID;
                    order.idReceive = 1;
                    order.idProduct = null;
                    if (order.totalPay <= 0)
                    {
                        return Ok(response.BadRequest("Số tiền nạp phải lớn hơn 0. Xin vui lòng kiểm tra lại"));
                    }
                    _context.Orders.Add(order);
                    var orderStatus = _context.OrderStatus.SingleOrDefault(x => x.code.Contains("recharge_create"));
                    order.OrderStatus.Add(orderStatus);
                    _context.SaveChanges();
                    order.code = CMS_Security.createTransactionIDString(order.id);
                    _context.SaveChanges();

                    var displayTotalPay = string.Format("{0:0,000 vnd}", order.totalPay);
                    var res = _context.Orders.Where(x => x.code.Equals(order.code)).Select(x => new VN_Response_CashWithDrawal
                    {
                        Card_Bank = x.card_bank,
                        Card_FullName = x.card_fullname,
                        Card_Number = x.card_number,
                        Total = (decimal)x.total,
                        DisplayTotal = displayTotalPay,
                        Note = x.note,
                        OrderNumber = x.code
                    }).SingleOrDefault();

                    return Ok(response.Ok(res, "Lệnh nạp tiền đã được thực hiện. Xin cám ơn."));
                }
                return Ok(response.BadRequest("Tài khoản của bạn không thỏa điều kiện nạp tiền. Xin vui lòng kiểm tra lại."));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }
        
    }
}
