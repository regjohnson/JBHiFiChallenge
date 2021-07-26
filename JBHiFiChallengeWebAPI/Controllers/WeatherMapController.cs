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
        IRateLimitCheckService rateLimitCheckService;

        public WeatherMapController(IWeatherMapService _weatherMapService, IRateLimitCheckService _rateLimitCheckService)
        {
            this.weatherMapService = _weatherMapService;
            this.rateLimitCheckService = _rateLimitCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string cityName, string countryName)
        {
            string keyName = Request.Headers["keyName"];
            ApiErrorResult invalidInput = weatherMapService.ValidateInputs(cityName, countryName, keyName);
            if (invalidInput != null)
            {
                return StatusCode(invalidInput.ErrorStatusCode, invalidInput.ErrorMessage);
            }

            bool rateLimitOk = rateLimitCheckService.CheckRateLimit(keyName);
            if (!rateLimitOk)
            {
                return StatusCode(422, "Too many requests");
            }

            await weatherMapService.GetMapDataAsync(cityName, countryName);

            List<WeatherMapResult> data = new List<WeatherMapResult>();
            ApiActionResult apiResult = new ApiActionResult(data);
            return Ok(apiResult);
        }
    }
}
