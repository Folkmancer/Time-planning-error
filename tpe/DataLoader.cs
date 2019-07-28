using System;
using System.IO;
using System.Text;

namespace tpe
{
    static class DataLoader
    {
        public static void ToFile(string path, TaskManager data)
        {
            StringBuilder lines = new StringBuilder();
            lines.AppendLine("Id,Start Date,Planned Time,Real Time,Inaccuracy");
            foreach (Record record in data.Records)
            {
                lines.AppendLine(record.ToCSV());
            }
            File.WriteAllText(path, lines.ToString());
        }

        public static bool FromFile(string path, TaskManager data)
        {
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] temp = line.Split(",");
                    if (temp[0] != "Id")
                    {
                        data.OpenTask(new Record(uint.Parse(temp[0]),
                            DateTimeOffset.FromUnixTimeSeconds(long.Parse(temp[1])),
                            TimeParser.Parse(temp[2]),
                            TimeParser.Parse(temp[3])));
                    }        
                }
                return true;
            }
            else
                return false;
        }
    }
}
