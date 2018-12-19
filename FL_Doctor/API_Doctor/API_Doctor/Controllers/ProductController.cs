using API_Doctor.Helper;
using API_Doctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_Doctor.Controllers
{
    public class ProductController : BaseController
    {
        #region Example
        //// GET: Product
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: Product/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: Product
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: Product/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: Product/5
        //public void Delete(int id)
        //{
        //}
        #endregion
        public Response<List<VM_Res_Product>> response = new Response<List<VM_Res_Product>>();
        /// <summary>
        /// Lấy danh sách chuyên khoa khám bệnh của bác sỹ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListMajor")]
        public IHttpActionResult GetListMajor()
        {
            try
            {
                var res = _context.Products.Where(x => x.isActive == true).Select(y => new VM_Res_Product
                {
                    MayjorCode = y.id,
                    MayjorName = y.name,
                    MayjorDescription = y.shortDesc
                }).ToList();
                return Ok(response.Ok(res));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }


        [HttpPost]
        [Route("Test")]
        public IHttpActionResult Test(string deviceToken)
        {

            try
            {
                var a = CMS_Lib.PushNotify(deviceToken, "PCare Title", "PCare Body", "patient");
                return BadRequest(a);
            }
            catch (Exception e)
            {
                return BadRequest("! Ok");
            }
        }
    }
}
