using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Thermostat.Controllers;
using Thermostat.DTO;
using Thermostat.Handlers;

namespace Thermostat
{
    public class Program
    {
        private static Thread tempratureThread;
        private static bool keepRunning = true;
        private static bool heating = false;
        private static Queue<double> temperatureList = new Queue<double>();

        public static void Main(string[] args)
        {
            Console.WriteLine("Temprature Controller Starting");
            ScheduleHandler.UpdateSchedulePointsFromJSON();
            tempratureThread = new Thread(ControlTemperature);
            tempratureThread.Start();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://localhost:5000", "http://192.168.1.1:5000", "https://192.168.1.1:5001");
                    webBuilder.UseStartup<Startup>();
                });

        public static void ControlTemperature()
        {
            Console.WriteLine("Initializing values");
            temperatureList.Enqueue(20);
            heating = false;
            ThermostatController.desiredTemperature = -1;
            Console.WriteLine("Temprature Controller Started");
            while (keepRunning)
            {
                double currenttemperature = SerialHandler.GetLastValue();
                if (currenttemperature != -1)
                {
                    temperatureList.Enqueue(currenttemperature);
                }
                if(temperatureList.Count > 5)
                {
                    temperatureList.Dequeue();
                }

                ScheduleObject upcomingSchedulePoint = ScheduleHandler.GetUpComingScheduledObject();
                if (upcomingSchedulePoint.Hour == DateTime.Now.Hour && upcomingSchedulePoint.Minute == DateTime.Now.Minute)
                {
                    ThermostatController.desiredTemperature = upcomingSchedulePoint.Temperature;
                }

                double currentAverage = Math.Round(temperatureList.Average(), 1);

                if (currentAverage < ThermostatController.desiredTemperature - 0.2 && !heating)
                {
                    heating = true;
                    SerialHandler.TurnOn();
                }
                else if (currentAverage > ThermostatController.desiredTemperature + 0.2 && heating)
                {
                    heating = false;
                    SerialHandler.TurnOff();
                }

                SerialHandler.ResetTimer();
                Thread.Sleep(500);
            }
        }

        public static double getTemperature()
        {
            return temperatureList.Average();
        }

        public static bool getHeatingStatus()
        {
            return heating;
        }
    }
}
