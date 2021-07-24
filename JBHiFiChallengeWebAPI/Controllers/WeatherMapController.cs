using JBHiFiChallengeWebAPI.Models;
using JBHiFiChallengeWebAPI.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherMapController : ControllerBase
    {
        IWeatherMapService weatherMapService;

        public WeatherMapController(IWeatherMapService _weatherMapService)
        {
            this.weatherMapService = _weatherMapService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string cityName, string countryName)
        {
            await Task.Delay(0);

            if (string.IsNullOrWhiteSpace(cityName))
                return BadRequest("City name was not provided");

            if (string.IsNullOrWhiteSpace(cityName))
                return BadRequest("Country Name was not provided");

            bool rateLimitOk = weatherMapService.CheckRateLimit("key1");
            if (!rateLimitOk)
                return StatusCode(422, "Too many requests");

            List<WeatherMapResult> data = new List<WeatherMapResult>();
            ApiActionResult apiResult = new ApiActionResult(data);
            return Ok(apiResult);
        }
    }
}
