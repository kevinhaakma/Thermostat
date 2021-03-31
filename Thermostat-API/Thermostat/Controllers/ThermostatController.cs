using Microsoft.AspNetCore.Mvc;
using System;
using System.IO.Ports;
using Thermostat.Handlers;
using Thermostat.DTO;

namespace Thermostat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThermostatController : ControllerBase
    {
        public static double desiredTemperature { get; set; }

        [HttpPut("TurnOnHeating")]
        public void TurnOnHeating()
        {
            SerialHandler.TurnOn();
        }
        [HttpPut("TurnOffHeating")]
        public void TurnOffHeating()
        {
            SerialHandler.TurnOff();
        }
        [HttpGet("Temperature")]
        public double GetTemperature()
        {
            return Program.getTemperature();
        }

        [HttpPut("Temperature")]
        public double SetTemprature(double temperature)
        {
            desiredTemperature = temperature;
            return desiredTemperature;
        }

        [HttpGet("Status")]
        public ThermostatObject GetStatus()
        {
            return new ThermostatObject() { CurrentTemperature = GetTemperature(), DesiredTemperature = desiredTemperature, heating = Program.getHeatingStatus()};
        }
    }
}
