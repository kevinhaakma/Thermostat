using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Thermostat.DTO;

namespace Thermostat.Handlers
{
    public class JSONHandler
    {
        private string path = ("schedule.json");
        public JSONHandler()
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

        public void WriteSchedule(List<ScheduleObject> scheduleObjects)
        {
            File.WriteAllText(path, string.Empty);
            using(StreamWriter sw = new StreamWriter(path))
            {
                foreach (var so in scheduleObjects)
                {
                    sw.WriteLine(JsonSerializer.Serialize(so));
                }
                sw.Flush();
            }
        }

        public List<ScheduleObject> ReadSchedule()
        {
            List<ScheduleObject> scheduleObjects = new List<ScheduleObject>();
            string line = string.Empty;
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    scheduleObjects.Add(JsonSerializer.Deserialize<ScheduleObject>(line));
                }
            }
            return scheduleObjects;
        }
    }
}
