using MH_SP_StaronTomas.Data;
using MH_SP_StaronTomas.GeneticAlgorithm.Operators;

namespace MH_SP_StaronTomas.GeneticAlgorithm;

public class GeneticAlgorithm
{
    private Parameters Parameters { get; set; }
    private int[,] DistanceMatrix { get; set; }
    private List<City> Cities { get; set; }
    private Population CurrentPopulation { get; set; }
    private Individual BestIndividual { get; set; }
    private Random Random { get; set; }
    
    private int GenerationsWithoutImprovement { get; set; }
    private StreamWriter ResultsWriter { get; set; }
    
    public GeneticAlgorithm(Parameters parameters, List<City> cities, int[,] distanceMatrix)
    {
        Parameters = parameters;
        Cities = cities;
        DistanceMatrix = distanceMatrix;
        Random = new Random();
    }
    
    public void Solve()
    {
        // Initialize population
        // Chromosome length is cities count + 1 for the return to the starting city
        CurrentPopulation = new Population(Parameters.PopulationSize, Cities.Count + 1, DistanceMatrix);
        
        BestIndividual = CurrentPopulation.Best.Clone();
        
        WriteGenerationStats(0);
        
        // Evolution loop
        for (int generation = 1; generation <= Parameters.MaxGenerations; generation++)
        {
            List<Individual> newPopulation = new List<Individual>();
            
            newPopulation.AddRange(CurrentPopulation.GetElite(Parameters.EliteSize));

            // Pomocou selekcií, kríženia a mutácií vyberám dalších potomkov z populácie
            while (newPopulation.Count < Parameters.PopulationSize)
            {
                // Selekcia Baker's SUS
                List<Individual> parents = BakerSelection.Select(CurrentPopulation, 2);

                // Krizenie
                List<Individual> children;
                if (Random.NextDouble() < Parameters.CrossoverRate)
                {
                    children = UniformCrossover.Crossover(parents[0], parents[1], DistanceMatrix);
                }
                else
                {
                    // Nerobim krizenie, iba nakopirujem rodicov
                    children = new List<Individual> { parents[0].Clone(), parents[1].Clone() };
                }

                // Mutacie
                foreach (var child in children)
                {
                    if (Random.NextDouble() < Parameters.MutationRate)
                    {
                        // Vyber typu mutacie random
                        double mutationType = Random.NextDouble();
                        if (mutationType < Parameters.SwapMutationProbability)
                        {
                            MutationTSP.SwapMutation(child);
                        }
                        else
                        {
                            MutationTSP.InversionMutation(child);
                        }
                    }
                }

                foreach (var child in children)
                {
                    newPopulation.Add(child);
                }
            }
            
            CurrentPopulation = new Population(newPopulation);
            
            // Kontrola ci vznikol novy najlepsi jedinec
            Individual currentBest = CurrentPopulation.Best;
            if (currentBest.Fitness < BestIndividual.Fitness)
            {
                BestIndividual = currentBest.Clone();
                GenerationsWithoutImprovement = 0;
            }
            else
            {
                GenerationsWithoutImprovement++;
            }
            
            if (generation % 50 == 0)
            {
                WriteGenerationStats(generation);
            }

            if (GenerationsWithoutImprovement >= Parameters.MaxGenerationsWithoutImprovement)
            {
                Console.WriteLine($"No improvement for {Parameters.MaxGenerationsWithoutImprovement} generations. Stopping.");
                break;
            }
        }
        
        Console.WriteLine("\nGenetic Algorithm Finished");
        WriteResults();
    }
    
    private void WriteGenerationStats(int generation)
    {
        var best = CurrentPopulation.GetStatistics();
        
        Console.WriteLine($"Generation {generation}: Best = {best}");
    }
    
    private void WriteResults()
    {
        Console.WriteLine("\nBest solution found:");
        Console.WriteLine($"Distance: {BestIndividual.Fitness}");
        
        // Print the route with city names
        Console.WriteLine("\nRoute:");
        Console.WriteLine($"Start: {Cities[0].Name} (ID: {Cities[0].Id})");
        
        for (int i = 1; i < BestIndividual.Chromosome.Count; i++)
        {
            int currentCityIndex = BestIndividual.Chromosome[i];
            int previousCityIndex = BestIndividual.Chromosome[i - 1];
            int distance = DistanceMatrix[previousCityIndex, currentCityIndex];
            
            Console.WriteLine($"{i}. {Cities[currentCityIndex].Name} (ID: {Cities[currentCityIndex].Id}) - Distance from previous: {distance}");
        }
        
        Console.WriteLine($"Return to: {Cities[0].Name} (ID: {Cities[0].Id})");
        
        using (StreamWriter routeWriter = new StreamWriter("best_route.txt"))
        {
            routeWriter.WriteLine($"Best Distance: {BestIndividual.Fitness}");
            routeWriter.WriteLine("\nRoute:");
            routeWriter.WriteLine($"Start: {Cities[0].Name} (ID: {Cities[0].Id})");
            
            for (int i = 1; i < BestIndividual.Chromosome.Count; i++)
            {
                int currentCityIndex = BestIndividual.Chromosome[i];
                int previousCityIndex = BestIndividual.Chromosome[i - 1];
                int distance = DistanceMatrix[previousCityIndex, currentCityIndex];
                
                routeWriter.WriteLine($"{i}. {Cities[currentCityIndex].Name} (ID: {Cities[currentCityIndex].Id}) - Distance from previous: {distance}");
            }
            
            routeWriter.WriteLine($"Return to: {Cities[0].Name} (ID: {Cities[0].Id})");
        }
    }

    /// <summary>
    /// Vráti fitness hodnotu najlepšieho nájdeného riešenia
    /// </summary>
    public int GetBestFitness()
    {
        return BestIndividual.Fitness;
    }

    /// <summary>
    /// Vráti najlepšie nájdené riešenie (trasu)
    /// </summary>
    public List<int> GetBestSolution()
    {
        return new List<int>(BestIndividual.Chromosome);
    }

    /// <summary>
    /// Vráti počet generácií, ktoré algoritmus vykonal
    /// </summary>
    public int GetGenerationsCount()
    {
        return Parameters.MaxGenerations - GenerationsWithoutImprovement;
    }
}
