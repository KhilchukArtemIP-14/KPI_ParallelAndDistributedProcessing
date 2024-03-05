using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab2.EquationCalculators.MultiThreadOutputStrategies
{
    public interface IMultiThreadOutput
    {
        public void OutputMessage(string path, string output);
    }
}
