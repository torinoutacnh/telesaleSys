using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Core.Common
{
    public static class StringConvert
    {

        public static String ConvertShortName(String strVietNamese)
        {
            //Loại bỏ dấu ':'
            char[] delimiter = { ':', '?', '"', '/', '!', ',', '-', '=', '%', '$', '&', '*' };
            String[] subString = strVietNamese.Split(delimiter);
            strVietNamese = "";
            for (int i = 0; i < subString.Length; i++)
            {
                strVietNamese += subString[i];
            }
            //Loại bỏ tiếng việt
            const string textToFind = " áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string textToReplace = "-aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            while ((index = strVietNamese.IndexOfAny(textToFind.ToCharArray())) != -1)
            {
                int index2 = textToFind.IndexOf(strVietNamese[index]);
                strVietNamese = strVietNamese.Replace(strVietNamese[index], textToReplace[index2]);
            }

            return strVietNamese.ToLower();
        }
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public static int ToSeconds(string dateString)
        {
            string[] s = dateString.Split(':');
            if (s.Count() == 3)
            {
                return int.Parse(s[0]) * 3600 + int.Parse(s[1]) * 60 + int.Parse(s[2]);
            }
            else
            {
                if (s.Count() == 2) return int.Parse(s[0]) * 60 + int.Parse(s[1]);
            }
            return int.Parse(s[0]);
        }
        public static string ToAmount(string money)
        {
            string result = "";
            int countUp = 0;
            for (var i = money.Length - 1; i >= 0; i--)
            {
                result = money[i].ToString() + result;
                countUp++;
                if (countUp == 3 && i != 0)
                {
                    result = "," + result;
                    countUp = 0;
                }
            }
            return result;
        }
    }
}
