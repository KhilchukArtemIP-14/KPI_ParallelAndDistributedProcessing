using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using ParallelAndDistributedProcessing_Lab2.Benchmarks;
using ParallelAndDistributedCalculations_Lab2.Data;
using ParallelAndDistributedCalculations_Lab2.EquationCalculators;
using ParallelAndDistributedCalculations_Lab2.Matrices;
using ParallelAndDistributedCalculations_Lab2.MatrixCalculators;
using ParallelAndDistributedCalculations_Lab2.Models;
using System.Globalization;
using ParallelAndDistributedProcessing_Lab2.MatrixCalculators.MultiThreadMatrixCalculators;
using ParallelAndDistributedProcessing_Lab2.EquationCalculators.MultiThreadOutputStrategies;
using ParallelAndDistributedProcessing_Lab2.EquationCalculators.MultiThreadCalculators;
using ParallelAndDistributedProcessing_Lab2.EquationCalculators.MultiThreadEquationCalculators;

public class Program
{
    private static void Main(string[] args)
    {

        Console.WriteLine("Would you like to run synchronizators benchmarks (warning, to do so the program must be ran in release mode)? y/n");
        var key = Console.ReadLine();
        while (key != "y" && key != "n")
        {
            Console.WriteLine("Please, type y or n");
            key = Console.ReadLine();
        }
        if (key == "y")
        {
            var config = DefaultConfig.Instance;
            BenchmarkRunner.Run<SynchronizatorsBenchmarks>(config, args);
        }
        Console.WriteLine("Fetching data from file...");
        var manager = new DataManager();

        Input data =manager.GetDataFromFile("data.json");

        if (data == null)
        {
            Console.WriteLine("No data was found! Generating aand saving it to data.json ...");
            data = manager.GenerateData(300, 300);
            manager.SaveDataToFile(data,"data.json");
        }
        Console.WriteLine("Done! Now proceeding to calculations ...");

        var calculator = new BarrierMultiThreadEquationCalculator(new SemaphoreMultiThreadOutput(), new BarrierMultiThreadMatrixCalculator());
        calculator.Calculate(data, "results.txt");
        Console.WriteLine();
    }
}