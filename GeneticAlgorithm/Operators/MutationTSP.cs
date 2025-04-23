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
        // Generate positions (skip start/end city)
        int pos1 = random.Next(1, chromosomeLength);
        int pos2 = random.Next(1, chromosomeLength);

        // In a case where pos1 and pos2 are the same, we need to select a different position
        while (pos1 == pos2)
        {
            pos2 = random.Next(1, chromosomeLength);
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
        int chromosomeLength = individual.Chromosome.Count;
        int pos1 = random.Next(1, chromosomeLength);
        int pos2 = random.Next(1, chromosomeLength);

        // In a case where pos1 and pos2 are the same, we need to select a different position
        while (pos1 == pos2)
        {
            pos2 = random.Next(1, chromosomeLength);
        }

        // Ensure pos1 < pos2
        if (pos1 > pos2)
        {
            int temp = pos1;
            pos1 = pos2;
            pos2 = temp;
        }

        // Reverse the subsequence
        while (pos1 < pos2)
        {
            int temp = individual.Chromosome[pos1];
            individual.Chromosome[pos1] = individual.Chromosome[pos2];
            individual.Chromosome[pos2] = temp;
            pos1++;
            pos2--;
        }

        // Recalculate fitness
        individual.CalculateFitness();
    }
}