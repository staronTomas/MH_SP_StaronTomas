namespace MH_SP_StaronTomas.GeneticAlgorithm.Operators
{
    /// <summary>
    /// Baker's Stochastic Universal Selection (SUS)
    /// 
    /// Pre TSP (kde cieľom je minimalizovať vzdialenosť), nižšia fitness hodnota je lepšia.
    /// SUS však očakáva, že vyššia hodnota fitness znamená, že daný jedinec je lepší.
    /// 
    /// Preto som použil spôsob inverzie fitnessu:
    /// invertedFitness = maxFitness - fitness + 1
    /// 
    /// Platí:
    /// - Zvyšuje selektivitu výberu (lepší jedinci majú oveľa väčšiu šancu na výber)
    /// - Zachovanie poradia jedincov (lepší jedinec má vyšší invertedFitness)
    /// - Zabezpečuje dostatočný rozptyl pravdepodobností
    /// 
    /// Pri nepoužití tohto spôsobu som dostával omnoho horšie výsledky.
    /// </summary>
    public class BakerSelection
    {
        private static Random random = new Random();

        public static List<Individual> Select(Population population, int selectionSize)
        {
            var individuals = population.Individuals;
            var selected = new List<Individual>();

            // Získať najväčšiu (najhoršiu) fitness hodnotu
            double maxFitness = individuals.Max(ind => ind.Fitness);

            // Výpočet invertovaných fitness hodnôt a ich súčet
            var invertedFitness = new double[individuals.Count];
            double totalInvertedFitness = 0;

            for (int i = 0; i < individuals.Count; i++)
            {
                invertedFitness[i] = maxFitness - individuals[i].Fitness + 1; // + 1 zaručuje, že aj najhorší jedinec má nenulový fitness(a teda má aspoň malú šancu na výber).
                totalInvertedFitness += invertedFitness[i];
            }

            // Výpočet pravdepodobnosti výberov jednotlivých jedincov
            var probabilities = new double[individuals.Count];
            for (int i = 0; i < individuals.Count; i++)
            {
                probabilities[i] = invertedFitness[i] / totalInvertedFitness;
            }

            // Vytvorenie kumulatívnych pravdepodobností
            var cumulativeProbabilities = new double[individuals.Count];
            cumulativeProbabilities[0] = probabilities[0];
            for (int i = 1; i < individuals.Count; i++)
            {
                cumulativeProbabilities[i] = cumulativeProbabilities[i - 1] + probabilities[i];
            }

            // Výber jedincov
            double randPointer = random.NextDouble() / selectionSize;
            double step = 1.0 / selectionSize; // vzdialenosť medzi jednotlivými pointermi

            // Pre každý pointer vyberiem jedinca a naplním postupne výsledný zoznam
            for (int i = 0; i < selectionSize; i++)
            {
                // Nájdenie jedinca, ktorý zodpovedá aktuálnemu pointeru..
                for (int j = 0; j < individuals.Count; j++)
                {
                    if (randPointer <= cumulativeProbabilities[j])
                    {
                        selected.Add(individuals[j].Clone());
                        break;
                    }
                }

                // Posun pointeru
                randPointer += step;
                if (randPointer >= 1.0)
                {
                    randPointer -= 1.0;
                }
            }

            return selected;
        }
    }
}
