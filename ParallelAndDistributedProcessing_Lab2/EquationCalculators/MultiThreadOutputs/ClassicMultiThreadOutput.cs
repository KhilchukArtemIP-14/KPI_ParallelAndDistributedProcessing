using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab2.EquationCalculators.MultiThreadOutputStrategies
{
    internal class ClassicMultiThreadOutput : IMultiThreadOutput
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void OutputMessage(string path, string output)
        {
            Console.WriteLine(output);
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(output);
            }
        }
    }
}
