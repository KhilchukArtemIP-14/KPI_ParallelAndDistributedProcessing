using BenchmarkDotNet.Attributes;
using ParallelAndDistributedProcessing_Lab4.Data;
using ParallelAndDistributedProcessing_Lab4.Matrices;
using ParallelAndDistributedProcessing_Lab4.MatrixCalculators;
using ParallelAndDistributedProcessing_Lab4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab5.Benchmarks
{
    public class ForkJoinThresholdBenchmarks
    {
        public IEnumerable<object[]> Data()
        {
            var manager = new DataManager();

            yield return new object[] { manager.GenerateData(100, 100), 100 };
            yield return new object[] { manager.GenerateData(150, 150), 150 };
            yield return new object[] { manager.GenerateData(200, 200), 200 };
            yield return new object[] { manager.GenerateData(250, 250), 250 };
            yield return new object[] { manager.GenerateData(300, 300), 300 };
            yield return new object[] { manager.GenerateData(350, 350), 350 };
            yield return new object[] { manager.GenerateData(400, 400), 400 };
            yield return new object[] { manager.GenerateData(450, 450), 450 };

        }
        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public Matrix<double> ForkJoin_MultiplyByMatrix(Input data, int size)
        {
            var calculator = new ForkJoinMatrixCalculator(int.MaxValue);
            var res = calculator.MultiplyByMatrix(data.MB, data.MT);

            return res;
        }
        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public Matrix<double> SimpleSingleThread_MultiplyByMatrix(Input data, int size)
        {
            var calculator = new SingleThreadMatrixCalculator();
            var res = calculator.MultiplyByMatrix(data.MB, data.MT);

            return res;
        }
    }
}
