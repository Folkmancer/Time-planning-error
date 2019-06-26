using System;
using System.Text;
using System.Text.RegularExpressions;


namespace tpe
{
    public class Time
    {
        private static string regDays = @"(\d+(?=[dD]))";
        private static string regHours = @"(\d+(?=[hH]))";
        private static string regMinuts = @"(\d+(?=[mM]))";
        private static string regexTime = @"(\b\d+[dD]\b)|(\b\d+[hH]\b)|(\b\d+[mM]\b)|(\b\d+[dD]\d+[hH]\b)|(\b\d+[dD]\d+[mM]\b)|(\b\d+[hH]\d+[mM]\b)|(\b\d+[dD]\d+[hH]\d+[mM]\b)";

        private int days;
        private int hours;
        private int minutes;

        public int Days { get => days; set => days = value; }
        public int Hours 
        {
            get => hours;
            set {
                if (value > 7)
                {
                    days += value / 8;
                    hours = value % 8;
                }
                else
                    hours = value;
            }
        }
        public int Minutes 
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

        public static bool TryParse(string timeIn, out Time timeOut)
        {
            timeOut = new Time();
            if (Regex.IsMatch(timeIn, regexTime) || timeIn == "")
            {
                Match matchDays = Regex.Match(timeIn, regDays);
                Match matchHours = Regex.Match(timeIn, regHours);
                Match matchMinuts = Regex.Match(timeIn, regMinuts);
                timeOut.Days = matchDays.Success ? int.Parse(matchDays.Value) : 0;
                timeOut.Hours = matchHours.Success ? int.Parse(matchHours.Value) : 0;
                timeOut.Minutes = matchMinuts.Success ? int.Parse(matchMinuts.Value) : 0;
                return true;
            }
            else
                return false;   
        }

        public static Time Parse(string timeIn)
        {
            if (!TryParse(timeIn, out Time timeOut))
                throw new FormatException();
            else
                return timeOut;
        }

        public double ToMinutes()
        {
            return (Days * 8 * 60) + Hours * 60 + Minutes;
        }

        public override string ToString()
        {
            StringBuilder temp = new StringBuilder();
            if (Days != 0) temp.Append(Days + "d");
            if (Hours != 0) temp.Append(Hours + "h");
            if (Minutes != 0) temp.Append(Minutes + "m");
            return (temp != null) ? temp.ToString() : "unknown";
        }
    }
}
