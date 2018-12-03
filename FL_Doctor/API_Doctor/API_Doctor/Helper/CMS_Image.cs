using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace API_Doctor.Helper
{
    public static class CMS_Image
    {
        public static string ConvertBase64ToImage(string strBase64, string filename)
        {
            try
            {
                String path = HttpContext.Current.Server.MapPath("~/Uploads/"); //Path                                                              //Check if directory exist
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                }
                byte[] imgBytes = Convert.FromBase64String(strBase64);
                
                using (var imageFile = new FileStream(path +
                    filename+".jpg", FileMode.Create))
                {
                    imageFile.Write(imgBytes, 0, imgBytes.Length);
                    imageFile.Flush();
                }
                return filename + ".jpg";
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}