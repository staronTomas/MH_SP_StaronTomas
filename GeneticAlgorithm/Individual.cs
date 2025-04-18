namespace MH_SP_StaronTomas.GeneticAlgorithm;

public class Individual
{
    private static Random random = new Random();
    
    public List<int> Chromosome { get; private set; }
    public int Fitness { get; private set; }
    private int[,] DistanceMatrix { get; set; }
    
    // Create a random individual
    public Individual(int chromosomeLength, int[,] distanceMatrix)
    {
        DistanceMatrix = distanceMatrix;
        Chromosome = Enumerable.Range(0, chromosomeLength).ToList();
        
        // Fisher-Yates shuffle
        for (int i = Chromosome.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            int temp = Chromosome[i];
            Chromosome[i] = Chromosome[j];
            Chromosome[j] = temp;
        }
        
        CalculateFitness();
    }
    
    // Create an individual with a predefined chromosome
    public Individual(List<int> chromosome, int[,] distanceMatrix)
    {
        DistanceMatrix = distanceMatrix;
        Chromosome = new List<int>(chromosome);
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
        
        for (int i = 0; i < Chromosome.Count - 1; i++)
        {
            totalDistance += DistanceMatrix[Chromosome[i], Chromosome[i + 1]];
        }
        
        // Add the distance from the last city back to the first
        totalDistance += DistanceMatrix[Chromosome[Chromosome.Count - 1], Chromosome[0]];
        
        Fitness = totalDistance;
    }
    
    // Override ToString to display the chromosome and fitness
    public override string ToString()
    {
        return $"Route: {string.Join(" -> ", Chromosome)} | Distance: {Fitness}";
    }
}
