using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using space_weather_api.Entities;

namespace space_weather_api.Controllers;

[ApiController]
[Route("/api/v1/satellites")]
public class SatelliteController : ControllerBase
{
    private static readonly List<Satellite> Satellites = new()
    {
        new Satellite()
        {
            Id = "1", Name = "moon"
        }
    };
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = Satellites.OrderBy(x=>x.Id).ToList();

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var satellite = Satellites.Find(w => w.Id == id);
        if (satellite == null)
        {
            return NotFound();
        }

        return Ok(satellite);
    }
    
    [HttpPost]
    public IActionResult Add([FromBody] Satellite satellite)
    {
        Satellites.Add(satellite);
        return CreatedAtAction(nameof(GetById), new { id = satellite.Id }, satellite);
    }
    
    [HttpPut]
    public IActionResult UpdateById(string id, [FromBody] Satellite satellite)
    {
        var satelliteById = Satellites.Find(w => w.Id == id);

        if (satelliteById == null)
        {
            return NotFound(id);
        }
        
        Satellite(satelliteById, satellite);
        return Ok();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(string id, [FromBody] JsonPatchDocument<Satellite> patchDocument)
    {
        var satellite = Satellites.SingleOrDefault(p => p.Id == id);

        if (satellite == null)
        {
            return NotFound();
        }

        patchDocument.ApplyTo(satellite, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok();
    }

    [HttpDelete]
    public IActionResult DeleteById(string id)
    {
        var satellite = Satellites.Find(w => w.Id == id);

        if (satellite == null)
        {
            return NotFound();
        }

        Satellites.Remove(satellite);
        return NoContent();
    }

    private void Satellite(SpaceObject satelliteById, SpaceObject satellite)
    {
        satelliteById.Name = satellite.Name;
        satelliteById.Id = satellite.Id;
    }
}