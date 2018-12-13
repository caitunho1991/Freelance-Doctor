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
        /// Về chúng tôi
        /// </summary>
        /// <returns></returns>
        [Route("API/Blogs/About")]
        [HttpGet]
        public IHttpActionResult Policy()
        {
            Response<VM_Response_Blogs> response = new Response<VM_Response_Blogs>();
            try
            {
                var link = "http://" + System.Configuration.ConfigurationManager.AppSettings["domain"] + "/Home/About";
                VM_Response_Blogs res = _context.Blogs.Where(x => x.ID == 1).Select(x=>new VM_Response_Blogs {
                    Title = x.Title,
                    Link = link
                }).Single();
                return Ok(response.Ok(res));
                
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }
        /// <summary>
        /// Điều khoản điều kiện
        /// </summary>
        /// <returns></returns>
        [Route("API/Blogs/Terms")]
        [HttpGet]
        public IHttpActionResult Terms()
        {
            Response<VM_Response_Blogs> response = new Response<VM_Response_Blogs>();
            try
            {
                var link = "http://" + System.Configuration.ConfigurationManager.AppSettings["domain"] + "/Home/Terms";
                VM_Response_Blogs res = _context.Blogs.Where(x => x.ID == 2).Select(x => new VM_Response_Blogs
                {
                    Title = x.Title,
                    Link = link
                }).Single();
                return Ok(response.Ok(res));

            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }
        /// <summary>
        /// Hướng dẫn sử dụng
        /// </summary>
        /// <returns></returns>
        [Route("API/Blogs/Support")]
        [HttpGet]
        public IHttpActionResult Support()
        {
            Response<VM_Response_Blogs> response = new Response<VM_Response_Blogs>();
            try
            {
                var link = "http://" + System.Configuration.ConfigurationManager.AppSettings["domain"] + "/Home/Support";
                VM_Response_Blogs res = _context.Blogs.Where(x => x.ID == 1).Select(x => new VM_Response_Blogs
                {
                    Title = x.Title,
                    Link = link
                }).Single();
                return Ok(response.Ok(res));
            }
            catch (Exception e)
            {
                return Ok(response.BadRequest("Có lỗi trong quá trình xử lý."));
            }
        }
    }
}
