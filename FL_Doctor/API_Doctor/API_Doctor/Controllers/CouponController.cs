using API_Doctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_Doctor.Controllers
{
    public class CouponController : BaseController
    {
        Response<VM_Response_Coupon> response = new Response<VM_Response_Coupon>();
        /// <summary>
        /// Lấy thông tin chương trình khuyến mãi
        /// </summary>
        /// <param name="CouponCode">Mã giảm giá</param>
        /// <param name="token">Token</param>
        /// <returns>
        /// <param name="CouponDescription">Thông tin chương trình khuyến mãi</param>
        /// <param name="CouponCode">Giá trị giảm giá chương trình khuyến mãi</param>
        /// <param name="IsPercent">IsPercent = true => giá trị giảm giá là %, ngược lại là vnđ</param>
        /// </returns>
        [HttpGet]
        [Route("API/Coupon/{CouponCode}/{token}")]
        public IHttpActionResult GetCoupon(string CouponCode, string token)
        {
            try
            {
                if (!checkAuth(token))
                {
                    return Ok(response.NoAuth("Tài khoản không đúng hoặc không có quyền truy cập. Vui lòng kiểm tra lại."));
                }
                var coupon = _context.Coupons.SingleOrDefault(x => x.Code.Equals(CouponCode));
                var acc = _context.Accounts.SingleOrDefault(x=>x.TokenLogin.Equals(token));
                if (coupon != null)
                {
                    //this statement check the coupon has expiry date
                    if (DateTime.Compare(DateTime.Now, (DateTime)coupon.DateStart) > 0 && DateTime.Compare(DateTime.Now, (DateTime)coupon.DateEnd) < 0)
                    {
                        if (!_context.OrderCouponAccounts.Any(x => x.AccountID == acc.ID && x.CouponID == coupon.ID))
                        {
                            VM_Response_Coupon res = _context.Coupons.Where(x => x.Code.Equals(CouponCode)).Select(x => new VM_Response_Coupon
                            {
                                CouponDescription = x.Description,
                                CouponValue = (decimal)x.Value == null ? ((decimal)x.Percent == null ? 0 : (decimal)x.Percent) : (decimal)x.Value,
                                IsPercent = (decimal)x.Value == null ? ((decimal)x.Percent == null ? false : true) : false
                            }).SingleOrDefault();
                            return Ok(response.Ok(res, "Lấy thông tin mã giảm giá thánh công."));
                        }
                        return Ok(response.BadRequest("Tài khoản đã sử dụng mã giảm giá này. Vui lòng sử dụng mã giảm giá khác"));
                    }
                    return Ok(response.BadRequest("Mã giảm giá đã hết hạn sử dụng. Vui lòng kiểm tra lại"));
                    
                }

                
                return Ok(response.BadRequest("Mã giảm giá không đúng hoặc không tồn tại. Vui lòng kiểm tra lại"));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }
    }
}
