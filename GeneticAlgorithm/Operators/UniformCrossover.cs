namespace MH_SP_StaronTomas.GeneticAlgorithm.Operators;

public class UniformCrossover
{
    private static Random random = new Random();

    public static List<Individual> Crossover(Individual parent1, Individual parent2, int[,] distanceMatrix)
    {
        int chromosomeLength = parent1.Chromosome.Count;
        int[] childChromosome1 = new int[chromosomeLength];
        childChromosome1[0] = 0; // Prv� gen mus� by� v�dy 0 (za�iatok trasy)
        childChromosome1[chromosomeLength - 1] = 0; // Posledn� gen mus� by� v�dy 0 (koniec trasy)
        int[] childChromosome2 = new int[chromosomeLength];
        childChromosome2[0] = 0;
        childChromosome2[chromosomeLength - 1] = 0;

        var used1 = new bool[chromosomeLength];
        used1[0] = true;

        var used2 = new bool[chromosomeLength];
        used2[0] = true;

        // Sablona ktora ponecha geny nedotknute tam kde je 1.
        int[] template = new int[chromosomeLength];
        for (int i = 0; i < chromosomeLength; i++)
        {
            template[i] = random.NextDouble() < 0.5 ? 1 : 0;
        }
        template[0] = 1; // Prv� gen mus� by� v�dy 0 (za�iatok trasy)
        template[chromosomeLength - 1] = 1; // Posledn� gen mus� by� v�dy 0 (koniec trasy)

        // Pridanie g�nov z rodi�ov do die�a�a pod�a �abl�ny
        // Aplikovanie sablony na potomkov
        for (int i = 0; i < chromosomeLength; i++)
        {
            if (template[i] == 1)
            {
                childChromosome1[i] = (parent1.Chromosome[i]);
                used1[parent1.Chromosome[i]] = true;

                childChromosome2[i] = (parent2.Chromosome[i]);
                used2[parent2.Chromosome[i]] = true;
            }
        }

        // Dopln�me g�nmi z rodi�ov, ktor� neboli pou�it�
        // Pre die�a 1
        for (int i = 1; i < chromosomeLength - 1; i++)
        {
            // Ak nie je zadan� g�n v die�ati 1, prid�me prv� g�n z rodi�a 2, ktor� nebol pou�it�
            if (childChromosome1[i] == 0)
            {
                for (int j = 1; j < chromosomeLength - 1; j++)
                {
                    if (!used1[parent2.Chromosome[j]])
                    {
                        childChromosome1[i] = parent2.Chromosome[j];
                        used1[parent2.Chromosome[j]] = true;
                        break;
                    }
                }
            }

            if (childChromosome2[i] == 0)
            {
                for (int j = 1; j < chromosomeLength - 1; j++)
                {
                    if (!used2[parent1.Chromosome[j]])
                    {
                        childChromosome2[i] = parent1.Chromosome[j];
                        used2[parent1.Chromosome[j]] = true;
                        break;
                    }
                }
            }
        }

        var child1 = new Individual(childChromosome1.ToList(), distanceMatrix);
        var child2 = new Individual(childChromosome2.ToList(), distanceMatrix);

        return new List<Individual> { child1, child2 };
    }
}
