using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using space_weather_api.Entities;

namespace space_planet_api.Controllers;

[ApiController]
[Route("/api/v1/planets")]
public class PlanetController : ControllerBase
{
    
    private static readonly List<Planet> Planets = new()
    {
        new Planet()
        {
           Id = "1", Name = "earth"
        }
    };
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = Planets.OrderBy(x=>x.Id).ToList();

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var planet = Planets.Find(w => w.Id == id);
        if (planet == null)
        {
            return NotFound();
        }

        return Ok(planet);
    }
    
    [HttpPost]
    public IActionResult Add([FromBody] Planet planet)
    {
        Planets.Add(planet);
        return CreatedAtAction(nameof(GetById), new { id = planet.Id }, planet);
    }
    
    [HttpPut]
    public IActionResult UpdateById(string id, [FromBody] Planet planet)
    {
        var planetById = Planets.Find(w => w.Id == id);

        if (planetById == null)
        {
            return NotFound(id);
        }
        
        Planet(planetById, planet);
        return Ok();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(string id, [FromBody] JsonPatchDocument<Planet> patchDocument)
    {
        var planet = Planets.SingleOrDefault(p => p.Id == id);

        if (planet == null)
        {
            return NotFound();
        }

        patchDocument.ApplyTo(planet, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok();
    }

    [HttpDelete]
    public IActionResult DeleteById(string id)
    {
        var planet = Planets.Find(w => w.Id == id);

        if (planet == null)
        {
            return NotFound();
        }

        Planets.Remove(planet);
        return NoContent();
    }

    private void Planet(SpaceObject planetById, SpaceObject planet)
    {
        planetById.Name = planet.Name;
        planetById.Id = planet.Id;
    }
}