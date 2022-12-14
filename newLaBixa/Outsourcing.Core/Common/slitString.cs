using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Core.Common
{
    public static class slitString
    {
        public static string TruncateAtWord(this string input, int length)
        {
            if (input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ", length);
            return string.Format("{0}...", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }

        //viet ham static trong nay thoi chu af ^^
        public static string ToTimeString(int second)
        {
            TimeSpan t = TimeSpan.FromSeconds(second);
            return t.Hours + ":" + t.Minutes + ":" + t.Seconds;
        }

    }
}
