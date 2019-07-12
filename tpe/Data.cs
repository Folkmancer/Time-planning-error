using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace tpe
{
    public class Data
    {
        public string Path { get; set; }
        public List<Record> records = new List<Record>();

        public Data(string path)
        {
            Path = path;
        }

        public bool Open(Record record)
        {
            if (!records.Exists(x => x.Id == record.Id))
            {
                records.Add(record);
                return true;
            }
            else
                return false;
        }

        public bool Close(uint id, TimeParser time)
        {
            Record temp = Find(id);
            if (temp != null)
            {
                records[records.IndexOf(temp)].RealWorkTime = time;
                return true;
            }
            else
                return false;
        }

        public bool Close(uint id)
        {
            Record temp = Find(id);
            if (temp != null)
            {
                TimeParser time = new TimeParser();
                DateTime startDate = temp.StartDate.LocalDateTime;
                DateTime endDate = DateTimeOffset.Now.LocalDateTime;
                if (startDate.Date != endDate.Date)
                    time = GetTimeFromSameDate(startDate, endDate);
                else
                    time = GetTimeFromOtherDate(startDate, endDate);
                records[records.IndexOf(temp)].RealWorkTime = time;
                return true;
            }
            else
                return false;
        }

        private TimeParser GetTimeFromSameDate(DateTime startDate, DateTime endDate)
        {
            TimeParser time = new TimeParser();
            TimeSpan tempTS = startDate.Date.AddHours(19).Subtract(startDate);
            time.Hours += tempTS.Hours;
            time.Minutes += tempTS.Minutes;
            tempTS = (endDate.Hour > 10) ? (endDate.Subtract(endDate.Date.AddHours(10))) : (endDate.Subtract(endDate.Date) + TimeSpan.FromHours(5));
            time.Hours += tempTS.Hours;
            time.Minutes += tempTS.Minutes;
            startDate = startDate.AddDays(1);
            while (startDate.Date < endDate.Date)
            {
                if (startDate.DayOfWeek != DayOfWeek.Saturday
                    && startDate.DayOfWeek != DayOfWeek.Sunday)
                    time.Days += 1;
                startDate = startDate.AddDays(1);
            }
            return time;
        }

        private TimeParser GetTimeFromOtherDate(DateTime startDate, DateTime endDate)
        {
            TimeParser time = new TimeParser();
            TimeSpan tempTS = endDate.Subtract(startDate);
            time.Hours += startDate.Hour < 14 ? tempTS.Hours - 1 : tempTS.Hours;
            time.Minutes += tempTS.Minutes;
            return time;
        }

        public bool Remove(Record record)
        {
            return records.Remove(record);
        }

        public Record Find(uint id)
        {
            return records.Find(x => x.Id == id);
        }

        public void Show()
        {
            Console.WriteLine("   Id".PadRight(10) +
                    "Start Date".PadRight(21) +
                    "Planned Time".PadRight(14) +
                    "Real Time".PadRight(11) +
                    "Inaccuracy");
            foreach (Record record in records)
            {
                Console.WriteLine(("   " + record.Id.ToString()).PadRight(10) + 
                    record.StartDate.LocalDateTime.ToString("dd.MM.yyyy HH:mm:ss").PadRight(21) +
                    record.PlannedWorkTime.ToString().PadRight(14) +
                    ((record.RealWorkTime.ToString() != "") ? record.RealWorkTime.ToString() : "unknown").PadRight(11) +
                    ((record.Inaccuracy != null) ? (Math.Round((double)record.Inaccuracy, 4)).ToString(CultureInfo.GetCultureInfo("en-US")) : "unknown") + " ");
            }
            var temp = records.Where(x => x.Inaccuracy != null).Select(x => x.Inaccuracy);
            Console.WriteLine("\nAvg:".PadLeft(49).PadRight(56) + (Math.Round((double)(temp.Sum() / temp.Count()), 4)).ToString(CultureInfo.GetCultureInfo("en-US")));
        }

        public void ToFile()
        {
            StringBuilder lines = new StringBuilder();
            lines.AppendLine("Id,Start Date,Planned Time,Real Time,Inaccuracy");
            foreach (Record record in records)
            {
                lines.AppendLine(record.ToCSV()); 
            }
            File.WriteAllText(Path, lines.ToString());
        }

        public bool FromFile()
        {
            if (File.Exists(Path))
            {
                string[] lines = File.ReadAllLines(Path);
                foreach (string line in lines)
                {
                    string[] temp = line.Split(",");
                    if (temp[0] != "Id")
                        records.Add(new Record(uint.Parse(temp[0]),
                            DateTimeOffset.FromUnixTimeSeconds(long.Parse(temp[1])),
                            TimeParser.Parse(temp[2]),
                            TimeParser.Parse(temp[3])));
                }
                return true;
            }
            else
                return false;
        }
    }
}
