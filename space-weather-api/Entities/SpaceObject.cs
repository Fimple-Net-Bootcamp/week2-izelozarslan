namespace space_weather_api.Entities;

public class SpaceObject
{
    public string Id { get; set; }
    public string Name { get; set; }
    
    protected SpaceObject(){}

    public SpaceObject(string id, string name)
    {
        Id = id;
        Name = name;
    }
}