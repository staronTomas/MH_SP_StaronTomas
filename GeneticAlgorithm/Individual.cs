namespace MH_SP_StaronTomas.GeneticAlgorithm;

public class Individual
{
    private static Random random = new Random();
    
    public List<int> Chromosome { get; private set; }
    public int Fitness { get; private set; }
    private int[,] DistanceMatrix { get; set; }

    /// <summary>
    /// Constructor for creating a random individual.
    /// </summary>
    public Individual(int chromosomeLength, int[,] distanceMatrix)
    {
        DistanceMatrix = distanceMatrix;
        Chromosome = new List<int>();

        List<int> listOfNodes = new List<int>();

        // Create random Chromosome
        for (int i = 1; i < 460; i++)
        {
            listOfNodes.Add(i);
        }

        while(listOfNodes.Count > 0)
        {
            int randomIndex = random.Next(0, listOfNodes.Count);
            Chromosome.Add(listOfNodes[randomIndex]);
            listOfNodes.RemoveAt(randomIndex);
        }

        // Add city 0 at the beginning
        Chromosome.Insert(0, 0);
        Chromosome.Add(0); // Add city 0 at the end to complete the route

        CalculateFitness();
    }
    
    /// <summary>
    /// Create an individual with a predefined chromosome
    /// </summary>
    public Individual(List<int> chromosome, int[,] distanceMatrix)
    {
        DistanceMatrix = distanceMatrix;
        // Ensure the chromosome starts with city 0
        if (chromosome[0] != 0)
        {
            throw new ArgumentException("Chromosome must start with city 0");
        }
        Chromosome = new List<int>(chromosome);
        CalculateFitness();
    }

    /// <summary>
    /// Add this constructor to your Individual class
    /// </summary>
    public Individual(int[] chromosomeArray, int[,] distanceMatrix)
    {
        DistanceMatrix = distanceMatrix;
        if (chromosomeArray[0] != 0)
        {
            throw new ArgumentException("Chromosome must start with city 0");
        }
        Chromosome = new List<int>(chromosomeArray);
        CalculateFitness();
    }


    // Clone an individual
    public Individual Clone()
    {
        return new Individual(new List<int>(Chromosome), DistanceMatrix);
    }
    
    // Calculate the total distance of the route (fitness)
    public void CalculateFitness()
    {
        int totalDistance = 0;
        
        // Calculate distance through all cities
        for (int i = 0; i < Chromosome.Count - 1; i++)
        {
            totalDistance += DistanceMatrix[Chromosome[i], Chromosome[i + 1]];
        }
        
        Fitness = totalDistance;
    }
    
    // Override ToString to display the chromosome and fitness
    public override string ToString()
    {
        return $"Route: {string.Join(" -> ", Chromosome)} -> 0 | Distance: {Fitness}";
    }
}
