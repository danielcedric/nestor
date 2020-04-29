using System;
using System.Text;

namespace Nestor.Tools.Helpers
{
    public static class TimeHelper
    {
        public static string ToLongTimeString(this TimeSpan ts)
        {
            StringBuilder sbOut = new StringBuilder();
            if(ts.Days > 0)
            {
                double weeks = (double)(ts.Days / 7);
                if (weeks > 1)
                    sbOut.AppendFormat("{0} semaine{1}", Math.Round(weeks), weeks > 1 ? "s" : string.Empty);
            }
            if (ts.Hours >= 1)
                sbOut.AppendFormat("{0} heure{1}{2}", ts.Hours, ts.Hours > 1 ? "s" : string.Empty, ts.Minutes > 0 ? ", " : string.Empty);
            if (ts.Minutes > 0)
                sbOut.AppendFormat("{0} minute{1}", ts.Minutes, ts.Minutes > 1 ? "s" : string.Empty);

            return sbOut.ToString();
        }
    }
}
