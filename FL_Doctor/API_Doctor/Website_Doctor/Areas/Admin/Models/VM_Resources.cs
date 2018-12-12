using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Website_Doctor.Areas.Admin.Data;

namespace Website_Doctor.Areas.Admin.Models
{
    public class VM_Resources
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá trị")]
        public string Value { get; set; }
        public bool Active { get; set; }

        public Resource ConvertModelToData()
        {
            Resource re = new Resource();
            re.Name = this.Name;
            re.Code = this.Code;
            re.Value = this.Value;
            re.IsActive = this.Active;
            return re;
        }

        public VM_Resources ConvertDataToModel(Resource p)
        {
            this.ID = p.ID;
            this.Name = p.Name;
            this.Code = p.Code;
            this.Value = p.Value;
            this.Active = p.IsActive!= true ? false : (bool)p.IsActive;
            return this;
        }
    }
}