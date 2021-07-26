using JBHiFiChallengeWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.ServiceContracts
{
    public interface IWebCallService
    {
        public Task<WebCallResult<List<OpenWeatherMapModel>>> GetOpenWeatherMapDataAsync(string cityName, string countryName);
    }
}
