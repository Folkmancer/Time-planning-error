using System;
using System.Text;
using System.Text.RegularExpressions;


namespace tpe
{
    public class TimeParser
    {
        private static string regDays = @"(\d+(?=[dD]))";
        private static string regHours = @"(\d+(?=[hH]))";
        private static string regMinuts = @"(\d+(?=[mM]))";
        private static string regexTime = @"(\b\d+[dD]\b)|(\b\d+[hH]\b)|(\b\d+[mM]\b)|(\b\d+[dD]\d+[hH]\b)|(\b\d+[dD]\d+[mM]\b)|(\b\d+[hH]\d+[mM]\b)|(\b\d+[dD]\d+[hH]\d+[mM]\b)";

        private static int hours;
        private static int minutes;

        public static int Days { get; set; }

        public static int Hours 
        {
            get => hours;
            set {
                if (value > 7)
                {
                    Days += value / 8;
                    hours = value % 8;
                }
                else
                    hours = value;
            }
        }

        public static int Minutes 
        {
            get => minutes;
            set 
            {
                if (value > 59)
                {
                    hours += value / 60;
                    minutes = value % 60;
                }
                else
                    minutes = value;
            }
        }

        public static bool TryParse(string timeIn, out TimeSpan timeOut)
        {
            timeOut = new TimeSpan();
            if (Regex.IsMatch(timeIn, regexTime) || timeIn == "")
            {
                Match matchDays = Regex.Match(timeIn, regDays);
                Match matchHours = Regex.Match(timeIn, regHours);
                Match matchMinuts = Regex.Match(timeIn, regMinuts);
                Minutes = matchMinuts.Success ? int.Parse(matchMinuts.Value) : 0;
                Hours = matchHours.Success ? int.Parse(matchHours.Value) : 0;
                Days = matchDays.Success ? int.Parse(matchDays.Value) : 0;
                timeOut = TimeSpan.FromDays(Days) + TimeSpan.FromHours(Hours) + TimeSpan.FromMinutes(Minutes);
                return true;
            }
            else
                return false;   
        }

        public static TimeSpan Parse(string timeIn)
        {
            if (!TryParse(timeIn, out TimeSpan timeOut))
                throw new FormatException();
            else
                return timeOut;
        }

        public static string ToCSV(TimeSpan timeIn)
        {
            StringBuilder temp = new StringBuilder();
            if (timeIn.Days != 0) temp.Append(timeIn.Days + "d");
            if (timeIn.Hours != 0) temp.Append(timeIn.Hours + "h");
            if (timeIn.Minutes != 0) temp.Append(timeIn.Minutes + "m");
            return temp.ToString();
        }

        public static string ToString(TimeSpan timeIn)
        {
            string temp = ToCSV(timeIn);
            return (temp.Length != 0) ? temp.ToString() : "unknown";
        }
    }
}
