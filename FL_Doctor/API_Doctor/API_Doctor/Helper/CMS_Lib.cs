using API_Doctor.Data;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Doctor.Helper
{
    public static class CMS_Lib
    {
        public static FL_DoctorEntities _context = new FL_DoctorEntities();
        public static string Resource(string key)
        {
            try
            {
                if (_context.Resources.Any(x=>x.Code.ToLower().Contains(key.ToLower())))
                {
                    return _context.Resources.SingleOrDefault(x=>x.Code.ToLower().Contains(key.ToLower())).Value;
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        //const double PIx = 3.141592653589793;
        //const double RADIUS = 6378.16;
        //public static double Radians(double x)
        //{
        //    return x * PIx / 180;
        //}
        //public static double Radius(string lat_1, string lng_1, string lat_2, string lng_2)
        //{
        //    try
        //    {
        //        var dlat = double.Parse(lat_2) - double.Parse(lat_1);
        //        var dlng = double.Parse(lng_2) - double.Parse(lng_1);
        //        var tmp = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(double.Parse(lat_1))) * Math.Cos(Radians(double.Parse(lat_2))) * (Math.Sin(dlng / 2) * Math.Sin(dlng / 2));
        //        double angle = 2 * Math.Atan2(Math.Sqrt(tmp), Math.Sqrt(1 - tmp));
        //        var radius =  angle * RADIUS;
        //        //var a = Math.Pow(Math.Sin(dlat) / 2, 2) + Math.Cos(double.Parse(lng_2)) * Math.Cos(double.Parse(lng_1)) * Math.Pow(Math.Sin(dlng) / 2, 2);
        //        //var b = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        //        //var radius = 6371 * b;

        //        return radius;
        //    }
        //    catch (Exception e)
        //    {
        //        return 0;
        //    }
        //}


        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }
        public static bool CheckPhoneNumber(string phoneNumber, string countryCode = "VN")
        {
            try
            {
                PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
                var a = phoneUtil.Parse(phoneNumber, "VN");
                PhoneNumber _phoneNumber = phoneUtil.ParseAndKeepRawInput(phoneNumber, "VN");
                return phoneUtil.IsValidNumber(_phoneNumber);
            }
            catch
            {
                return false;
            }
        }


        public static bool PushNotify()
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("~/App_Start/Firebase.json"),
            });
            return true;
        }
    }
}