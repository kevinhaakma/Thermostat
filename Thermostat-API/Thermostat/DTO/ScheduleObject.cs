using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thermostat.DTO
{
    public class ScheduleObject
    {
        public Guid Guid { get; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public double Temperature { get;  set; }

        public ScheduleObject()
        {
            this.Guid = Guid.NewGuid();
        }
    }

    public class ScheduleObjectList
    {
        public IEnumerable<ScheduleObject> schedule { get; set; }
    }
}
