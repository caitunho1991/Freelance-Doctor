using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}