using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Thermostat.DTO;

namespace Thermostat.Handlers
{
    public static class LogHandler
    {
        public static async void LogCurrentTemprature(ThermostatObject thermostatObject)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "http://192.168.1.1:8086/write?db=db"))
                {
                    request.Content = new StringContent("temperature id=" + (await GetCurrentID()+1).ToString() + ",current_temperature=" + thermostatObject.CurrentTemperature + ",desired_temperature=" + thermostatObject.DesiredTemperature + ",heating=" + thermostatObject.heating);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    await httpClient.SendAsync(request);
                }
            }
        }
        public static async Task<int> GetCurrentID()
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "http://192.168.1.1:8086/query?q=SELECT max(id) from db..temperature"))
                {
                    var result = await httpClient.SendAsync(request);

                    MaxIDFromDBObject maxIDFromDBObject = JsonSerializer.Deserialize<MaxIDFromDBObject>(await result.Content.ReadAsStringAsync());
                    int currentID = Convert.ToInt32(maxIDFromDBObject.results.FirstOrDefault().series.FirstOrDefault().values[0][1].ToString().FirstOrDefault(c => char.IsDigit(c)));
                    return currentID;
                }
            }
        }

    }
}
