namespace MH_SP_StaronTomas.GeneticAlgorithm.Operators;

public class UniformCrossover
{
    private static Random random = new Random();

    public static List<Individual> Crossover(Individual parent1, Individual parent2, int[,] distanceMatrix)
    {
        int chromosomeLength = parent1.Chromosome.Count;
        int[] childChromosome1 = new int[chromosomeLength];
        int[] childChromosome2 = new int[chromosomeLength];

        childChromosome1[0] = 0; // Prv˝ gen musÌ byù vûdy 0 (zaËiatok trasy)
        childChromosome1[chromosomeLength - 1] = 0; // Posledn˝ gen musÌ byù vûdy 0 (koniec trasy)
        childChromosome2[0] = 0;
        childChromosome2[chromosomeLength - 1] = 0;

        bool[] used1 = new bool[chromosomeLength];
        bool[] used2 = new bool[chromosomeLength];
        used1[0] = true;
        used2[0] = true;

        int[] template = new int[chromosomeLength];
        for (int i = 0; i < chromosomeLength; i++)
        {
            template[i] = random.NextDouble() < 0.5 ? 1 : 0;
        }
        template[0] = 1;
        template[chromosomeLength - 1] = 1;

        var p1 = parent1.Chromosome;
        var p2 = parent2.Chromosome;

        // Pridanie gÈnov z rodiËov do dieùaùa podæa öablÛny
        // Aplikovanie sablony na potomkov
        for (int i = 0; i < chromosomeLength; i++)
        {
            if (template[i] == 1)
            {
                childChromosome1[i] = p1[i];
                used1[p1[i]] = true;

                childChromosome2[i] = p2[i];
                used2[p2[i]] = true;
            }
        }

        // Vytvorim si fronty z nepouûit˝ch gÈnov z opaËn˝ch rodiËov...
        var remainingGenes1 = new Queue<int>(p2.Where(g => g != 0 && !used1[g]));
        var remainingGenes2 = new Queue<int>(p1.Where(g => g != 0 && !used2[g]));

        for (int i = 1; i < chromosomeLength - 1; i++)
        {
            if (childChromosome1[i] == 0 && remainingGenes1.Count > 0)
            {
                childChromosome1[i] = remainingGenes1.Dequeue();
            }
            if (childChromosome2[i] == 0 && remainingGenes2.Count > 0)
            {
                childChromosome2[i] = remainingGenes2.Dequeue();
            }
        }

        var child1 = new Individual(childChromosome1, distanceMatrix);
        var child2 = new Individual(childChromosome2, distanceMatrix);

        return new List<Individual> { child1, child2 };
    }
}
