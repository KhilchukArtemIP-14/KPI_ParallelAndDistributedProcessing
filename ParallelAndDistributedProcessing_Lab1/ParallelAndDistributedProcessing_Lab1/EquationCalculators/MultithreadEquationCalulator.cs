﻿using ParallelAndDistributedCalculations_Lab1.Matrices;
using ParallelAndDistributedCalculations_Lab1.MatrixCalculators;
using ParallelAndDistributedCalculations_Lab1.Models;
using System.Runtime.CompilerServices;

namespace ParallelAndDistributedCalculations_Lab1.EquationCalculators
{
    public class MultithreadEquationCalulator : IEquationCalulator
    {

        private IMatrixCalculator _calculator;
        public MultithreadEquationCalulator(IMatrixCalculator calculator)
        {
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

            Thread maxAPlusCxMBxMTThread = new Thread(() =>
            {
                APlusC = _calculator.Add(input.A, input.C);
                OutputMessage(outputFilePath, $"A + C =\n{APlusC}\n");
                maxAPlusC = APlusC.Max;
                OutputMessage(outputFilePath, $"max(A + C) =\n{maxAPlusC}\n");

                maxAPlusCxMB = _calculator.MultiplyByScalar(input.MB, maxAPlusC);
                OutputMessage(outputFilePath, $"max(A + C) * MB =\n{maxAPlusCxMB}\n");

                maxAPlusCxMBxMT = _calculator.MultiplyByMatrix(maxAPlusCxMB, input.MT);
                OutputMessage(outputFilePath, $"max(A + C) * MB * MT =\n{maxAPlusCxMBxMT}\n");
            });

            Thread MZxMExaThread = new Thread(() =>
            {
                MZxME = _calculator.MultiplyByMatrix(input.MZ, input.ME);
                OutputMessage(outputFilePath, $"MZ * ME =\n{MZxME}\n");

                MZxMExa = _calculator.MultiplyByScalar(MZxME, input.a);
                OutputMessage(outputFilePath, $"MZ * ME * a =\n{MZxMExa}\n");
            });

            Thread AxMBThread = new Thread(() =>
            {
                AxMB = _calculator.MultiplyByMatrix(input.A, input.MB);
                OutputMessage(outputFilePath, $"A * MB =\n{AxMB}\n");
            });

            Thread minCxCThread = new Thread(() =>
            {
                minC = input.C.Max;
                OutputMessage(outputFilePath, $"min(C) =\n{minC}\n");

                minCxC = _calculator.MultiplyByScalar(input.C, minC);
                OutputMessage(outputFilePath, $"min(C) * C =\n{minCxC}\n");
            });

            maxAPlusCxMBxMTThread.Start();
            MZxMExaThread.Start();
            AxMBThread.Start();
            minCxCThread.Start();

            maxAPlusCxMBxMTThread.Join();
            MZxMExaThread.Join();
            AxMBThread.Join();
            minCxCThread.Join();

            MG = _calculator.Substract(maxAPlusCxMBxMT, MZxMExa);
            OutputMessage(outputFilePath, $"MG =\n{MG}");

            X = _calculator.Substract(AxMB, minCxC);
            OutputMessage(outputFilePath, $"X =\n{X}");
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void OutputMessage(string path, string output)
        {
             Console.WriteLine(output);
             using(StreamWriter sw = new StreamWriter(path, true))
             {
                 sw.WriteLine(output);
             }
        }
    }
}
