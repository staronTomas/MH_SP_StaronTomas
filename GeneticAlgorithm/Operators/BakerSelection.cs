namespace MH_SP_StaronTomas.GeneticAlgorithm.Operators;

public class BakerSelection
{
    private static Random random = new Random();

    /// <summary>
    /// Baker's Stochastic Universal Selection
    /// For TSP, we need to invert fitness since we want to minimize distance
    /// </summary>
    public static List<Individual> Select(Population population, int selectionSize)
    {
        var individuals = population.Individuals;
        var selected = new List<Individual>();
        
        double totalFitness = 0;
        var invertedFitness = new double[individuals.Count];
        double maxFitness = individuals.Max(ind => ind.Fitness);
        
        for (int i = 0; i < individuals.Count; i++)
        {
            // Invert fitness: higher fitness for shorter distances
            invertedFitness[i] = maxFitness - individuals[i].Fitness + 1;
            totalFitness += invertedFitness[i];
        }
        
        // Calculate selection probabilities
        var probabilities = new double[individuals.Count];
        for (int i = 0; i < individuals.Count; i++)
        {
            probabilities[i] = invertedFitness[i] / totalFitness;
        }
        
        // Calculate cumulative probabilities
        var cumulativeProbabilities = new double[individuals.Count];
        cumulativeProbabilities[0] = probabilities[0];
        for (int i = 1; i < individuals.Count; i++)
        {
            cumulativeProbabilities[i] = cumulativeProbabilities[i - 1] + probabilities[i];
        }
        
        // Baker's SUS: generate equally spaced pointers
        double pointer = random.NextDouble() / selectionSize;
        double step = 1.0 / selectionSize;
        
        for (int i = 0; i < selectionSize; i++)
        {
            // Find the individual that corresponds to the pointer
            for (int j = 0; j < individuals.Count; j++)
            {
                if (pointer <= cumulativeProbabilities[j])
                {
                    selected.Add(individuals[j].Clone());
                    break;
                }
            }
            
            // Move the pointer
            pointer += step;
            if (pointer >= 1.0)
            {
                pointer -= 1.0;
            }
        }
        
        return selected;
    }
}
