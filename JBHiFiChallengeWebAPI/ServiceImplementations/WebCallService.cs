using JBHiFiChallengeWebAPI.Helpers;
using JBHiFiChallengeWebAPI.Models;
using JBHiFiChallengeWebAPI.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.ServiceImplementations
{
    public class WebCallService : IWebCallService
    {
        public async Task<WebCallResult<List<OpenWeatherMapModel>>> GetOpenWeatherMapDataAsync(string cityName, string countryName)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string apiKey = AppDefs.AppSettings.OpenWeatherMapApiKey;
                // string url = $"api.openweathermap.org/data/2.5/weather?q=London,uk&appid={apiKey}";
                string url = $"";

                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    string apiResult = await response.Content.ReadAsStringAsync();

                    int statusCode = (int)response.StatusCode;
                    var resultList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OpenWeatherMapModel>>(apiResult);

                    var result = new WebCallResult<List<OpenWeatherMapModel>>();
                    result.StatusCode = statusCode;
                    result.Data = resultList;

                    return result;
                }
            }
        }
    }
}
