namespace uServiceDemo.Infrastructure;

internal class Constants
{
    public static class Database
    {
        public const string DabaseConnectionString = "WeatherforecastDB";

        public static class Tables
        {
            public const string WeatherForecastTable = "WeatherForecast";
            public const string WindTable = "Wind";
        }
    }
}