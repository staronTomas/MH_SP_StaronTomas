using MH_SP_StaronTomas.Data;
using MH_SP_StaronTomas.GeneticAlgorithm;

namespace MH_SP_StaronTomas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Genetic Algorithm for Traveling Salesman Problem");
            Console.WriteLine("==============================================");
            
            try
            {
                // Load data
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string nameFile = Path.Combine(basePath, "KE", "S_CKE_0460_0001_N.txt");
                string codeFile = Path.Combine(basePath, "KE", "S_CKE_0460_0001_J.txt");
                string distanceFile = Path.Combine(basePath, "KE", "S_CKE_0460_0002_D.txt");
                
                Console.WriteLine("Loading data...");
                var cities = DataLoader.LoadCities(nameFile, codeFile);
                var distanceMatrix = DataLoader.LoadDistanceMatrix(distanceFile, cities.Count);
                Console.WriteLine($"Loaded {cities.Count} cities.");
                
                // Set up GA parameters
                var parameters = new Parameters
                {
                    PopulationSize = 100,
                    EliteSize = 5,
                    MaxGenerations = 1000,
                    MutationRate = 0.02,
                    CrossoverRate = 0.9,
                    SwapMutationProbability = 0.5,
                    InversionMutationProbability = 0.5,
                    MaxGenerationsWithoutImprovement = 500,
                    OutputFrequency = 10,
                    OutputFilePath = "ga_results.csv"
                };
                
                // Initialize and run the genetic algorithm
                Console.WriteLine("Starting genetic algorithm...");
                var ga = new GeneticAlgorithm.GeneticAlgorithm(parameters, cities, distanceMatrix);
                ga.Run();
                
                Console.WriteLine("Algorithm finished. Results saved to best_route.txt and ga_results.csv");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                Console.ReadKey();
            }
        }
    }
}
