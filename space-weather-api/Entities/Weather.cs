
namespace space_weather_api.Entities;

public class Weather
{
    public string Id { get; set; }
    public string PlanetId { get; set; }
    public string SatelliteId { get; set; }
    public string TemperatureUnit { get; set; }

    public double TemperatureValue { get; set; }
    public string Description { get; set; }
    public DateTime LastUpdateTime { get; set; }

    public Weather()
    {
    }

    public Weather(string id, string planetId, string satelliteId, string temperatureUnit, double temperatureValue,
        DateTime lastUpdateTime
        , string description)
    {
        Id = id;
        PlanetId = planetId;
        SatelliteId = satelliteId;
        TemperatureUnit = temperatureUnit;
        LastUpdateTime = lastUpdateTime;
        Description = description;
        TemperatureValue = temperatureValue;
    }
}