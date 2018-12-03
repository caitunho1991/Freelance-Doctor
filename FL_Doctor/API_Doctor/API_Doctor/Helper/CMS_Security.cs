using System;
using System.Security.Cryptography;
using System.Text;

namespace API_Doctor.Helper
{
    public static class CMS_Security
    {
        /// <summary>
        /// Generate GUID
        /// </summary>
        /// <returns>String: 0f8fad5b-d9cb-469f-a165-70867728950e</returns>
        public static String GenerateGUID()
        {
            var GUID = Guid.NewGuid().ToString();
            return GUID;
        }

        /// <summary>
        /// Hash SHA1 string
        /// </summary>
        /// <param name="str"></param>
        /// <returns>String: 0C2E99D0949684278C30B9369B82638E1CEAD415</returns>
        public static string SHA1(string str)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(str));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
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
        /// Create Transaction Number
        /// </summary>
        /// <param name="transactionID"></param>
        /// <returns></returns>
        public static string createTransactionIDString(long transactionID)
        {
            string begin = "Order_";
            int lenght = 10;
            string result = begin + transactionID.ToString().PadLeft(lenght, '0');
            return result;

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
