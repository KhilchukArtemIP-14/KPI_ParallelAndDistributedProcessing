using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab2.EquationCalculators.MultiThreadOutputStrategies
{
    public class SemaphoreMultiThreadOutput:IMultiThreadOutput
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public void OutputMessage(string path, string output)
        {
            _semaphore.Wait();

            try
            {
                Console.WriteLine(output);
                using(StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(output);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
