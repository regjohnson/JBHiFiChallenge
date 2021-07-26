using JBHiFiChallengeWebAPI.Entities;
using JBHiFiChallengeWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.ServiceContracts
{
    public interface IWeatherMapService
    {
        ApiErrorResult ValidateInputs(string cityName, string countryName, string keyName);
        Task<string> GetWeatherMapDataAsync(string cityName, string countryName);
    }
}
