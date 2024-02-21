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

public class Program
{
    private static void Main(string[] args)
    {
        /*var config = DefaultConfig.Instance;
        var summary = BenchmarkRunner.Run <MatrixOperationsBenchmarks> (config, args);

        var matrix = new Matrix<decimal>() { Values= new decimal[,] { { 1.1m, 1.2m }, { 1.3m, 1.5m } } };
        Console.WriteLine(matrix.Min);
        Console.WriteLine(matrix.Max);
        Console.WriteLine(matrix.Values);

        var manager = new DataManager();
        manager.SaveDataToFile(new Input() { A=matrix},"kek.json");

        matrix = manager.GetDataFromFile("kek.json").A;
        Console.WriteLine(matrix.Min);
        Console.WriteLine(matrix.Max);
        Console.WriteLine(matrix.Values);

        Random random = new Random();
        Console.WriteLine(manager.NextDecimal(random, 0));
        Console.WriteLine(manager.NextDecimal(random, 10));

        Console.WriteLine(manager.NextDecimal(random, 20));
        manager.SaveDataToFile(manager.GenerateData(4,4), "kek2.json");

        var calculator = new MultiThreadMatrixCalculator();
        Console.WriteLine(calculator.MultiplyByScalar(matrix, 2).Max);*/
        /*var manager = new DataManager();

        var input = manager.GenerateData(4, 4);
        var matrixCalculator = new MultiThreadMatrixCalculator();
        var equationCalulcator = new SingleThreadEquationCalculator(matrixCalculator);

        equationCalulcator.Calculate(input, "shkerke.txt");*/
        //var summary = BenchmarkRunner.Run<EquationsCalculationsBenchmarks>(config, args);
        //var summary = BenchmarkRunner.Run<MatrixOperationsBenchmarks>(config, args);

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

        Input data=manager.GetDataFromFile("data.json");

        if(data == null)
        {
            Console.WriteLine("No data was found! Generating aand saving it to data.json ...");
            data = manager.GenerateData(300, 300);
            manager.SaveDataToFile(data,"data.json");
        }
        Console.WriteLine("Done! Now proceeding to calculations ...");

        var calculator = new MultithreadEquationCalulator(new MultiThreadMatrixCalculator());
        calculator.Calculate(data, "results.txt");
    }
}