namespace MH_SP_StaronTomas.GeneticAlgorithm.Operators;

/// <summary>
/// Mutation class for the genetic algorithm for Traveling Salesman Problem.
/// </summary>
public class MutationTSP
{
    private static Random random = new Random();

    /// <summary>
    /// Swapping mutation for the TSP problem. This mutation swaps 2 gens in the chromosome.
    /// </summary>
    /// <param name="individual">Individual whose chromosome will be mutated.</param>
    public static void SwapMutation(Individual individual)
    {
        int chromosomeLength = individual.Chromosome.Count;
        int pos1 = random.Next(chromosomeLength);
        int pos2 = random.Next(chromosomeLength);

        // In a case where pos1 and pos2 are the same, we need to select a different position
        while (pos1 == pos2)
        {
            pos2 = random.Next(chromosomeLength);
        }
        
        // Swap the gens
        int temp = individual.Chromosome[pos1];
        individual.Chromosome[pos1] = individual.Chromosome[pos2];
        individual.Chromosome[pos2] = temp;
        
        // Recalculate fitness
        individual.CalculateFitness();
    }

    /// <summary>
    /// Inversion mutation for the TSP problem. This mutation reverses a subsequence of 2 gens in the chromosome.
    /// </summary>
    /// <param name="individual">Individual whose chromosome whill be mutated.</param>
    public static void InversionMutation(Individual individual)
    {
        var chromosome = individual.Chromosome;

        int gen1 = random.Next(chromosome.Count);
        int gen2 = random.Next(chromosome.Count);

        // In a case where pos1 and pos2 are the same, we need to select a different position
        while (gen1 == gen2)
        {
            gen2 = random.Next(chromosome.Count);
        }
        
        // Ensure pos1 < pos2
        if (gen1 > gen2)
        {
            int temp = gen1;
            gen1 = gen2;
            gen2 = temp;
        }
        
        // Reverse the subsequence
        while (gen1 < gen2)
        {
            int temp = chromosome[gen1];
            chromosome[gen1] = chromosome[gen2];
            chromosome[gen2] = temp;
            gen1++;
            gen2--;
        }
        
        // Recalculate fitness
        individual.CalculateFitness();
    }
}
