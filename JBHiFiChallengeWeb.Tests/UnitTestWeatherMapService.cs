using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using JBHiFiChallengeWebAPI.Entities;
using JBHiFiChallengeWebAPI.Models;
using JBHiFiChallengeWebAPI.ServiceContracts;
using JBHiFiChallengeWebAPI.ServiceImplementations;
using Moq;
using Xunit;

namespace JBHiFiChallengeWeb.Tests
{
    public class UnitTestWeatherMapService
    {
        IWeatherMapService weatherMapService;

        public UnitTestWeatherMapService()
        {
            var mockWebCallService = new Mock<IWebCallService>();

            mockWebCallService
                .Setup(s => s.GetOpenWeatherMapDataAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string cityName, string countryName) => MockResult(cityName, countryName));

            IWebCallService webCallService = mockWebCallService.Object;
            weatherMapService = new WeatherMapService(webCallService);
        }

        private Task<WebCallResult<OpenWeatherMapModel>> MockResult(string cityName, string countryName)
        {
            WebCallResult<OpenWeatherMapModel> mockResult = new WebCallResult<OpenWeatherMapModel>();
            mockResult.Data = new OpenWeatherMapModel();

            mockResult.Data.Weather = new List<Weather>();
            mockResult.Data.Weather.Add(new Weather() { Description = "Cloudy" });
            
            return Task.FromResult(mockResult);
        }

        [Fact]
        public async Task TestWeatherRetrieval()
        {
            var weatherResult = await weatherMapService.GetWeatherMapDataAsync("Melbourne", "AUS");
            Assert.Equal("Cloudy", weatherResult);
        }

        [Fact]
        public void TestInputCityNameNotEmpty()
        {
            ApiErrorResult errorResult = weatherMapService.ValidateInputs(string.Empty, "AUS", "Key1");
            Assert.Equal("City name was not provided", errorResult.ErrorMessage);
        }

        [Fact]
        public void TestInputCountryNameNotEmpty()
        {
            ApiErrorResult errorResult = weatherMapService.ValidateInputs("Melbourne", null, "Key1");
            Assert.Equal("Country name was not provided", errorResult.ErrorMessage);
        }

        [Fact]
        public void TestInputKeyNotEmpty()
        {
            ApiErrorResult errorResult = weatherMapService.ValidateInputs("Melbourne", "AUS", null);
            Assert.Equal("Key name was not sent in header", errorResult.ErrorMessage);
        }

        [Fact]
        public void TestInvalidKeyName()
        {
            ApiErrorResult errorResult = weatherMapService.ValidateInputs("Melbourne", "AUS", "Key123");
            Assert.Equal("Invalid Key name was sent", errorResult.ErrorMessage);
        }

        [Fact]
        public void TestValidKey()
        {
            ApiErrorResult errorResult = weatherMapService.ValidateInputs("Melbourne", "AUS", "Key1");
            Assert.Null(errorResult);
        }
    }
}
