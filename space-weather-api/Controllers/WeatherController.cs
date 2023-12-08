using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using space_weather_api.Entities;

namespace space_weather_api.Controllers;

[ApiController]
[Route("/api/v1/weather-forecasts")]
public class WeatherController : ControllerBase
{
    private static readonly List<Weather> Weathers = new()
    {
        new Weather
        {
            Id = "1", PlanetId = "1", SatelliteId = "1", TemperatureUnit = "Celsius", TemperatureValue = 150.6,
            Description = "cloudy", LastUpdateTime = new DateTime(2023, 12, 31)
        }
    };

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var weather = Weathers.Find(w => w.Id == id);
        if (weather == null)
        {
            return NotFound();
        }

        return Ok(weather);
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] FilteringParameters filteringParameters,
        [FromQuery] PagingParameters pagingParameters, [FromQuery] SortingParameters sortingParameters)
    {
        var filteredData = Weathers.AsQueryable();

        if (filteringParameters != null)
        {
            if (filteringParameters.Date.HasValue)
            {
                DateTime date = filteringParameters.Date.Value.Date;

                filteredData = filteredData.Where(x => x.LastUpdateTime == date);
            }
        }

        if (sortingParameters != null)
        {
            if (!string.IsNullOrEmpty(sortingParameters.OrderBy))
            {
                filteredData = sortingParameters.OrderBy.ToLower() switch
                {
                    "date" => sortingParameters.SortOrder == "asc"
                        ? filteredData.OrderBy(x => x.LastUpdateTime)
                        : filteredData.OrderByDescending(x => x.LastUpdateTime),
                    _ => filteredData,
                };
            }
        }

        if (pagingParameters != null)
        {
            var page = pagingParameters.PageNumber != null ? pagingParameters.PageNumber : 1;
            var size = pagingParameters.PageSize != null ? pagingParameters.PageSize : 10;


            filteredData = filteredData.Skip((page - 1) * size).Take(size);
        }

        var result = filteredData.ToList();

        return Ok(result);
    }

    [HttpGet("/planets/{planetId}")]
    public IActionResult GetByPlanetId(string planetId)
    {
        var weather = Weathers.Find(w => w.PlanetId == planetId);
        if (weather == null)
        {
            return NotFound();
        }

        return Ok(weather);
    }

    [HttpGet("/satellites/{satelliteId}")]
    public IActionResult GetBySatelliteId(string satelliteId)
    {
        var weather = Weathers.Find(w => w.PlanetId == satelliteId);
        if (weather == null)
        {
            return NotFound();
        }

        return Ok(weather);
    }

    [HttpPost]
    public IActionResult Add([FromBody] Weather weather)
    {
        Weathers.Add(weather);
        return CreatedAtAction(nameof(GetById), new { id = weather.Id }, weather);
    }

    [HttpPut]
    public IActionResult UpdateById(string id, [FromBody] Weather weather)
    {
        var weatherById = Weathers.Find(w => w.Id == id);

        if (weatherById == null)
        {
            return NotFound(id);
        }
        
        SetWeather(weatherById, weather);
        return Ok();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(string id, [FromBody] JsonPatchDocument<Weather> patchDocument)
    {
        var weather = Weathers.SingleOrDefault(p => p.Id == id);

        if (weather == null)
        {
            return NotFound();
        }

        patchDocument.ApplyTo(weather, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok();
    }

    [HttpDelete]
    public IActionResult DeleteById(string id)
    {
        var weather = Weathers.Find(w => w.Id == id);

        if (weather == null)
        {
            return NotFound();
        }

        Weathers.Remove(weather);
        return NoContent();
    }

    private void SetWeather(Weather weatherById, Weather weather)
    {
        weatherById.PlanetId = weather.PlanetId;
        weatherById.SatelliteId = weather.SatelliteId;
        weatherById.Description = weather.Description;
        weatherById.TemperatureValue = weather.TemperatureValue;
        weatherById.TemperatureUnit = weather.TemperatureUnit;
        weatherById.LastUpdateTime = weather.LastUpdateTime;
        weatherById.Id = weather.Id;
    }
}