using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thermostat.DTO
{
    public class MaxIDFromDBObject
    {
        public List<Result> results { get; set; }
    }
    public class Series
    {
        public string name { get; set; }
        public List<string> columns { get; set; }
        public List<List<object>> values { get; set; }
    }

    public class Result
    {
        public int statement_id { get; set; }
        public List<Series> series { get; set; }
    }
}
