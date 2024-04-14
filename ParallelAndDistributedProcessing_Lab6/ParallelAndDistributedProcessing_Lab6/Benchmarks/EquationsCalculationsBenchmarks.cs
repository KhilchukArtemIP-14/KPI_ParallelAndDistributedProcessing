using BenchmarkDotNet.Attributes;
using ParallelAndDistributedProcessing_Lab6.Data;
using ParallelAndDistributedProcessing_Lab6.EquationCalculators;
using ParallelAndDistributedProcessing_Lab6.MatrixCalculators;
using ParallelAndDistributedProcessing_Lab6.Models;
using ParallelAndDistributedProcessing_Lab6.MatrixCalculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab6.Benchmarks
{
    [IterationCount(5)]
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
        public void MultiThreadMatrix_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new MultiThreadMatrixCalculator();
            var calculator = new MultithreadEquationCalulator(matrixCalculator);


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
        public void SingleThreadMatrix_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new SingleThreadMatrixCalculator();
            var calculator = new MultithreadEquationCalulator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void SingleThreadMatrix_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new SingleThreadMatrixCalculator();
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }


        [Benchmark]
        public void BlockingQueueMatrix_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new BlockingQueueMatrixCalculator();
            var calculator = new MultithreadEquationCalulator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void BlockingQueueMatrix_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new BlockingQueueMatrixCalculator();
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
    }
}
