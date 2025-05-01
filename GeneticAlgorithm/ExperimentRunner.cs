using MH_SP_StaronTomas.Data;
using System.Diagnostics;

namespace MH_SP_StaronTomas.GeneticAlgorithm;

/// <summary>
/// Trieda na spustenie viacerých behov genetického algoritmu a vyhodnotenie najlepších výsledkov
/// </summary>
public class ExperimentRunner
{
    private List<City> Cities { get; set; }
    private int[,] DistanceMatrix { get; set; }
    private Parameters BaseParameters { get; set; }
    private List<ExperimentResult> Results { get; set; }
    private ExperimentResult BestRun { get; set; }

    public ExperimentRunner(List<City> cities, int[,] distanceMatrix, Parameters parameters)
    {
        Cities = cities;
        DistanceMatrix = distanceMatrix;
        BaseParameters = parameters;
        Results = new List<ExperimentResult>();
    }

    public void RunExperiments(int numberOfRuns)
    {
        Console.WriteLine($"Starting {numberOfRuns} experiments with Genetic Algorithm...");
        Console.WriteLine("Base parameters:");
        Console.WriteLine($"- Population size: {BaseParameters.PopulationSize}");
        Console.WriteLine($"- Elite size: {BaseParameters.EliteSize}");
        Console.WriteLine($"- Crossover rate: {BaseParameters.CrossoverRate}");
        Console.WriteLine($"- Mutation rate: {BaseParameters.MutationRate}");
        Console.WriteLine($"- Max generations: {BaseParameters.MaxGenerations}");
        Console.WriteLine($"- Max generations without improvement: {BaseParameters.MaxGenerationsWithoutImprovement}");
        Console.WriteLine();

        for (int run = 1; run <= numberOfRuns; run++)
        {
            Console.WriteLine($"Run {run}/{numberOfRuns}:");

            var runParameters = new Parameters
            {
                PopulationSize = BaseParameters.PopulationSize,
                EliteSize = BaseParameters.EliteSize,
                CrossoverRate = BaseParameters.CrossoverRate,
                MutationRate = BaseParameters.MutationRate,
                MaxGenerations = BaseParameters.MaxGenerations,
                MaxGenerationsWithoutImprovement = BaseParameters.MaxGenerationsWithoutImprovement,
                SwapMutationProbability = BaseParameters.SwapMutationProbability,
                InversionMutationProbability = BaseParameters.InversionMutationProbability,
                StartEndCityIndex = BaseParameters.StartEndCityIndex
            };

            var ga = new GeneticAlgorithm(runParameters, Cities, DistanceMatrix);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ga.Solve();

            stopwatch.Stop();
            TimeSpan executionTime = stopwatch.Elapsed;

            var result = new ExperimentResult
            {
                RunNumber = run,
                BestFitness = ga.GetBestFitness(),
                BestSolution = ga.GetBestSolution(),
                ExecutionTime = executionTime,
                GenerationsCount = ga.GetGenerationsCount()
            };

            Results.Add(result);

            // aktualizacia najlepsieho behu
            if (BestRun == null || result.BestFitness < BestRun.BestFitness)
            {
                BestRun = result;
            }

            // Zapis vysledkov...
            using (StreamWriter writer = new StreamWriter($"run_{run}_result.txt"))
            {
                writer.WriteLine($"Run {run} Results:");
                writer.WriteLine($"Best Distance: {result.BestFitness}");
                writer.WriteLine($"Execution Time: {result.ExecutionTime}");
                writer.WriteLine($"Generations: {result.GenerationsCount}");
                writer.WriteLine("\nBest Route:");
                writer.WriteLine($"Start: {Cities[0].Name} (ID: {Cities[0].Id})");

                for (int i = 1; i < result.BestSolution.Count; i++)
                {
                    int cityIndex = result.BestSolution[i];
                    writer.WriteLine($"{i}. {Cities[cityIndex].Name} (ID: {Cities[cityIndex].Id})");
                }

                writer.WriteLine($"Return to: {Cities[0].Name} (ID: {Cities[0].Id})");
            }


            Console.WriteLine($"Completed with best fitness: {result.BestFitness}, time: {result.ExecutionTime}");
            Console.WriteLine();
        }

        // Vyhodnotenie vsetkych behov
        AnalyzeResults();
    }

    private void AnalyzeResults()
    {
        if (Results.Count == 0)
        {
            Console.WriteLine("No experiments to analyze.");
            return;
        }

        double avgFitness = Results.Average(r => r.BestFitness);
        double minFitness = Results.Min(r => r.BestFitness);
        double maxFitness = Results.Max(r => r.BestFitness);

        double avgTime = Results.Average(r => r.ExecutionTime.TotalSeconds);
        double minTime = Results.Min(r => r.ExecutionTime.TotalSeconds);
        double maxTime = Results.Max(r => r.ExecutionTime.TotalSeconds);

        double avgGenerations = Results.Average(r => r.GenerationsCount);
        int minGenerations = Results.Min(r => r.GenerationsCount);
        int maxGenerations = Results.Max(r => r.GenerationsCount);

        Console.WriteLine("=== Celkove vysledky ===");
        Console.WriteLine($"Pocet behov: {Results.Count}");
        Console.WriteLine("\nFitness statistiky:");
        Console.WriteLine($"- Priemer: {avgFitness:F2}");
        Console.WriteLine($"- Najlepsi: {minFitness}");
        Console.WriteLine($"- Najhorsi: {maxFitness}");

        Console.WriteLine("\nDlžká trvania (sekundy):");
        Console.WriteLine($"- Priemer: {avgTime:F2}");
        Console.WriteLine($"- Minimum: {minTime:F2}");
        Console.WriteLine($"- Maximum: {maxTime:F2}");

        Console.WriteLine("\nPočet vygenerovaných generácií:");
        Console.WriteLine($"- Priemer: {avgGenerations:F2}");
        Console.WriteLine($"- Minimum: {minGenerations}");
        Console.WriteLine($"- Maximum: {maxGenerations}");

        Console.WriteLine("\n=== Najlepší výsledok ===");
        Console.WriteLine($"Číslo behu: {BestRun.RunNumber}");
        Console.WriteLine($"Najlepší výsledok: {BestRun.BestFitness}");
        Console.WriteLine($"Čas trvania: {BestRun.ExecutionTime}");
        Console.WriteLine($"Počet generácií: {BestRun.GenerationsCount}");

        SaveSummaryResults(avgFitness, minFitness, maxFitness, avgTime, 
            minTime, maxTime, avgGenerations, minGenerations, maxGenerations);

        SaveBestRunDetails();
    }

    /// <summary>
    /// Uloží súhrnné výsledky experimentov do súboru
    /// </summary>
    private void SaveSummaryResults(double avgFitness, double minFitness, double maxFitness, double avgTime,
        double minTime, double maxTime, double avgGenerations, int minGenerations, int maxGenerations)
    {
        using (StreamWriter writer = new StreamWriter("experiment_summary.txt"))
        {
            writer.WriteLine("=== Genetic Algorithm Experiment Results ===");
            writer.WriteLine($"Date: {DateTime.Now}");
            writer.WriteLine($"Total runs: {Results.Count}");

            writer.WriteLine("\nParameters:");
            writer.WriteLine($"- Veľkosť populácie: {BaseParameters.PopulationSize}");
            writer.WriteLine($"- Veľkosť elitných jedincov: {BaseParameters.EliteSize}");
            writer.WriteLine($"- Pravdepodobnost krizenia: {BaseParameters.CrossoverRate}");
            writer.WriteLine($"- Pravdepodobnost mutacie: {BaseParameters.MutationRate}");
            writer.WriteLine($"- Max pocet generacii: {BaseParameters.MaxGenerations}");
            writer.WriteLine($"- Max pocet generacii bez zlepsenia: {BaseParameters.MaxGenerationsWithoutImprovement}");

            writer.WriteLine("\nFitness štatistiky:");
            writer.WriteLine($"- Priemer: {avgFitness:F2}");
            writer.WriteLine($"- Najlepší: {minFitness}");
            writer.WriteLine($"- Najhorší: {maxFitness}");

            writer.WriteLine("\nDobra trvania (seconds):");
            writer.WriteLine($"- Priemer: {avgTime:F2}");
            writer.WriteLine($"- Minimum: {minTime:F2}");
            writer.WriteLine($"- Maximum: {maxTime:F2}");

            writer.WriteLine("\nPočet generácií:");
            writer.WriteLine($"- Average: {avgGenerations:F2}");
            writer.WriteLine($"- Minimum: {minGenerations}");
            writer.WriteLine($"- Maximum: {maxGenerations}");

            writer.WriteLine("\nAll Runs:");
            writer.WriteLine("Run\tFitness\tTime(s)\tGenerations");
            foreach (var result in Results)
            {
                writer.WriteLine($"{result.RunNumber}\t{result.BestFitness}\t{result.ExecutionTime.TotalSeconds:F2}\t{result.GenerationsCount}");
            }
        }
    }

    /// <summary>
    /// Uloží detailné výsledky najlepšieho behu do súboru
    /// </summary>
    private void SaveBestRunDetails()
    {
        using (StreamWriter writer = new StreamWriter("best_run_result.txt"))
        {
            writer.WriteLine("=== Najlepší beh programum ===");
            writer.WriteLine($"Číslo behu: {BestRun.RunNumber}");
            writer.WriteLine($"Najlepší výsledok: {BestRun.BestFitness}");
            writer.WriteLine($"Dobra trvania: {BestRun.ExecutionTime}");
            writer.WriteLine($"Počet generácií: {BestRun.GenerationsCount}");

            writer.WriteLine("\nNajlepšia trasa:");
            writer.WriteLine($"Začiatok: {Cities[0].Name} (ID: {Cities[0].Id})");

            for (int i = 1; i < BestRun.BestSolution.Count; i++)
            {
                int cityIndex = BestRun.BestSolution[i];
                writer.WriteLine($"{i}. {Cities[cityIndex].Name} (ID: {Cities[cityIndex].Id})");
            }

            writer.WriteLine($"Koniec: {Cities[0].Name} (ID: {Cities[0].Id})");
        }
    }
}

/// <summary>
/// Trieda reprezentujúca výsledok jedného behu experimentu
/// </summary>
public class ExperimentResult
{
    public int RunNumber { get; set; }
    public int BestFitness { get; set; }
    public List<int> BestSolution { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public int GenerationsCount { get; set; }
}