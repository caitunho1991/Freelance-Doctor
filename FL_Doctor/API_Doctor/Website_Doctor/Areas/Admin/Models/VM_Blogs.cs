using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_Doctor.Areas.Admin.Data;
using Website_Doctor.Areas.Admin.Helpers;

namespace Website_Doctor.Areas.Admin.Models
{
    public class VM_Blogs
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string ShortDesscription { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string Alias { get; set; }

        public Blog ConvertModelToData()
        {
            Blog re = new Blog();
            re.Title = this.Title;
            re.ShortDescription = this.ShortDesscription;
            re.Content = this.Content;
            re.Alias = CMS_Lib.ConvertString(this.Title);
            return re;
        }

        public VM_Blogs ConvertDataToModel(Blog p)
        {
            this.ID = p.ID;
            this.Title = p.Title;
            this.ShortDesscription= p.ShortDescription;
            this.Content = p.Content;
            this.Alias = p.Alias;
            return this;
        }
    }
}