using System;

namespace Thermostat.DTO
{
    public class ThermostatObject
    {
        public double DesiredTemperature { get; set; }
        public double CurrentTemperature { get; set; }
        public bool heating { get; set; }
    }
}
