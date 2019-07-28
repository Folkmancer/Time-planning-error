using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace tpe
{
    public class TaskManager
    {
        public List<Record> Records { get; }

        public TaskManager() => Records = new List<Record>(); 

        public void OpenTask(Record record)
        {
            Records.Add(record);
        }

        public bool CloseTask(uint id, TimeSpan time)
        {
            Record temp = FindTask(id);
            if (temp != null)
            {
                Records[Records.IndexOf(temp)].RealWorkTime = time;
                return true;
            }
            else return false;
        }

        public bool CloseTask(uint id)
        {
            Record temp = FindTask(id);
            if (temp != null)
            {
                TimeSpan time;
                DateTime startDate = temp.StartWorkDate.LocalDateTime;
                DateTime endDate = DateTimeOffset.Now.LocalDateTime;
                if (startDate.Date != endDate.Date)
                    time = GetTimeFromOtherDate(startDate, endDate);
                else
                    time = GetTimeFromSameDate(startDate, endDate);
                Records[Records.IndexOf(temp)].RealWorkTime = time;
                return true;
            }
            else
                return false;
        }

        private TimeSpan GetTimeFromOtherDate(DateTime startDate, DateTime endDate)
        {
            TimeSpan firstDateTime = startDate.Date.AddHours(19).Subtract(startDate);
            TimeSpan lastDateTime = (endDate.Hour > 10) ? (endDate.Subtract(endDate.Date.AddHours(10))) : (endDate.Subtract(endDate.Date) + TimeSpan.FromHours(5));
            int minutsPassed = (firstDateTime.Minutes + lastDateTime.Minutes) % 60;
            int hoursPassed = ((firstDateTime.Hours + lastDateTime.Hours) + ((firstDateTime.Minutes + lastDateTime.Minutes) / 60)) % 8;
            int daysPassed = ((firstDateTime.Hours + lastDateTime.Hours) + ((firstDateTime.Minutes + lastDateTime.Minutes) / 60)) / 8;
            startDate = startDate.AddDays(1);
            while (startDate.Date < endDate.Date)
            {
                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                    daysPassed += 1;
                startDate = startDate.AddDays(1);
            }
            return TimeSpan.FromDays(daysPassed) + TimeSpan.FromHours(hoursPassed) + TimeSpan.FromMinutes(minutsPassed);
        }

        private TimeSpan GetTimeFromSameDate(DateTime startDate, DateTime endDate)
        {
            TimeSpan temp = endDate.Subtract(startDate);
            int minutsPassed = temp.Minutes;
            int hoursPassed = startDate.Hour < 14 ? temp.Hours - 1 : temp.Hours;
            return TimeSpan.FromHours(hoursPassed) + TimeSpan.FromMinutes(minutsPassed);
        }

        public bool RemoveTask(uint id)
        {
            return Records.Remove(FindTask(id));
        }

        public Record FindTask(uint id)
        {
            return Records.Find(x => x.Id == id);
        }

        public bool ExistsTask(uint id)
        {
            return Records.Exists(x => x.Id == id);
        }

        public bool IsEmpty() => Records.Count != 0;

        public void ShowTasks()
        {
            Console.WriteLine("Id".PadLeft(5).PadRight(10) +
                    "Start Date".PadRight(21) +
                    "Planned Time".PadRight(14) +
                    "Real Time".PadRight(11) +
                    "Inaccuracy");
            foreach (Record record in Records)
            {
                Console.WriteLine((record.Id.ToString()).PadLeft(7).PadRight(10) +
                    record.StartWorkDate.LocalDateTime.ToString("dd.MM.yyyy HH:mm:ss").PadRight(21) +
                    TimeParser.ToString(record.PlannedWorkTime).PadRight(14) +
                    TimeParser.ToString(record.RealWorkTime).PadRight(11) +
                    ((record.Inaccuracy != null) ? (Math.Round((double)record.Inaccuracy, 4)).ToString(CultureInfo.GetCultureInfo("en-US")) : "unknown") + " ");
            }
            var temp = Records.Where(x => x.Inaccuracy != null).Select(x => x.Inaccuracy);
            Console.WriteLine("\nAvg:".PadLeft(49).PadRight(56) + (Math.Round((double)(temp.Sum() / temp.Count()), 4)).ToString(CultureInfo.GetCultureInfo("en-US")));
        }
    }
}
