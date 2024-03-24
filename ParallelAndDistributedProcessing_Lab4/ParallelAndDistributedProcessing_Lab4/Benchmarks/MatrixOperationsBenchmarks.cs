using BenchmarkDotNet.Attributes;
using BenchmarkDotNet;
using ParallelAndDistributedProcessing_Lab4.Data;
using ParallelAndDistributedProcessing_Lab4.MatrixCalculators;
using ParallelAndDistributedProcessing_Lab4.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ParallelAndDistributedProcessing_Lab4.Models;

namespace ParallelAndDistributedProcessing_Lab4.Benchmarks
{
    [IterationCount(16)]
    public class MatrixOperationsBenchmarks
    {
        private Input _data;
        [GlobalSetup]
        public void PrepareData()
        {
            var manager = new DataManager();

            _data = manager.GenerateData(500, 500);
        }
        [Benchmark]
        public Matrix<double> MultiThread_MultiplyByMatrix()
        {
            var calculator = new MultiThreadMatrixCalculator();
            var res = calculator.MultiplyByMatrix(_data.MB, _data.MT);

            return res;
        }
        [Benchmark]
        public Matrix<double> SingleThread_MultiplyByMatrix()
        {
            var calculator = new SingleThreadMatrixCalculator();

            var res = calculator.MultiplyByMatrix(_data.MB, _data.MT);
            return res;
        }
        [Benchmark]
        public Matrix<double> Task_MultiplyByMatrix()
        {
            var calculator = new TaskMatrixCalculator();

            var res = calculator.MultiplyByMatrix(_data.MB, _data.MT);
            return res;
        }

        [Benchmark]
        public Matrix<double> MultiThread_MultiplyByScalar()
        {
            var calculator = new MultiThreadMatrixCalculator();

            var res = calculator.MultiplyByScalar(_data.MB, 1.5f);
            return res;
        }
        [Benchmark]
        public Matrix<double> SingleThread_MultiplyByScalar()
        {
            var calculator = new SingleThreadMatrixCalculator();

            var res = calculator.MultiplyByScalar(_data.MB, 1.5f);
            return res;
        }
        [Benchmark]
        public Matrix<double> Task_MultiplyByScalar()
        {
            var calculator = new TaskMatrixCalculator();

            var res = calculator.MultiplyByScalar(_data.MB, 1.5f);
            return res;
        }

        [Benchmark]
        public Matrix<double> MultiThread_Add()
        {
            var calculator = new MultiThreadMatrixCalculator();

            var res = calculator.Add(_data.MB, _data.MT);
            return res;
        }

        [Benchmark]
        public Matrix<double> SingleThread_Add()
        {
            var calculator = new SingleThreadMatrixCalculator();

            var res = calculator.Add(_data.MB, _data.MT);
            return res;
        }
        [Benchmark]
        public Matrix<double> Task_Add()
        {
            var calculator = new TaskMatrixCalculator();

            var res = calculator.Add(_data.MB, _data.MT);
            return res;
        }


        [Benchmark]
        public Matrix<double> MultiThread_Substract()
        {
            var calculator = new MultiThreadMatrixCalculator();

            var res = calculator.Substract(_data.MB, _data.MT);
            return res;
        }

        [Benchmark]
        public Matrix<double> SingleThread_Substract()
        {
            var calculator = new SingleThreadMatrixCalculator();

            var res = calculator.Substract(_data.MB, _data.MT);
            return res;
        }
        [Benchmark]
        public Matrix<double> Task_Substract()
        {
            var calculator = new TaskMatrixCalculator();

            var res = calculator.Substract(_data.MB, _data.MT);
            return res;
        }
    }

}
