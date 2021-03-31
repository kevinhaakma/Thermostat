using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thermostat.DTO;

namespace Thermostat.Handlers
{
    public class ScheduleHandler
    {
        private static List<ScheduleObject> scheduleObjects = new List<ScheduleObject>();

        public static ScheduleObject GetUpComingScheduledObject()
        {
            ScheduleObject firstScheduleObject = null;
            if (scheduleObjects.Count > 0)
            {
                foreach (ScheduleObject so in scheduleObjects)
                {
                    if(firstScheduleObject == null)
                    {
                        firstScheduleObject = so;
                        continue;
                    }

                    if(so.Hour == DateTime.Now.Hour || so.Hour - DateTime.Now.Hour > 0 && (DateTime.Now.Hour - so.Hour) > (DateTime.Now.Hour - firstScheduleObject.Hour))
                    {
                        if(so.Minute == DateTime.Now.Minute || so.Minute - DateTime.Now.Minute > 0 && (DateTime.Now.Minute - so.Minute) > (DateTime.Now.Minute - firstScheduleObject.Minute))
                        {
                            firstScheduleObject = so;
                            continue;
                        }
                    }
                }
            }
            else
            {
                return new ScheduleObject() { Hour = -1, Minute = -1, Temperature = -1 };
            }
            return firstScheduleObject;
        }

        public static void AddNewSchedulePoint(ScheduleObject so)
        {
            scheduleObjects.Add(so);
            new JSONHandler().WriteSchedule(scheduleObjects);
        }
        public static void UpdateSchedulePointsFromJSON()
        {
            scheduleObjects = new JSONHandler().ReadSchedule();
        }

        public static List<ScheduleObject> GetScheduleObjects()
        {
            return scheduleObjects;
        }

        public static void DeleteScheduleObject(Guid guid)
        {
            try
            {
                scheduleObjects.RemoveAll(so => so.Guid == guid);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
