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
        [Route("Blogs/About")]
        [HttpGet]
        public IHttpActionResult About()
        {
            Response<VM_Response_Blogs> response = new Response<VM_Response_Blogs>();
            try
            {
                var link = "";
                if (string.IsNullOrEmpty(_context.Blogs.Single(x => x.ID == 1).Content))
                {
                    link = System.Configuration.ConfigurationManager.AppSettings["base_url"];
                }
                else
                {
                    link = System.Configuration.ConfigurationManager.AppSettings["base_url"] + "/Web/Home/About";
                }
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
        [Route("Blogs/Terms")]
        [HttpGet]
        public IHttpActionResult Terms()
        {
            Response<VM_Response_Blogs> response = new Response<VM_Response_Blogs>();
            try
            {
                var link = System.Configuration.ConfigurationManager.AppSettings["base_url"] + "/Web/Home/Terms";
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
        [Route("Blogs/Support")]
        [HttpGet]
        public IHttpActionResult Support()
        {
            Response<VM_Response_Blogs> response = new Response<VM_Response_Blogs>();
            try
            {
                var link = System.Configuration.ConfigurationManager.AppSettings["base_url"] + "/Web/Home/Support";
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
