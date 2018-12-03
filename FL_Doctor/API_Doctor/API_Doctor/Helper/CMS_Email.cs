using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace API_Doctor.Helper
{
    public class CMS_Email
    {
        public MailMessage mail = new MailMessage();

        public Boolean SendEmail(string title, string content)
        {
            try
            {
                var tmp = System.Configuration.ConfigurationManager.AppSettings["smtp_username"];
                SmtpClient smtp_server = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["smtp_server"]);
                mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["smtp_username"]);
                mail.To.Add("victornguyen305@yahoo.com");
                mail.Subject = title;
                mail.Body = content;
                mail.IsBodyHtml = true;
                smtp_server.Port = 587;
                smtp_server.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["smtp_username"], System.Configuration.ConfigurationManager.AppSettings["smtp_password"]);
                smtp_server.EnableSsl = true;
                smtp_server.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}