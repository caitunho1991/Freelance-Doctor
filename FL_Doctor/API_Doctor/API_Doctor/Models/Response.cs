using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API_Doctor.Data;
using Newtonsoft.Json;
using Swashbuckle.Swagger;

namespace API_Doctor.Models
{
    public class Response<T>
    {
        public string Code { get; set; }
        public string Status { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
        public string Message { get; set; }
        public string Version { get; set; }

        public Response<T> Ok(T o, string msg="", bool status=true)
        {
            this.Code = "200";
            this.Message = msg;
            this.Data = o;
            this.Status = status==true?"Success":"Fail";
            return this;
        }

        public Response<T> BadRequest(string msg = "")
        {
            this.Code = "400";
            this.Message = msg;
            this.Status = "Bad Request";
            return this;
        }

        public Response<T> NotFound(string msg = "")
        {
            this.Code = "404";
            this.Message = msg;
            this.Status = "Not Found";
            return this;
        }

        public Response<T> NoAuth(string msg = "")
        {
            this.Code = "401";
            this.Message = msg;
            this.Status = "No Authenication";
            return this;
        }
    }
}