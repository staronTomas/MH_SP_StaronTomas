namespace MH_SP_StaronTomas.GeneticAlgorithm.Operators;

public class UniformCrossover
{
    private static Random random = new Random();
    
    public static Individual Crossover(Individual parent1, Individual parent2, int[,] distanceMatrix)
    {
        int chromosomeLength = parent1.Chromosome.Count;
        var childChromosome = new List<int>();
        var used = new bool[chromosomeLength];
        
        // Randomly select genes from either parent
        for (int i = 0; i < chromosomeLength; i++)
        {
            // Decide which parent to take the gene from
            int gene;
            if (random.Next(2) == 0)
            {
                gene = parent1.Chromosome[i];
            }
            else
            {
                gene = parent2.Chromosome[i];
            }
            
            // If the gene is already in the child, find the nearest unused gene
            if (used[gene])
            {
                // Find the closest unused city to the current gene
                int bestCity = -1;
                int minDistance = int.MaxValue;
                
                for (int j = 0; j < chromosomeLength; j++)
                {
                    if (!used[j])
                    {
                        int distance = distanceMatrix[gene, j];
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            bestCity = j;
                        }
                    }
                }
                
                gene = bestCity;
            }
            
            childChromosome.Add(gene);
            used[gene] = true;
        }
        
        return new Individual(childChromosome, distanceMatrix);
    }
}
