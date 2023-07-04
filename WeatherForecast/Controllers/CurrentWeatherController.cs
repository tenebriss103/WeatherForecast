using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Xml.Linq;
using WeatherForecast.Models;
using WeatherForecast.ModelsCurrentWeather;

namespace WeatherForecast.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentWeatherController : ControllerBase
    {
        
        [HttpGet(Name = "GetCurrentWeather")]
        public ActionResult<ResultCurrentWeather> GetCurrentWeather(string city)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid=263839bfe3bdf82555867b1da6c9d6bb");
            
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

            }
            catch(WebException e)
{
                response = (HttpWebResponse)e.Response;
                switch (response.StatusCode)
                {

                    case HttpStatusCode.Unauthorized:
                        return StatusCode(401);
                    case HttpStatusCode.PaymentRequired:
                        return StatusCode(402);
                    case HttpStatusCode.Forbidden:
                        return StatusCode(403);                         
                    case HttpStatusCode.NotFound:
                        return StatusCode(404);                        
                    case HttpStatusCode.Moved:    
                        return StatusCode(301);
                    default:                    
                        break;
                }

            }
           
        
            JsonTextReader reader = new JsonTextReader(new StreamReader(response.GetResponseStream()));
            reader.SupportMultipleContent = true;
            JsonSerializer serializer = new JsonSerializer();
            var indicators = serializer.Deserialize<Root>(reader);
            var _clouds = indicators.clouds.all;
            var _wind = indicators.wind.speed;
            var _data = indicators.dt;
            var _temp = indicators.main.temp;

            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(_data).ToLocalTime();


            var result = new ResultCurrentWeather()
            {
                clouds = _clouds,
                wind = _wind,
                data = dateTime,
                temperature = Math.Round((_temp - 273.15), 0)


            };

            return Ok(result);
        }

        
    }
}
