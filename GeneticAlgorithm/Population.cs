namespace MH_SP_StaronTomas.GeneticAlgorithm;

public class Population
{
    public List<Individual> Individuals { get; private set; }
    public int Size => Individuals.Count;
    public Individual Best => Individuals.OrderBy(ind => ind.Fitness).First();
    
    // Create a new population with random individuals
    public Population(int size, int chromosomeLength, int[,] distanceMatrix)
    {
        Individuals = new List<Individual>(size);
        
        for (int i = 0; i < size; i++)
        {
            Individuals.Add(new Individual(chromosomeLength, distanceMatrix));
        }
    }
    
    // Create a population with predefined individuals
    public Population(List<Individual> individuals)
    {
        Individuals = new List<Individual>(individuals);
    }
    
    // Get the best n individuals (elite)
    public List<Individual> GetElite(int eliteSize)
    {
        return Individuals
            .OrderBy(ind => ind.Fitness)
            .Take(eliteSize)
            .Select(ind => ind.Clone())
            .ToList();
    }
    
    // Sort population by fitness (ascending for TSP, as we want to minimize distance)
    public void SortByFitness()
    {
        Individuals = Individuals.OrderBy(ind => ind.Fitness).ToList();
    }
    
    // Add an individual to the population
    public void AddIndividual(Individual individual)
    {
        Individuals.Add(individual);
    }
    
    // Get statistics about the population
    public (int Best, double Average, int Worst) GetStatistics()
    {
        var best = Individuals.Min(i => i.Fitness);
        var average = Individuals.Average(i => i.Fitness);
        var worst = Individuals.Max(i => i.Fitness);
        
        return (best, average, worst);
    }
}
