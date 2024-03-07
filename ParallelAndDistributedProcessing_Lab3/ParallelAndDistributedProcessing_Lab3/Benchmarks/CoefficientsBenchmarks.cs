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

namespace ParallelAndDistributedProcessing_Lab3.Benchmarks
{
    public class CoefficientsBenchmarks
    {
        private Input _data;
        [GlobalSetup]
        public void PrepareData()
        {
            var manager = new DataManager();

            _data = manager.GenerateData(500, 500);
        }

        [Benchmark]
        public void ActionBlockMatrix_OneThird_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new ActionBlockMultiThreadMatrixCalculator(size => 1 + size / 3);
            var calculator = new MultithreadEquationCalulator(matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void ActionBlockMatrix_OneThird_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new ActionBlockMultiThreadMatrixCalculator(size => 1 + size / 3);
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void ActionBlockMatrix_OneFifth_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new ActionBlockMultiThreadMatrixCalculator(size => 1 + size / 5);
            var calculator = new MultithreadEquationCalulator(matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void ActionBlockMatrix_OneFifth_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new ActionBlockMultiThreadMatrixCalculator(size => 1 + size / 5);
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void ActionBlockMatrix_OneSeventh_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new ActionBlockMultiThreadMatrixCalculator(size => 1 + size / 7);
            var calculator = new MultithreadEquationCalulator(matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void ActionBlockMatrix_OneSeventh_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new ActionBlockMultiThreadMatrixCalculator(size => 1 + size / 7);
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void ActionBlockMatrix_OneNinth_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new ActionBlockMultiThreadMatrixCalculator(size => 1 + size / 9);
            var calculator = new MultithreadEquationCalulator(matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void ActionBlockMatrix_OneNinth_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new ActionBlockMultiThreadMatrixCalculator(size => 1 + size / 9);
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
    }
}
