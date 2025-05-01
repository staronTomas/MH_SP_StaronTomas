namespace MH_SP_StaronTomas.GeneticAlgorithm;

public class Population
{
    public List<Individual> Individuals { get; private set; }
    public int Size => Individuals.Count;
    public Individual Best => Individuals.OrderBy(ind => ind.Fitness).First();
    
    public Population(int size, int chromosomeLength, int[,] distanceMatrix)
    {
        Individuals = new List<Individual>(size);
        
        for (int i = 0; i < size; i++)
        {
            Individuals.Add(new Individual(chromosomeLength, distanceMatrix));
        }
    }
    
    public Population(List<Individual> individuals)
    {
        Individuals = new List<Individual>(individuals);
    }
    
    public List<Individual> GetElite(int eliteSize)
    {
        return Individuals
            .OrderBy(ind => ind.Fitness)
            .Take(eliteSize)
            .Select(ind => ind.Clone())
            .ToList();
    }
    
    public void SortByFitness()
    {
        Individuals = Individuals.OrderBy(ind => ind.Fitness).ToList();
    }
    
    public void AddIndividual(Individual individual)
    {
        Individuals.Add(individual);
    }
    
    public double GetStatistics()
    {
        return Individuals.Min(i => i.Fitness);
    }
}
