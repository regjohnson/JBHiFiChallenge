using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.Models
{
    public class OpenWeatherMapModel
    {
        public List<Weather> Weather { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
    }

    public class OpenWeatherMapErrorModel
    {
        public string Message { get; set; }
    }
}
