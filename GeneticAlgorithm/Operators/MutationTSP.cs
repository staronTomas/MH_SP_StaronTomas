namespace MH_SP_StaronTomas.GeneticAlgorithm.Operators;

/// <summary>
/// Trieda zabezpecujuca mutacie pre TSP.
/// </summary>
public class MutationTSP
{
    private static Random random = new Random();

    /// <summary>
    /// Vymenna mutacia pre TSP.
    /// </summary>
    public static void SwapMutation(Individual individual)
    {
        int chromosomeLength = individual.Chromosome.Count;

        int pos1 = random.Next(1, chromosomeLength);
        int pos2 = random.Next(1, chromosomeLength);

        // Pozicie genov nesmu byt rovnake..
        while (pos1 == pos2)
        {
            pos2 = random.Next(1, chromosomeLength);
        }

        // Vymena genov
        int temp = individual.Chromosome[pos1];
        individual.Chromosome[pos1] = individual.Chromosome[pos2];
        individual.Chromosome[pos2] = temp;

        individual.CalculateFitness();
    }

    /// <summary>
    /// Inverzná mutacia pre TSP. Otoèí poradie 2 genov chromozomu.
    /// </summary>
    public static void InversionMutation(Individual individual)
    {
        int chromosomeLength = individual.Chromosome.Count;
        int pos1 = random.Next(1, chromosomeLength);
        int pos2 = random.Next(1, chromosomeLength);

        while (pos1 == pos2)
        {
            pos2 = random.Next(1, chromosomeLength);
        }

        // kvoli otacanu poradia potrebujem indexy, aby boli takto nastavene..
        if (pos1 > pos2)
        {
            int temp = pos1;
            pos1 = pos2;
            pos2 = temp;
        }

        // otocim poradie
        while (pos1 < pos2)
        {
            int temp = individual.Chromosome[pos1];
            individual.Chromosome[pos1] = individual.Chromosome[pos2];
            individual.Chromosome[pos2] = temp;
            pos1++;
            pos2--;
        }

        individual.CalculateFitness();
    }
}