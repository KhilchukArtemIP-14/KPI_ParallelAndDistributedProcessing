﻿using BenchmarkDotNet.Attributes;
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
    public class MatrixOperationsBenchmarksBenchmarks
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
        public Matrix<double> ForkJoin_MultiplyByMatrix()
        {
            var calculator = new ForkJoinMatrixCalculator(150);

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
        public Matrix<double> ForkJoin_MultiplyByScalar()
        {
            var calculator = new ForkJoinMatrixCalculator(150);

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
        public Matrix<double> ForkJoin_Add()
        {
            var calculator = new ForkJoinMatrixCalculator(150);

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
        public Matrix<double> ForkJoin_Substract()
        {
            var calculator = new ForkJoinMatrixCalculator(150);

            var res = calculator.Substract(_data.MB, _data.MT);
            return res;
        }
    }

}
