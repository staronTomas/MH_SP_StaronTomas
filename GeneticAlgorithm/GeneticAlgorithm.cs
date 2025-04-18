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
        
        // Initialize results file
        ResultsWriter = new StreamWriter(Parameters.OutputFilePath);
        ResultsWriter.WriteLine("Generation,Best Fitness,Average Fitness,Worst Fitness");
    }
    
    public void Run()
    {
        // Initialize population
        CurrentPopulation = new Population(Parameters.PopulationSize, Cities.Count, DistanceMatrix);
        
        // Find the initial best individual
        BestIndividual = CurrentPopulation.Best.Clone();
        
        // Log initial statistics
        WriteGenerationStats(0);
        
        // Evolution loop
        for (int generation = 1; generation <= Parameters.MaxGenerations; generation++)
        {
            // Create new population
            List<Individual> newPopulation = new List<Individual>();
            
            // Add elite individuals
            newPopulation.AddRange(CurrentPopulation.GetElite(Parameters.EliteSize));
            
            // Selection, crossover and mutation
            while (newPopulation.Count < Parameters.PopulationSize)
            {
                // Selection
                List<Individual> parents = BakerSelection.Select(CurrentPopulation, 2);
                
                Individual child;
                
                // Crossover
                if (Random.NextDouble() < Parameters.CrossoverRate)
                {
                    child = UniformCrossover.Crossover(parents[0], parents[1], DistanceMatrix);
                }
                else
                {
                    // No crossover, just clone a parent
                    child = parents[0].Clone();
                }
                
                // Mutation
                if (Random.NextDouble() < Parameters.MutationRate)
                {
                    // Choose mutation type based on probabilities
                    double mutationType = Random.NextDouble();
                    if (mutationType < Parameters.SwapMutationProbability)
                    {
                        Mutation.SwapMutation(child);
                    }
                    else
                    {
                        Mutation.InversionMutation(child);
                    }
                }
                
                newPopulation.Add(child);
            }
            
            // Update current population
            CurrentPopulation = new Population(newPopulation);
            
            // Check if we have a new best individual
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
            
            // Log statistics
            if (Parameters.ShowGenerationStats && generation % Parameters.OutputFrequency == 0)
            {
                WriteGenerationStats(generation);
            }
            
            // Termination criteria check
            if (Parameters.TargetFitness > 0 && BestIndividual.Fitness <= Parameters.TargetFitness)
            {
                Console.WriteLine($"Target fitness reached in generation {generation}!");
                break;
            }
            
            if (GenerationsWithoutImprovement >= Parameters.MaxGenerationsWithoutImprovement)
            {
                Console.WriteLine($"No improvement for {Parameters.MaxGenerationsWithoutImprovement} generations. Stopping.");
                break;
            }
        }
        
        // Final results
        Console.WriteLine("\nGenetic Algorithm Finished");
        WriteResults();
        
        // Close results file
        ResultsWriter.Close();
    }
    
    private void WriteGenerationStats(int generation)
    {
        var stats = CurrentPopulation.GetStatistics();
        
        Console.WriteLine($"Generation {generation}: Best = {stats.Best}, Avg = {stats.Average:F2}, Worst = {stats.Worst}");
        
        // Write to file
        ResultsWriter.WriteLine($"{generation},{stats.Best},{stats.Average},{stats.Worst}");
    }
    
    private void WriteResults()
    {
        Console.WriteLine("\nBest solution found:");
        Console.WriteLine($"Distance: {BestIndividual.Fitness}");
        
        // Print the route with city names
        Console.WriteLine("\nRoute:");
        for (int i = 0; i < BestIndividual.Chromosome.Count; i++)
        {
            int cityIndex = BestIndividual.Chromosome[i];
            Console.WriteLine($"{i+1}. {Cities[cityIndex].Name} (ID: {Cities[cityIndex].Id})");
        }
        
        // Write to separate results file
        using (StreamWriter routeWriter = new StreamWriter("best_route.txt"))
        {
            routeWriter.WriteLine($"Best Distance: {BestIndividual.Fitness}");
            routeWriter.WriteLine("\nRoute:");
            
            for (int i = 0; i < BestIndividual.Chromosome.Count; i++)
            {
                int cityIndex = BestIndividual.Chromosome[i];
                routeWriter.WriteLine($"{i+1}. {Cities[cityIndex].Name} (ID: {Cities[cityIndex].Id})");
            }
        }
    }
}
