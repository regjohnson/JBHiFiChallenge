using JBHiFiChallengeWebAPI.Helpers;
using JBHiFiChallengeWebAPI.Models;
using JBHiFiChallengeWebAPI.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace JBHiFiChallengeWebAPI.ServiceImplementations
{
    public class WebCallService : IWebCallService
    {
        public async Task<WebCallResult<OpenWeatherMapModel>> GetOpenWeatherMapDataAsync(string cityName, string countryName)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string encodedCityName = HttpUtility.UrlEncode(cityName);
                string encodedCountryName = HttpUtility.UrlEncode(countryName);

                string apiKey = AppDefs.AppSettings.OpenWeatherMapApiKey;
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={encodedCityName},{encodedCountryName}&appid={apiKey}";

                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    string apiContent = await response.Content.ReadAsStringAsync();
                    int statusCode = (int)response.StatusCode;

                    WebCallResult<OpenWeatherMapModel> result = new WebCallResult<OpenWeatherMapModel>();
                    result.StatusCode = statusCode;

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResult = Newtonsoft.Json.JsonConvert.DeserializeObject<OpenWeatherMapModel>(apiContent);
                        result.Data = apiResult;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        var apiResult = Newtonsoft.Json.JsonConvert.DeserializeObject<OpenWeatherMapErrorModel>(apiContent);
                        result.ErrorMessage = apiResult.Message;
                    }

                    return result;
                }
            }
        }
    }
}
