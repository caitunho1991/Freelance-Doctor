using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_Doctor.Areas.Admin.Data;

namespace Website_Doctor.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public FL_DoctorEntities _context;
        public string Fullname;
        public int ID;
        public string URL;
        public bool CheckAuth()
        {
            //check token
            HttpCookie cookie = HttpContext.Request.Cookies.Get("TokenLogin");
            HttpCookie a = Request.Cookies.Get("TokenLogin");
            if (cookie == null)
            {
                return false;
            }

            var token = cookie.Value;
            if (!_context.Accounts.Any(x => x.TokenLogin.Equals(token)))
            {
                return false;
            }
            
            var acc = _context.Accounts.Single(x => x.TokenLogin.Equals(token));
            if (DateTime.Compare((DateTime)acc.ExpireTokenLogin, DateTime.Now) < 0)
            {
                return false;
            }
            int token_time = 0;
            int.TryParse(_context.Resources.Single(x => x.Code.Equals("cms_token_time")).Value, out token_time);
            acc.ExpireTokenLogin = DateTime.Now.AddDays(token_time);
            cookie.Expires = (DateTime)acc.ExpireTokenLogin;
            _context.SaveChanges();

            this.Fullname = acc.FullName;
            this.ID = acc.ID;
            this.URL = acc.Thumbnail;
            return true;
        }
        public BaseController()
        {
            this._context = new FL_DoctorEntities();
            
        }
    }
}