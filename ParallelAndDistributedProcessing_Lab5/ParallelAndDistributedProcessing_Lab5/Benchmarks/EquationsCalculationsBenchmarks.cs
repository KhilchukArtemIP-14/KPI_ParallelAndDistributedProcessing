using BenchmarkDotNet.Attributes;
using ParallelAndDistributedProcessing_Lab4.Data;
using ParallelAndDistributedProcessing_Lab4.EquationCalculators;
using ParallelAndDistributedProcessing_Lab4.MatrixCalculators;
using ParallelAndDistributedProcessing_Lab4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab4 .Benchmarks
{
    public class EquationsCalculationsBenchmarks
    {
        private Input _data;

        [GlobalSetup]
        public void PrepareData()
        {
            var manager = new DataManager();

            _data = manager.GenerateData(1000, 1000);
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
        public void MultiThreadMatrix_ForkJoinEquations_Calculate()
        {
            var matrixCalculator = new MultiThreadMatrixCalculator();
            var calculator = new ForkJoinEquationCalculator(matrixCalculator);


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
        public void SingleThreadMatrix_ForkJoinEquations_Calculate()
        {
            var matrixCalculator = new SingleThreadMatrixCalculator();
            var calculator = new ForkJoinEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }

        [Benchmark]
        public void ForkJoinMatrix_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new ForkJoinMatrixCalculator(150);
            var calculator = new MultithreadEquationCalulator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void ForkJoinMatrix_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new ForkJoinMatrixCalculator(150);
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void ForkJoinMatrix_ForkJoinEquations_Calculate()
        {
            var matrixCalculator = new ForkJoinMatrixCalculator(150);
            var calculator = new ForkJoinEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
    }
}
