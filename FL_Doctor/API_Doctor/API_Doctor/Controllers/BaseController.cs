﻿using API_Doctor.Data;
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
    public class BaseController : ApiController
    {
        public FL_DoctorEntities _context = new FL_DoctorEntities();
        public CMS_Email email = new CMS_Email();

        protected Boolean checkAuth(string token)
        {
            var acc = _context.Accounts.SingleOrDefault(x=>x.TokenLogin.Equals(token));
            if (!string.IsNullOrEmpty(token) && acc != null)
            {
                if(DateTime.Compare((DateTime)acc.ExpireTokenLogin, DateTime.Now) > 0)
                {
                    acc.ExpireTokenLogin = DateTime.Now.AddMinutes(30);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
