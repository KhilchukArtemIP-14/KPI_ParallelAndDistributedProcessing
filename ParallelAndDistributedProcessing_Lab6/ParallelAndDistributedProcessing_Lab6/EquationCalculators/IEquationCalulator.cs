using ParallelAndDistributedProcessing_Lab6.Models;

namespace ParallelAndDistributedProcessing_Lab6.EquationCalculators
{
    public interface IEquationCalulator
    {
        void Calculate(Input input, string outputFilePath);
    }
}