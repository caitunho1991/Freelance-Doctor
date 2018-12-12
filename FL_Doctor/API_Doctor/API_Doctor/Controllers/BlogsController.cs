using API_Doctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace API_Doctor.Controllers
{
    public class BlogsController : BaseController
    {
        /// <summary>
        /// Lấy link nội dung policy
        /// </summary>
        /// <returns></returns>
        [Route("API/Blogs/Policy")]
        [HttpGet]
        public IHttpActionResult Policy()
        {
            Response<string> response = new Response<string>();
            try
            {
                return Ok(response.Ok("http://" + System.Configuration.ConfigurationManager.AppSettings["domain"] + "/Home/Policy"));
                
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }
        /// <summary>
        /// Lấy link nội dung terms
        /// </summary>
        /// <returns></returns>
        [Route("API/Blogs/Terms")]
        [HttpGet]
        public IHttpActionResult Terms()
        {
            Response<string> response = new Response<string>();
            try
            {
                return Ok(response.Ok("http://" + System.Configuration.ConfigurationManager.AppSettings["domain"] + "/Home/Terms"));

            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }
        /// <summary>
        /// Lấy link nội dung support
        /// </summary>
        /// <returns></returns>
        [Route("API/Blogs/Support")]
        [HttpGet]
        public IHttpActionResult Support()
        {
            Response<string> response = new Response<string>();
            try
            {
                return Ok(response.Ok("http://" + System.Configuration.ConfigurationManager.AppSettings["domain"] + "/Home/Support"));

            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }
    }
}
