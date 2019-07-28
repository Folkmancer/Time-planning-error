using System;

namespace tpe
{
    public class Record
    {
        public uint Id { get; set; }
        public DateTimeOffset StartWorkDate { get; set; }
        public TimeSpan PlannedWorkTime { get; set; }
        public TimeSpan RealWorkTime { get; set; }
        public double? Inaccuracy 
        {
            get 
            {
                if (!RealWorkTime.Equals(TimeSpan.Zero))
                    return (RealWorkTime.TotalSeconds / PlannedWorkTime.TotalSeconds);
                else
                    return null;
            }
        }

        public Record()
        {

        }

        public Record(uint id, DateTimeOffset date, TimeSpan planned, TimeSpan real = default)
        {
            Id = id;
            StartWorkDate = date;
            PlannedWorkTime = planned;
            RealWorkTime = real;
        }

        public Record(uint id, TimeSpan planned, TimeSpan real = default)
        {
            Id = id;
            StartWorkDate = DateTimeOffset.Now;
            PlannedWorkTime = planned;
            RealWorkTime = real;
        }

        public static bool TryParse(string[] args, out Record record)
        {
            record = new Record();
            if (uint.TryParse(args[0], out uint id) && DateTime.TryParse(args[1], out DateTime date) && TimeParser.TryParse(args[2], out TimeSpan planned) && TimeParser.TryParse(args[3], out TimeSpan real))
            {
                record = new Record(id, date, planned, real);
                return true;
            }
            else if (uint.TryParse(args[0], out id) && DateTime.TryParse(args[1], out date) && TimeParser.TryParse(args[2], out planned))
            {
                record = new Record(id, date, planned);
                return true;
            }
            else if (uint.TryParse(args[0], out id) && TimeParser.TryParse(args[2], out planned) && TimeParser.TryParse(args[3], out real))
            {
                record = new Record(id, planned, real);
                return true;
            }
            else if (uint.TryParse(args[0], out id) && TimeParser.TryParse(args[2], out planned))
            {
                record = new Record(id, planned);
                return true;
            }
            else
                return false;
        }

        public static Record Parse(string[] args)
        {
            if (!TryParse(args, out Record record))
                throw new FormatException();
            else
                return record;
        }

        public string ToCSV()
        {
            return Id + ","
                + StartWorkDate.ToUnixTimeSeconds() + ","
                + TimeParser.ToCSV(PlannedWorkTime) + ","
                + TimeParser.ToCSV(RealWorkTime) + ","
                + ((Inaccuracy != null) ? (Math.Round((double)Inaccuracy, 4)).ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")) : "");
        }
    }
}
