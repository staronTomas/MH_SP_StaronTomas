namespace MH_SP_StaronTomas.GeneticAlgorithm;

public class Parameters
{
    // Population parameters
    public int PopulationSize { get; set; } = 100;
    public int EliteSize { get; set; } = 5;
    
    // Evolution parameters
    public int MaxGenerations { get; set; } = 10000;
    public double MutationRate { get; set; } = 0.01;
    public double CrossoverRate { get; set; } = 0.8;
    
    // Mutation type probabilities
    public double SwapMutationProbability { get; set; } = 0.5;
    public double InversionMutationProbability { get; set; } = 0.5;
    
    // Termination criteria
    public int MaxGenerationsWithoutImprovement { get; set; } = 100;
    public int TargetFitness { get; set; } = 0;  // 0 means no target fitness
    
    // Output parameters
    public bool ShowGenerationStats { get; set; } = true;
    public int OutputFrequency { get; set; } = 10;  // Show stats every n generations
    public string OutputFilePath { get; set; } = "results.txt";
}
