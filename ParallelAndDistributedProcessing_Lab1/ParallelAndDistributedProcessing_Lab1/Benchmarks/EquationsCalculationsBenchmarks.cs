using BenchmarkDotNet.Attributes;
using ParallelAndDistributedCalculations_Lab1.Data;
using ParallelAndDistributedCalculations_Lab1.EquationCalculators;
using ParallelAndDistributedCalculations_Lab1.MatrixCalculators;
using ParallelAndDistributedCalculations_Lab1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedCalculations_Lab1.Benchmarks
{
    public class EquationsCalculationsBenchmarks
    {
        private Input _data;

        [GlobalSetup]
        public void PrepareData()
        {
            var manager = new DataManager();

            _data = manager.GenerateData(300, 300);
        }

        [Benchmark]
        public void MultiThreadMatrix_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new MultiThreadMatrixCalculator();
            var calculator = new MultithreadEquationCalulator(matrixCalculator);


            calculator.Calculate(_data, "kekeke.txt");
        }

        [Benchmark]
        public void MultiThreadMatrix_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new MultiThreadMatrixCalculator();
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "kekeke.txt");
        }
        [Benchmark]
        public void SingleThreadMatrix_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new SingleThreadMatrixCalculator();
            var calculator = new MultithreadEquationCalulator(matrixCalculator);


            calculator.Calculate(_data, "kekeke.txt");
        }
        [Benchmark]
        public void SingleThreadMatrix_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new SingleThreadMatrixCalculator();
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "kekeke.txt");
        }
    }
}
