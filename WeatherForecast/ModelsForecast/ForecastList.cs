using WeatherForecast.Models;

namespace WeatherForecast.ModelsForecast
{
    public class ForecastList
    {
        public int dt { get; set; }
        public Main main { get; set; }
        public List<WeatherForecast> weather { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        public int visibility { get; set; }
        public double pop { get; set; }
        public Sys sys { get; set; }
        public string dt_txt { get; set; }
    }
}
