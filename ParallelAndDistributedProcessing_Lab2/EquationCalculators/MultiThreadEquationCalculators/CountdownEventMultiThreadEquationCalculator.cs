using ParallelAndDistributedCalculations_Lab2.Matrices;
using ParallelAndDistributedCalculations_Lab2.MatrixCalculators;
using ParallelAndDistributedCalculations_Lab2.Models;
using ParallelAndDistributedProcessing_Lab2.EquationCalculators.MultiThreadOutputStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab2.EquationCalculators.MultiThreadEquationCalculators
{
    internal class CountdownEventMultiThreadEquationCalculator
    {
        private IMultiThreadOutput _outputStrategy;
        private IMatrixCalculator _calculator;

        public CountdownEventMultiThreadEquationCalculator(IMultiThreadOutput outputStrategy, IMatrixCalculator calculator)
        {
            _outputStrategy = outputStrategy;
            _calculator = calculator;
        }

        public void Calculate(Input input, string outputFilePath)
        {
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            Matrix<double> APlusC = null;
            double maxAPlusC = 0f;
            Matrix<double> maxAPlusCxMB = null;
            Matrix<double> maxAPlusCxMBxMT = null;
            Matrix<double> MZxME = null;
            Matrix<double> MZxMExa = null;
            Matrix<double> MG = null;
            Matrix<double> AxMB = null;
            double minC = 0f;
            Matrix<double> minCxC = null;
            Matrix<double> X;

            CountdownEvent countdownEvent = new CountdownEvent(4);

            Thread maxAPlusCxMBxMTThread = new Thread(() =>
            {
                try
                {
                    APlusC = _calculator.Add(input.A, input.C);
                    OutputMessage(outputFilePath, $"A + C =\n{APlusC}\n");
                    maxAPlusC = APlusC.Max;
                    OutputMessage(outputFilePath, $"max(A + C) =\n{maxAPlusC}\n");

                    maxAPlusCxMB = _calculator.MultiplyByScalar(input.MB, maxAPlusC);
                    OutputMessage(outputFilePath, $"max(A + C) * MB =\n{maxAPlusCxMB}\n");

                    maxAPlusCxMBxMT = _calculator.MultiplyByMatrix(maxAPlusCxMB, input.MT);
                    OutputMessage(outputFilePath, $"max(A + C) * MB * MT =\n{maxAPlusCxMBxMT}\n");
                }
                finally
                {
                    countdownEvent.Signal();
                }
            });

            Thread MZxMExaThread = new Thread(() =>
            {
                try
                {
                    MZxME = _calculator.MultiplyByMatrix(input.MZ, input.ME);
                    OutputMessage(outputFilePath, $"MZ * ME =\n{MZxME}\n");

                    MZxMExa = _calculator.MultiplyByScalar(MZxME, input.a);
                    OutputMessage(outputFilePath, $"MZ * ME * a =\n{MZxMExa}\n");
                }
                finally
                {
                    countdownEvent.Signal();
                }
            });

            Thread AxMBThread = new Thread(() =>
            {
                try
                {
                    AxMB = _calculator.MultiplyByMatrix(input.A, input.MB);
                    OutputMessage(outputFilePath, $"A * MB =\n{AxMB}\n");
                }
                finally
                {
                    countdownEvent.Signal(); 
                }
            });

            Thread minCxCThread = new Thread(() =>
            {
                try
                {
                    minC = input.C.Max;
                    OutputMessage(outputFilePath, $"min(C) =\n{minC}\n");

                    minCxC = _calculator.MultiplyByScalar(input.C, minC);
                    OutputMessage(outputFilePath, $"min(C) * C =\n{minCxC}\n");
                }
                finally
                {
                    countdownEvent.Signal(); 
                }
            });

            maxAPlusCxMBxMTThread.Start();
            MZxMExaThread.Start();
            AxMBThread.Start();
            minCxCThread.Start();

            countdownEvent.Wait();

            MG = _calculator.Substract(maxAPlusCxMBxMT, MZxMExa);
            OutputMessage(outputFilePath, $"MG =\n{MG}");

            X = _calculator.Substract(AxMB, minCxC);
            OutputMessage(outputFilePath, $"X =\n{X}");
        }

        private void OutputMessage(string path, string output)
        {
            _outputStrategy.OutputMessage(path, output);
        }
    }
}
