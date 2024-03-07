using ParallelAndDistributedCalculations_Lab1.Matrices;
using ParallelAndDistributedCalculations_Lab1.MatrixCalculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ParallelAndDistributedProcessing_Lab3.MatrixCalculators
{
    public class ActionBlockMultiThreadMatrixCalculator:IMatrixCalculator
    {
        private Func<int, int> _getDegreeOfParallelism;
        public ActionBlockMultiThreadMatrixCalculator(Func<int, int> getDegreeOfParallelism)
        {
            _getDegreeOfParallelism = getDegreeOfParallelism;
        }

        public Matrix<double> Add(Matrix<double> matrixA, Matrix<double> matrixB)
        {
            if (matrixA.Values.GetLength(0) != matrixB.Values.GetLength(0) ||
                matrixA.Values.GetLength(1) != matrixB.Values.GetLength(1))
            {
                throw new ArgumentException("Matrix dimensions must match for addition.");
            }

            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];
            var _maxDegreeOfParallelism = _getDegreeOfParallelism(matrixA.Values.GetLength(1));

            var actionBlock = new ActionBlock<int>(
                row =>
                {
                    for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                    {
                        resultValues[row, j] = matrixA.Values[row, j] + matrixB.Values[row, j];
                    }
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _maxDegreeOfParallelism }
            );

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                actionBlock.Post(i);
            }
            
            actionBlock.Complete();
            actionBlock.Completion.Wait();

            return new Matrix<double>(resultValues);
        }

        public Matrix<double> MultiplyByMatrix(Matrix<double> matrixA, Matrix<double> matrixB)
        {
            if (matrixA.Values.GetLength(1) != matrixB.Values.GetLength(0))
            {
                throw new ArgumentException("Matrix dimensions must match for matrix multiplication.");
            }

            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixB.Values.GetLength(1)];
            var _maxDegreeOfParallelism = _getDegreeOfParallelism(matrixA.Values.GetLength(1));

            var actionBlock = new ActionBlock<int>(
                row =>
                {
                    for (int j = 0; j < matrixB.Values.GetLength(1); j++)
                    {
                        var nums = new List<double>();
                        for (int k = 0; k < matrixA.Values.GetLength(1); k++)
                        {
                            nums.Add(matrixA.Values[row, k] * matrixB.Values[k, j]);
                        }
                        resultValues[row, j] = KahanSum(nums);
                    }
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _maxDegreeOfParallelism }
            );

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                actionBlock.Post(i);
            }

            actionBlock.Complete();
            actionBlock.Completion.Wait();

            return new Matrix<double>(resultValues);
        }

        public Matrix<double> MultiplyByScalar(Matrix<double> matrixA, double scalar)
        {
            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];
            var _maxDegreeOfParallelism = _getDegreeOfParallelism(matrixA.Values.GetLength(1));

            var actionBlock = new ActionBlock<int>(
                row =>
                {
                    for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                    {
                        resultValues[row, j] = matrixA.Values[row, j] * scalar;
                    }
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _maxDegreeOfParallelism }
            );

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                actionBlock.Post(i);
            }

            actionBlock.Complete();
            actionBlock.Completion.Wait();

            return new Matrix<double>(resultValues);
        }


        public Matrix<double> Substract(Matrix<double> matrixA, Matrix<double> matrixB)
        {
            if (matrixA.Values.GetLength(0) != matrixB.Values.GetLength(0) ||
                matrixA.Values.GetLength(1) != matrixB.Values.GetLength(1))
            {
                throw new ArgumentException("Matrix dimensions must match for subtraction.");
            }

            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];
            var _maxDegreeOfParallelism = _getDegreeOfParallelism(matrixA.Values.GetLength(1));

            var actionBlock = new ActionBlock<int>(
                row =>
                {
                    for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                    {
                        resultValues[row, j] = matrixA.Values[row, j] - matrixB.Values[row, j];
                    }
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _maxDegreeOfParallelism }
            );

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                actionBlock.Post(i);
            }

            actionBlock.Complete();
            actionBlock.Completion.Wait();

            return new Matrix<double>(resultValues);
        }

        private double KahanSum(IEnumerable<double> sequence)
        {
            var sum = 0d;
            var compensation = 0d;

            foreach (var value in sequence)
            {
                var adjustedValue = value - compensation;
                var tempSum = sum + adjustedValue;
                compensation = (tempSum - sum) - adjustedValue;
                sum = tempSum;
            }

            return sum;
        }
    }
}
