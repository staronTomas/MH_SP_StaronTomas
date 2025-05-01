namespace MH_SP_StaronTomas.GeneticAlgorithm;

public class Parameters
{
    public int PopulationSize { get; set; } = 100;
    public int EliteSize { get; set; } = 25;
    public int MaxGenerations { get; set; } = 10000000;
    public double MutationRate { get; set; } = 0.5;
    public double CrossoverRate { get; set; } = 0.5;
    public double SwapMutationProbability { get; set; } = 0.5;
    public double InversionMutationProbability { get; set; } = 0.5;
    public int MaxGenerationsWithoutImprovement { get; set; } = 20000;
    public int StartEndCityIndex { get; set; } = 0;
}
