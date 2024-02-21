using ParallelAndDistributedCalculations_Lab1.Matrices;
using ParallelAndDistributedCalculations_Lab1.MatrixCalculators;
using ParallelAndDistributedCalculations_Lab1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedCalculations_Lab1.EquationCalculators
{
    public class MultithreadEquationCalulator : IEquationCalulator
    {
        private static readonly object _lock = new object(); // Define a lock object

        private IMatrixCalculator _calculator;
        public MultithreadEquationCalulator(IMatrixCalculator calculator)
        {
            _calculator = calculator;
        }
        public void Calculate(Input input, string outputFilePath)
        {
            if(File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
            //using (StreamWriter writer = new StreamWriter(outputFilePath))
            //{
                Matrix<decimal> APlusC = null;
                decimal maxAPlusC = 0m;
                Matrix<decimal> maxAPlusCxMB = null;

                Matrix<decimal> maxAPlusCxMBxMT = null;
                Matrix<decimal> MZxME = null;
                Matrix<decimal> MZxMExa = null;
                Matrix<decimal> MG = null;
                Matrix<decimal> AxMB = null;
                decimal minC = 0m;
                Matrix<decimal> minCxC = null;
                Matrix<decimal> X;

                var maxAPlusCxMBxMTJob = Task.Run(() =>
                {
                    APlusC = _calculator.Add(input.A, input.C);
                    //OutputMessage(outputFilePath,$"A + C =\n{APlusC}\n");
                    maxAPlusC = APlusC.Max;
                    //OutputMessage(outputFilePath, $"max(A + C) =\n{maxAPlusC}\n");

                    maxAPlusCxMB = _calculator.MultiplyByScalar(input.MB, maxAPlusC);
                    //OutputMessage(outputFilePath, $"max(A + C) * MB =\n{maxAPlusCxMB}\n");

                    maxAPlusCxMBxMT = _calculator.MultiplyByMatrix(maxAPlusCxMB, input.MT);
                    //OutputMessage(outputFilePath,$"max(A + C) * MB * MT =\n{maxAPlusCxMBxMT}\n");

                });

                var MZxMExaJob = Task.Run(() =>
                {
                    MZxME = _calculator.MultiplyByMatrix(input.MZ, input.ME);
                    //OutputMessage(outputFilePath, $"MZ * ME =\n{MZxME}\n");


                    MZxMExa = _calculator.MultiplyByScalar(MZxME, input.a);
                    //OutputMessage(outputFilePath, $"MZ * ME * a =\n{MZxMExa}\n");
                });

                var AxMBJob = Task.Run(() =>
                {
                    AxMB = _calculator.MultiplyByMatrix(input.A, input.MB);
                    //OutputMessage(outputFilePath, $"A * MB =\n{AxMB}\n");
                });

                var minCxCJob = Task.Run(() =>
                {
                    minC = input.C.Max;
                    //OutputMessage(outputFilePath, $"min(C) =\n{minC}\n");

                    minCxC = _calculator.MultiplyByScalar(input.C, minC);
                    //OutputMessage(outputFilePath, $"min(C) * C =\n{minCxC}\n");
                });

                Task.WaitAll(maxAPlusCxMBxMTJob, MZxMExaJob, AxMBJob, minCxCJob);

                MG = _calculator.Substract(maxAPlusCxMBxMT, MZxMExa);
                //OutputMessage(outputFilePath, $"MG =\n{MG}");
                //Console.WriteLine($"MG =\n{MG}\n");

                X = _calculator.Substract(AxMB, minCxC);
                //OutputMessage(outputFilePath, $"X =\n{X}");
                //Console.WriteLine($"X =\n{X}\n");
            //}
        }
        private void OutputMessage(string path, string output)
        {
            lock (_lock)
            {
                Console.WriteLine(output);
                using(StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(output);
                }
            }
        }
    }
}
