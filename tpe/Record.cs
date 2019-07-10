using System;

namespace tpe
{
    public class Record
    {
        public uint Id { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public Time PlannedWorkTime { get; set; }
        public Time RealWorkTime { get; set; }
        public double? Inaccuracy 
        {
            get 
            {
                if (RealWorkTime != null && RealWorkTime.ToString() != "")
                    return (RealWorkTime.ToMinutes() / PlannedWorkTime.ToMinutes());
                else
                    return null;
            }
        }

        public Record(uint id, DateTimeOffset date, Time planned, Time real = default)
        {
            Id = id;
            StartDate = date;
            PlannedWorkTime = planned;
            RealWorkTime = real;
        }

        public Record(uint id, Time planned, Time real = default)
        {
            Id = id;
            StartDate = DateTimeOffset.Now;
            PlannedWorkTime = planned;
            RealWorkTime = real;
        }

        public override string ToString()
        {
            return Id + ","
                + StartDate.ToUnixTimeSeconds() + ","
                + PlannedWorkTime + ","
                + RealWorkTime + ","
                + ((Inaccuracy != null) ? (Math.Round((double)Inaccuracy, 4)).ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")) : "");
        }
    }
}
