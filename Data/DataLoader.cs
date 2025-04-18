namespace MH_SP_StaronTomas.Data;

public class DataLoader
{
    public static List<City> LoadCities(string nameFile, string codeFile)
    {
        var cities = new List<City>();
        
        string[] names = File.ReadAllLines(nameFile);
        string[] codes = File.ReadAllLines(codeFile);
        
        int cityCount = int.Parse(names[0]);
        
        for (int i = 1; i < cityCount; i++)
        {
            cities.Add(new City(i, names[i].Trim(), codes[i].Trim()));
        }
        
        return cities;
    }
    
    public static int[,] LoadDistanceMatrix(string distanceFile, int cityCount)
    {
        string[] lines = File.ReadAllLines(distanceFile);
        int[,] distances = new int[cityCount, cityCount];
        
        int lineIndex = 2; // Skip the first and second line (city count) 460 x 460
        
        for (int i = 0; i < cityCount; i++)
        {
            for (int j = 0; j < cityCount; j++)
            {
                distances[i, j] = int.Parse(lines[lineIndex++].Trim());
            }
        }
        
        return distances;
    }
}
