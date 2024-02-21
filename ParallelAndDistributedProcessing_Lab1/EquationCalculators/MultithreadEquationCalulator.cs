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
        private IMatrixCalculator _calculator;
        public MultithreadEquationCalulator(IMatrixCalculator calculator)
        {
            _calculator = calculator;
        }
        public void Calculate(Input input, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
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
                    maxAPlusC = APlusC.Max;
                    maxAPlusCxMB = _calculator.MultiplyByScalar(input.MB, maxAPlusC);
                    maxAPlusCxMBxMT = _calculator.MultiplyByMatrix(maxAPlusCxMB, input.MT);
                });

                var MZxMExaJob = Task.Run(() =>
                {
                    MZxME = _calculator.MultiplyByMatrix(input.MZ, input.ME);
                    MZxMExa = _calculator.MultiplyByScalar(MZxME, input.a);
                });

                var AxMBJob = Task.Run(() =>
                {
                    AxMB = _calculator.MultiplyByMatrix(input.A, input.MB);
                });

                var minCxCJob = Task.Run(() =>
                {
                    minC = input.C.Max;
                    minCxC = _calculator.MultiplyByScalar(input.C, minC);
                });

                Task.WaitAll(maxAPlusCxMBxMTJob, MZxMExaJob, AxMBJob, minCxCJob);

                MG = _calculator.Substract(maxAPlusCxMBxMT, MZxMExa);
                X = _calculator.Substract(AxMB, minCxC);

                writer.WriteLine($"A + C =\n{APlusC}\n");
                writer.WriteLine($"max(A + C) =\n{maxAPlusC}\n");
                writer.WriteLine($"max(A + C)*MB =\n{maxAPlusCxMB}\n");
                writer.WriteLine($"max(A + C) * MB * MT =\n{maxAPlusCxMBxMT}\n");
                writer.WriteLine($"MZ * ME =\n{MZxME}\n");
                writer.WriteLine($"MZ * ME * a =\n{MZxMExa}\n");
                writer.WriteLine($"MG =\n{MG}\n");
                writer.WriteLine($"A * MB =\n{AxMB}\n");
                writer.WriteLine($"min(C) =\n{minC}\n");
                writer.WriteLine($"min(C) * C =\n{minCxC}\n");
                writer.WriteLine($"X =\n{X}\n");

                Console.WriteLine($"A + C =\n{APlusC}");
                Console.WriteLine($"max(A + C) =\n{maxAPlusC}");
                Console.WriteLine($"max(A + C)*MB =\n{maxAPlusCxMB}");
                Console.WriteLine($"max(A + C) * MB * MT =\n{maxAPlusCxMBxMT}");
                Console.WriteLine($"MZ * ME =\n{MZxME}");
                Console.WriteLine($"MZ * ME * a =\n{MZxMExa}");
                Console.WriteLine($"MG =\n{MG}");
                Console.WriteLine($"A * MB =\n{AxMB}");
                Console.WriteLine($"min(C) =\n{minC}");
                Console.WriteLine($"min(C) * C =\n{minCxC}");
                Console.WriteLine($"X =\n{X}");
            }

        }
    }
}
