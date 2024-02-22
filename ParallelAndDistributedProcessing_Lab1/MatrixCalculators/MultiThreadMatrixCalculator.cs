using ParallelAndDistributedCalculations_Lab1.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedCalculations_Lab1.MatrixCalculators
{
    public class MultiThreadMatrixCalculator : IMatrixCalculator
    {
        public Matrix<decimal> Add(Matrix<decimal> matrixA, Matrix<decimal> matrixB)
        {
            if (matrixA.Values.GetLength(0) != matrixB.Values.GetLength(0) ||
                matrixA.Values.GetLength(1) != matrixB.Values.GetLength(1))
            {
                throw new ArgumentException("Matrix dimensions must match for addition.");
            }

            decimal[,] resultValues = new decimal[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];

            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                int row = i;
                Thread thread = new Thread(() =>
                {
                    for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                    {
                        resultValues[row, j] = matrixA.Values[row, j] + matrixB.Values[row, j];
                    }
                });
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return new Matrix<decimal>(resultValues);
        }

        public Matrix<decimal> MultiplyByMatrix(Matrix<decimal> matrixA, Matrix<decimal> matrixB)
        {
            if (matrixA.Values.GetLength(1) != matrixB.Values.GetLength(0))
            {
                throw new ArgumentException("Matrix dimensions must match for matrix multiplication.");
            }

            decimal[,] resultValues = new decimal[matrixA.Values.GetLength(0), matrixB.Values.GetLength(1)];

            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                int row = i;
                Thread thread = new Thread(() =>
                {
                    for (int j = 0; j < matrixB.Values.GetLength(1); j++)
                    {
                        var nums = new List<decimal>();
                        for (int k = 0; k < matrixA.Values.GetLength(1); k++)
                        {
                            nums.Add(matrixA.Values[row, k] * matrixB.Values[k, j]);
                        }
                        resultValues[row, j] = KahanSum(nums);
                    }
                });
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return new Matrix<decimal>(resultValues);
        }

        public Matrix<decimal> MultiplyByScalar(Matrix<decimal> matrixA, decimal scalar)
        {
            decimal[,] resultValues = new decimal[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];

            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                int row = i;
                Thread thread = new Thread(() =>
                {
                    for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                    {
                        resultValues[row, j] = matrixA.Values[row, j] * scalar;
                    }
                });
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return new Matrix<decimal>(resultValues);
        }

        public Matrix<decimal> Substract(Matrix<decimal> matrixA, Matrix<decimal> matrixB)
        {
            if (matrixA.Values.GetLength(0) != matrixB.Values.GetLength(0) ||
                matrixA.Values.GetLength(1) != matrixB.Values.GetLength(1))
            {
                throw new ArgumentException("Matrix dimensions must match for subtraction.");
            }

            decimal[,] resultValues = new decimal[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];

            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                int row = i;
                Thread thread = new Thread(() =>
                {
                    for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                    {
                        resultValues[row, j] = matrixA.Values[row, j] - matrixB.Values[row, j];
                    }
                });
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return new Matrix<decimal>(resultValues);
        }

        private decimal KahanSum(IEnumerable<decimal> sequence)
        {
            var sum = 0m;
            var compensation = 0m;

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
