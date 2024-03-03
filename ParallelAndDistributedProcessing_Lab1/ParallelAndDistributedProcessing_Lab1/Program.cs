using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using ParallelAndDistributedCalculations_Lab1.Benchmarks;
using ParallelAndDistributedCalculations_Lab1.Data;
using ParallelAndDistributedCalculations_Lab1.EquationCalculators;
using ParallelAndDistributedCalculations_Lab1.Matrices;
using ParallelAndDistributedCalculations_Lab1.MatrixCalculators;
using ParallelAndDistributedCalculations_Lab1.Models;
using System.Globalization;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Would you like to run multi vs single -thread matrix operations benchmarks (warning, to do so the program must be ran in release mode)? y/n");
        var key = Console.ReadLine();
        while(key != "y" && key!="n") 
        {
            Console.WriteLine("Please, type y or n");
            key = Console.ReadLine();
        }
        if(key == "y")
        {
            var config = DefaultConfig.Instance;
            BenchmarkRunner.Run<MatrixOperationsBenchmarks>(config, args);
        }

        Console.WriteLine("Would you like to run multi vs single -thread equation calculation benchmarks (warning, to do so the program must be ran in release mode)? y/n");
        key = Console.ReadLine();
        while (key != "y" && key != "n")
        {
            Console.WriteLine("Please, type y or n");
            key = Console.ReadLine();
        }
        if (key == "y")
        {
            var config = DefaultConfig.Instance;
            BenchmarkRunner.Run<EquationsCalculationsBenchmarks>(config, args);
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

        var calculator = new MultithreadEquationCalulator(new MultiThreadMatrixCalculator());
        calculator.Calculate(data, "results.txt");
        Console.WriteLine();
    }
}