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
        public FL_DoctorEntities _context = new FL_DoctorEntities();
    }
}