using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Thermostat.DTO;
using Thermostat.Handlers;
using System;

namespace Thermostat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        [HttpPost("New")]
        public void AddNewSchedulePoint([FromBody]ScheduleObject sro)
        {
            ScheduleHandler.AddNewSchedulePoint(sro);
        }

        [HttpGet("Upcoming")]
        public ScheduleObject GetUpComingScheduledObject()
        {
            return ScheduleHandler.GetUpComingScheduledObject();
        }

        [HttpGet("All")]
        public IEnumerable<ScheduleObject> GetAllSchedulePoint()
        {
            return ScheduleHandler.GetScheduleObjects();
        }

        [HttpDelete("Delete")]
        public void RemoveSchedulePoint(Guid guid)
        {
            ScheduleHandler.DeleteScheduleObject(guid);
        }
    }
}
