namespace MH_SP_StaronTomas.Data;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }

    public City(int id, string name, string code)
    {
        Id = id;
        Name = name;
        Code = code;
    }

    public override string ToString()
    {
        return $"{Id}: {Name} ({Code})";
    }
}
