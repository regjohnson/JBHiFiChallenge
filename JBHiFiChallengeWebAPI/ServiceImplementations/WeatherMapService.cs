using JBHiFiChallengeWebAPI.Entities;
using JBHiFiChallengeWebAPI.Models;
using JBHiFiChallengeWebAPI.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.ServiceImplementations
{
    public class WeatherMapService : IWeatherMapService
    {
        IWebCallService webCallService;
        public WeatherMapService(IWebCallService _webCallService)
        {            
            this.webCallService = _webCallService;
        }

        public ApiErrorResult ValidateInputs(string cityName, string countryName, string keyName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return new ApiErrorResult(HttpStatusCode.BadRequest, "City name was not provided");

            if (string.IsNullOrWhiteSpace(countryName))
                return new ApiErrorResult(HttpStatusCode.BadRequest, "Country name was not provided");

            if (string.IsNullOrWhiteSpace(keyName))
                return new ApiErrorResult(HttpStatusCode.BadRequest, "Key name was not sent in header");

            return null;
        }

        public async Task GetMapDataAsync(string cityName, string countryName)
        {
            var result = await this.webCallService.GetOpenWeatherMapDataAsync(cityName, countryName);
        }
    }
}
