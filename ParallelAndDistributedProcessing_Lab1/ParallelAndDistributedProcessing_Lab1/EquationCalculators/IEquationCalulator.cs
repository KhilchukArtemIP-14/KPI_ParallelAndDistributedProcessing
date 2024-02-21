using ParallelAndDistributedCalculations_Lab1.Models;

namespace ParallelAndDistributedCalculations_Lab1.EquationCalculators
{
    public interface IEquationCalulator
    {
        void Calculate(Input input, string outputFilePath);
    }
}