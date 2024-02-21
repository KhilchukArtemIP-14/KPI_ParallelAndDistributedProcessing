using BenchmarkDotNet.Attributes;
using BenchmarkDotNet;
using ParallelAndDistributedCalculations_Lab1.Data;
using ParallelAndDistributedCalculations_Lab1.MatrixCalculators;
using ParallelAndDistributedCalculations_Lab1.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ParallelAndDistributedCalculations_Lab1.Models;

namespace ParallelAndDistributedCalculations_Lab1.Benchmarks
{
    public class MatrixOperationsBenchmarks
    {
        private Input _data;
        [GlobalSetup]
        public void PrepareData()
        {
            var manager = new DataManager();

            _data = manager.GenerateData(300, 300);
        }
        [Benchmark]
        public Matrix<decimal> MultiThread_MultiplyByMatrix()
        {
            var calculator = new MultiThreadMatrixCalculator();
            var res = calculator.MultiplyByMatrix(_data.MB, _data.MT);
            Console.WriteLine();
            return res;
        }
        [Benchmark]
        public Matrix<decimal> SingleThread_MultiplyByMatrix()
        {
            var calculator = new SingleThreadMatrixCalculator();

            var res = calculator.MultiplyByMatrix(_data.MB, _data.MT);
            return res;
        }

        [Benchmark]
        public Matrix<decimal> MultiThread_MultiplyByScalar()
        {
            var calculator = new MultiThreadMatrixCalculator();
            var manager = new DataManager();
            var res = calculator.MultiplyByScalar(_data.MB, 1.5m);
            return res;
        }
        [Benchmark]
        public Matrix<decimal> SingleThread_MultiplyByScalar()
        {
            var calculator = new SingleThreadMatrixCalculator();

            var res = calculator.MultiplyByScalar(_data.MB, 1.5m);
            return res;
        }
        [Benchmark]
        public Matrix<decimal> MultiThread_Add()
        {
            var calculator = new MultiThreadMatrixCalculator();

            var res = calculator.Add(_data.MB, _data.MT);
            return res;
        }

        [Benchmark]
        public Matrix<decimal> SingleThread_Add()
        {
            var calculator = new SingleThreadMatrixCalculator();

            var res = calculator.Add(_data.MB, _data.MT);
            return res;
        }

        [Benchmark]
        public Matrix<decimal> MultiThread_Substract()
        {
            var calculator = new MultiThreadMatrixCalculator();

            var res = calculator.Substract(_data.MB, _data.MT);
            return res;
        }

        [Benchmark]
        public Matrix<decimal> SingleThread_Substract()
        {
            var calculator = new SingleThreadMatrixCalculator();

            var res = calculator.Substract(_data.MB, _data.MT);
            return res;
        }
    }

}
