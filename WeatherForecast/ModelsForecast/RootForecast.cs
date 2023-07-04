using System.Collections.Generic;

namespace WeatherForecast.ModelsForecast
{
    public class RootForecast
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public List<ForecastList> list { get; set; }
        public City city { get; set; }
    }
}
