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
    [IterationCount(16)]
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
        public void MultiThreadMatrix_TaskEquations_Calculate()
        {
            var matrixCalculator = new MultiThreadMatrixCalculator();
            var calculator = new TaskEquationCalculator(matrixCalculator);


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
        public void SingleThreadMatrix_TaskEquations_Calculate()
        {
            var matrixCalculator = new SingleThreadMatrixCalculator();
            var calculator = new TaskEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }

        [Benchmark]
        public void TaskMatrix_MultiThreadEquations_Calculate()
        {
            var matrixCalculator = new TaskMatrixCalculator();
            var calculator = new MultithreadEquationCalulator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void TaskMatrix_SingleThreadEquations_Calculate()
        {
            var matrixCalculator = new TaskMatrixCalculator();
            var calculator = new SingleThreadEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void TaskMatrix_TaskEquations_Calculate()
        {
            var matrixCalculator = new TaskMatrixCalculator();
            var calculator = new TaskEquationCalculator(matrixCalculator);


            calculator.Calculate(_data, "yeah.txt");
        }
    }
}
