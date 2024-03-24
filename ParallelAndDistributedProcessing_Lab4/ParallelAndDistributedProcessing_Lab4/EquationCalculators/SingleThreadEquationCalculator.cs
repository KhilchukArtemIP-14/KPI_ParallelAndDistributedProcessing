using ParallelAndDistributedProcessing_Lab4.MatrixCalculators;
using ParallelAndDistributedProcessing_Lab4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ParallelAndDistributedProcessing_Lab4.Matrices;

namespace ParallelAndDistributedProcessing_Lab4.EquationCalculators
{
    public class SingleThreadEquationCalculator: IEquationCalulator
    {
        private IMatrixCalculator _calculator;
        public SingleThreadEquationCalculator(IMatrixCalculator calculator) 
        {
            _calculator = calculator;
        }
        public void Calculate(Input input, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                var APlusC = _calculator.Add(input.A, input.C);
                //Console.WriteLine("A + C =\n{0}", APlusC);
                //writer.WriteLine(string.Format("A + C =\n{0}\n", APlusC.ToString()));

                var maxAPlusC = APlusC.Max;
                //Console.WriteLine("max(A + C) =\n{0}", maxAPlusC);
                //writer.WriteLine(string.Format("max(A + C) =\n{0}\n", maxAPlusC.ToString()));

                var maxAPlusCxMB = _calculator.MultiplyByScalar(input.MB, maxAPlusC);
                //Console.WriteLine("max(A + C)*MB =\n{0}", maxAPlusCxMB);
                //writer.WriteLine(string.Format("max(A + C)*MB =\n{0}\n", maxAPlusCxMB.ToString()));

                var maxAPlusCxMBxMT = _calculator.MultiplyByMatrix(maxAPlusCxMB, input.MT);
                //Console.WriteLine("max(A + C) * MB * MT =\n{0}\n", maxAPlusCxMBxMT);
                //writer.WriteLine(string.Format("max(A + C) * MB * MT =\n{0}\n", maxAPlusCxMBxMT.ToString()));


                var MZxME = _calculator.MultiplyByMatrix(input.MZ, input.ME);
                //Console.WriteLine("MZ * ME =\n{0}", MZxME);
                //writer.WriteLine(string.Format("MZ * ME =\n{0}\n", MZxME.ToString()));

                var MZxMExa = _calculator.MultiplyByScalar(MZxME, input.a);
                //Console.WriteLine("MZ * ME * a =\n{0}", MZxMExa);
                //writer.WriteLine(string.Format("MZ * ME * a =\n{0}\n", MZxMExa.ToString()));

                var MG = _calculator.Substract(maxAPlusCxMBxMT, MZxMExa);
                //Console.WriteLine("MG =\n{0}", MG);
                //writer.WriteLine(string.Format("MG =\n{0}\n", MG.ToString()));


                var AxMB = _calculator.MultiplyByMatrix(input.A, input.MB);
                //Console.WriteLine("A * MB =\n{0}", AxMB);
                //writer.WriteLine(string.Format("A * MB =\n{0}\n", AxMB.ToString()));

                var minC = input.C.Max;
                //Console.WriteLine("min(C) =\n{0}", minC);
                //writer.WriteLine(string.Format("min(C) =\n{0}\n", minC.ToString()));

                var minCxC = _calculator.MultiplyByScalar(input.C, minC);
                //Console.WriteLine("min(C) * C =\n{0}", minCxC);
                //writer.WriteLine(string.Format("min(C) * C =\n{0}\n", minCxC.ToString()));

                var X = _calculator.Substract(AxMB, minCxC);
                //Console.WriteLine("X =\n{0}", X);
                //writer.WriteLine(string.Format("X =\n{0}\n", X.ToString()));

            }
        }
    }
}
