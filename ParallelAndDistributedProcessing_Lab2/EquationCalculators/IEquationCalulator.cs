using ParallelAndDistributedCalculations_Lab2.Models;

namespace ParallelAndDistributedCalculations_Lab2.EquationCalculators
{
    public interface IEquationCalulator
    {
        void Calculate(Input input, string outputFilePath);
    }
}