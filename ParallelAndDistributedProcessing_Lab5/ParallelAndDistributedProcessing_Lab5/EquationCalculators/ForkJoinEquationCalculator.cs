using ParallelAndDistributedProcessing_Lab4.Matrices;
using ParallelAndDistributedProcessing_Lab4.MatrixCalculators;
using ParallelAndDistributedProcessing_Lab4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab4.EquationCalculators
{
    public class ForkJoinEquationCalculator
    {
        private IMatrixCalculator _calculator;
        private object _outputLock = new object();

        public ForkJoinEquationCalculator(IMatrixCalculator calculator)
        {
            _calculator = calculator;
        }

        public void Calculate(Input input, string outputFilePath)
        {
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            Task<Matrix<double>> maxAPlusCxMBxMTTask = Task.Run(() =>
            {
                Matrix<double> APlusC = _calculator.Add(input.A, input.C);
                OutputMessage(outputFilePath, $"A + C =\n{APlusC}\n");

                double maxAPlusC = APlusC.Max;
                OutputMessage(outputFilePath, $"max(A + C) =\n{maxAPlusC}\n");

                Matrix<double> maxAPlusCxMB = _calculator.MultiplyByScalar(input.MB, maxAPlusC);
                OutputMessage(outputFilePath, $"max(A + C) * MB =\n{maxAPlusCxMB}\n");

                return _calculator.MultiplyByMatrix(maxAPlusCxMB, input.MT);
            });

            Task<Matrix<double>> MZxMExaTask = Task.Run(() =>
            {
                Matrix<double> MZxME = _calculator.MultiplyByMatrix(input.MZ, input.ME);
                OutputMessage(outputFilePath, $"MZ * ME =\n{MZxME}\n");

                return _calculator.MultiplyByScalar(MZxME, input.a);
            });

            Task<Matrix<double>> AxMBTask = Task.Run(() =>
            {
                Matrix<double> AxMB = _calculator.MultiplyByMatrix(input.A, input.MB);
                OutputMessage(outputFilePath, $"A * MB =\n{AxMB}\n");
                return AxMB;
            });

            Task<Matrix<double>> minCxCTask = Task.Run(() =>
            {
                double minC = input.C.Max;
                OutputMessage(outputFilePath, $"min(C) =\n{minC}\n");

                Matrix<double> minCxC = _calculator.MultiplyByScalar(input.C, minC);
                OutputMessage(outputFilePath, $"min(C) * C =\n{minCxC}\n");

                return minCxC;
            });

            Task.WaitAll(maxAPlusCxMBxMTTask, MZxMExaTask, AxMBTask, minCxCTask);

            Matrix<double> maxAPlusCxMBxMT = maxAPlusCxMBxMTTask.Result;
            Matrix<double> MZxMExa = MZxMExaTask.Result;
            Matrix<double> AxMB = AxMBTask.Result;
            Matrix<double> minCxC = minCxCTask.Result;

            Matrix<double> MG = _calculator.Substract(maxAPlusCxMBxMT, MZxMExa);
            OutputMessage(outputFilePath, $"MG =\n{MG}");

            Matrix<double> X = _calculator.Substract(AxMB, minCxC);
            OutputMessage(outputFilePath, $"X =\n{X}");
        }

        private void OutputMessage(string path, string output)
        {
            lock (_outputLock)
            {
                Console.WriteLine(output);
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(output);
                }
            }
        }
    }
}
