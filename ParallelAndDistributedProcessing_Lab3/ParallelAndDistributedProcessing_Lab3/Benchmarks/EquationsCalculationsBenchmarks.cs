using BenchmarkDotNet.Attributes;
using ParallelAndDistributedCalculations_Lab1.Data;
using ParallelAndDistributedCalculations_Lab1.EquationCalculators;
using ParallelAndDistributedCalculations_Lab1.MatrixCalculators;
using ParallelAndDistributedCalculations_Lab1.Models;
using ParallelAndDistributedProcessing_Lab3.MatrixCalculators;
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

            _data = manager.GenerateData(500, 500);
        }
        [Benchmark]
        public void ActionBlockMatrix_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new ActionBlockMultiThreadMatrixCalculator(size => 1 + size / 3);
            var calculator = new MultithreadEquationCalulator(matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void ActionBlockMatrix_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new ActionBlockMultiThreadMatrixCalculator(size => 1 + size / 3);
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }

        [Benchmark]
        public void MultiThreadMatrix_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new MultiThreadMatrixCalculator();
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void MultiThreadMatrix_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new MultiThreadMatrixCalculator();
            var calculator = new MultithreadEquationCalulator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
    }
}
