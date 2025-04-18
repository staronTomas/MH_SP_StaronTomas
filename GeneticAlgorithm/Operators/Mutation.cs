namespace MH_SP_StaronTomas.GeneticAlgorithm.Operators;

public class Mutation
{
    private static Random random = new Random();
    
    // Swap mutation - swap two random cities
    public static void SwapMutation(Individual individual)
    {
        int chromosomeLength = individual.Chromosome.Count;
        int pos1 = random.Next(chromosomeLength);
        int pos2 = random.Next(chromosomeLength);
        
        // Make sure pos1 and pos2 are different
        while (pos1 == pos2)
        {
            pos2 = random.Next(chromosomeLength);
        }
        
        // Swap the cities
        int temp = individual.Chromosome[pos1];
        individual.Chromosome[pos1] = individual.Chromosome[pos2];
        individual.Chromosome[pos2] = temp;
        
        // Recalculate fitness
        individual.CalculateFitness();
    }
    
    // Inversion mutation - reverse a random subsequence of the route
    public static void InversionMutation(Individual individual)
    {
        int chromosomeLength = individual.Chromosome.Count;
        int pos1 = random.Next(chromosomeLength);
        int pos2 = random.Next(chromosomeLength);
        
        // Make sure pos1 and pos2 are different
        while (pos1 == pos2)
        {
            pos2 = random.Next(chromosomeLength);
        }
        
        // Ensure pos1 < pos2
        if (pos1 > pos2)
        {
            int temp = pos1;
            pos1 = pos2;
            pos2 = temp;
        }
        
        // Reverse the subsequence between pos1 and pos2
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
