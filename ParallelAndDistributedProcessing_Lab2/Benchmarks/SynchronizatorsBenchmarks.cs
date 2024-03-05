using BenchmarkDotNet.Attributes;
using ParallelAndDistributedCalculations_Lab2.Data;
using ParallelAndDistributedCalculations_Lab2.Matrices;
using ParallelAndDistributedCalculations_Lab2.MatrixCalculators;
using ParallelAndDistributedCalculations_Lab2.Models;
using ParallelAndDistributedProcessing_Lab2.EquationCalculators.MultiThreadCalculators;
using ParallelAndDistributedProcessing_Lab2.EquationCalculators.MultiThreadEquationCalculators;
using ParallelAndDistributedProcessing_Lab2.EquationCalculators.MultiThreadOutputStrategies;
using ParallelAndDistributedProcessing_Lab2.MatrixCalculators.MultiThreadMatrixCalculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab2.Benchmarks
{
    public class SynchronizatorsBenchmarks
    {
        private Input _data;
        [GlobalSetup]
        public void PrepareData()
        {
            var manager = new DataManager();

            _data = manager.GenerateData(300, 300);
        }
        [Benchmark]
        public void JoinMatrix_JoinEquation()
        {
            var matrixCalculator = new JoinMultiThreadMatrixCalculator();
            var outputStrategy = new SemaphoreMultiThreadOutput();
            var calculator = new JoinMultiThreadEquationCalculator(outputStrategy, matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void JoinMatrix_BarrierEquation()
        {
            var matrixCalculator = new JoinMultiThreadMatrixCalculator();
            var outputStrategy = new SemaphoreMultiThreadOutput();
            var calculator = new BarrierMultiThreadEquationCalculator(outputStrategy, matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void JoinMatrix_CountdownEquation()
        {
            var matrixCalculator = new JoinMultiThreadMatrixCalculator();
            var outputStrategy = new SemaphoreMultiThreadOutput();
            var calculator = new CountdownEventMultiThreadEquationCalculator(outputStrategy, matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void BarrierMatrix_JoinEquation()
        {
            var matrixCalculator = new BarrierMultiThreadMatrixCalculator();
            var outputStrategy = new SemaphoreMultiThreadOutput();
            var calculator = new JoinMultiThreadEquationCalculator(outputStrategy, matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void BarrierMatrix_BarrierEquation()
        {
            var matrixCalculator = new BarrierMultiThreadMatrixCalculator();
            var outputStrategy = new SemaphoreMultiThreadOutput();
            var calculator = new BarrierMultiThreadEquationCalculator(outputStrategy, matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void BarrierMatrix_CountdownEquation()
        {
            var matrixCalculator = new BarrierMultiThreadMatrixCalculator();
            var outputStrategy = new SemaphoreMultiThreadOutput();
            var calculator = new CountdownEventMultiThreadEquationCalculator(outputStrategy, matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void CountdownMatrix_JoinEquation()
        {
            var matrixCalculator = new CountdownEventMultiThreadMatrixCalculator();
            var outputStrategy = new SemaphoreMultiThreadOutput();
            var calculator = new JoinMultiThreadEquationCalculator(outputStrategy, matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void CountdownMatrix_BarrierEquation()
        {
            var matrixCalculator = new CountdownEventMultiThreadMatrixCalculator();
            var outputStrategy = new SemaphoreMultiThreadOutput();
            var calculator = new BarrierMultiThreadEquationCalculator(outputStrategy, matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
        [Benchmark]
        public void CountdownMatrix_CountdownEquation()
        {
            var matrixCalculator = new CountdownEventMultiThreadMatrixCalculator();
            var outputStrategy = new SemaphoreMultiThreadOutput();
            var calculator = new CountdownEventMultiThreadEquationCalculator(outputStrategy, matrixCalculator);

            calculator.Calculate(_data, "yeah.txt");
        }
    }
}
