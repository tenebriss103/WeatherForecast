using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

using WeatherForecast.Models;
using WeatherForecast.ModelsCurrentWeather;
using WeatherForecast.ModelsForecast;

namespace WeatherForecast.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       

        [HttpGet(Name = "GetWeatherForecast")]

        public ActionResult<IEnumerable<ResultForecastWeather>> GetWeatherForecast(string city)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid=263839bfe3bdf82555867b1da6c9d6bb");
            
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

            }
            catch (WebException e)
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
            var indicators = serializer.Deserialize<RootForecast>(reader);

            var _clouds = indicators.list.Select(i => i.clouds.all).ToArray();
            var _temp = indicators.list.Select(i => i.main.temp).ToArray();
            var _wind = indicators.list.Select(i => i.wind.speed).ToArray();
            var _data = indicators.list.Select(i => i.dt_txt).ToArray();
            
            var result = Enumerable.Range(1, indicators.list.ToArray().Length - 1).Select(index => new ResultForecastWeather()
            {
                clouds = _clouds[index],
                wind = _wind[index],
                data = _data[index],
                temperature = Math.Round((_temp[index] - 273.15), 0)


            }).ToArray();

            return Ok(result);
        }
       
    }





}

       

