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
            var parameters = new Parameters();

            Console.WriteLine("Vyber rezimu:");
            Console.WriteLine("1. Sputiť raz");
            Console.WriteLine("2. Spustiť viac krát");
            
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
            {
                Console.WriteLine("Zvolte pocet:");
            }

            if (choice == 1)
            {
                Console.WriteLine("Spustam jeden beh genetickeho algoritmu..");
                Console.WriteLine($"Start city: {cities[parameters.StartEndCityIndex].Name}");
                Console.WriteLine($"End city: {cities[parameters.StartEndCityIndex].Name}");

                var ga = new GeneticAlgorithm.GeneticAlgorithm(parameters, cities, distanceMatrix);
                ga.Solve();
            }
            else
            {
                Console.WriteLine("Zvolte pocet behov:");
                int numberOfRuns;
                while (!int.TryParse(Console.ReadLine(), out numberOfRuns) || numberOfRuns <= 0)
                {
                    Console.WriteLine("Prosim zvolte kladne cislo:");
                }

                var experimentRunner = new ExperimentRunner(cities, distanceMatrix, parameters);
                experimentRunner.RunExperiments(numberOfRuns);

                Console.WriteLine("Experimenty skoncily. Vysledky boli ulozene");
            }
        }
    }
}
