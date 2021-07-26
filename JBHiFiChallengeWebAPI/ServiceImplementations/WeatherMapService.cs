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

            if (!Constants.KeyNames.Keys.Contains(keyName))
                return new ApiErrorResult(HttpStatusCode.BadRequest, "Invalid Key name was sent");

            return null;
        }

        public async Task<string> GetWeatherMapDataAsync(string cityName, string countryName)
        {
            try
            {
                var apiResult = await this.webCallService.GetOpenWeatherMapDataAsync(cityName, countryName);
                var weatherItem = apiResult.Data?.Weather.FirstOrDefault();

                string weatherDescription = weatherItem?.Description ?? "No weather description was avaiable";
                return weatherDescription;
            }
            catch (Exception ex)
            {
                string errMsg = $"Error retrieving weather ({ex.Message})";
                return errMsg;
            }
        }
    }
}
