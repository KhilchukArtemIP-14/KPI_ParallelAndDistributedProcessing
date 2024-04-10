using ParallelAndDistributedProcessing_Lab4.Models;

namespace ParallelAndDistributedProcessing_Lab4.EquationCalculators
{
    public interface IEquationCalulator
    {
        void Calculate(Input input, string outputFilePath);
    }
}