using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Website_Doctor.Areas.Admin.Helpers
{
    public class CMS_Lib
    {
        public static string ConvertString(string stringInput)
        {
            stringInput = stringInput.ToLower();
            string convert = "ĂÂÀẰẦÁẮẤẢẲẨÃẴẪẠẶẬỄẼỂẺÉÊÈỀẾẸỆÔÒỒƠỜÓỐỚỎỔỞÕỖỠỌỘỢƯÚÙỨỪỦỬŨỮỤỰÌÍỈĨỊỲÝỶỸỴĐăâàằầáắấảẳẩãẵẫạặậễẽểẻéêèềếẹệôòồơờóốớỏổởõỗỡọộợưúùứừủửũữụựìíỉĩịỳýỷỹỵđ";
            string To = "aaaaaaaaaaaaaaaaaeeeeeeeeeeeooooooooooooooooouuuuuuuuuuuiiiiiyyyyydaaaaaaaaaaaaaaaaaeeeeeeeeeeeooooooooooooooooouuuuuuuuuuuiiiiiyyyyyd";
            for (int i = 0; i < To.Length; i++)
            {
                stringInput = stringInput.Replace(convert[i], To[i]);
            }
            stringInput = stringInput.Replace("-", "");
            stringInput = stringInput.Replace("&", "");
            stringInput = stringInput.Replace(@"""", "");
            stringInput = stringInput.Replace("/", "");
            stringInput = stringInput.Replace("(", "");
            stringInput = stringInput.Replace(")", "");
            stringInput = stringInput.Replace("\\", "");
            stringInput = stringInput.Replace(".", "-");
            stringInput = stringInput.Replace(" ", "-");
            stringInput = stringInput.Replace(",", "-");
            stringInput = stringInput.Replace(";", "-");
            stringInput = stringInput.Replace(":", "-");
            stringInput = stringInput.Replace("'", "");
            stringInput = stringInput.Replace("?", "");
            stringInput = stringInput.Replace("%", "-");

            return stringInput;

        }

        /// <summary>
        /// Hash MD5 string
        /// </summary>
        /// <param name="str"></param>
        /// <returns>String </returns>
        public static string MD5(string str)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }


        // <summary>
        /// Generate GUID
        /// </summary>
        /// <returns>String: 0f8fad5b-d9cb-469f-a165-70867728950e</returns>
        public static String GenerateGUID()
        {
            var GUID = Guid.NewGuid().ToString();
            return GUID;
        }
    }
}