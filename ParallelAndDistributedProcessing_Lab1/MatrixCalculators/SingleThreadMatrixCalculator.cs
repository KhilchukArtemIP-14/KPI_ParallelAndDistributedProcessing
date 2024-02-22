using ParallelAndDistributedCalculations_Lab1.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedCalculations_Lab1.MatrixCalculators
{
    internal class SingleThreadMatrixCalculator : IMatrixCalculator
    {
        public Matrix<decimal> Add(Matrix<decimal> matrixA, Matrix<decimal> matrixB)
        {
            if (matrixA.Values.GetLength(0) != matrixB.Values.GetLength(0) ||
                matrixA.Values.GetLength(1) != matrixB.Values.GetLength(1))
            {
                throw new ArgumentException("Matrix dimensions must match for addition.");
            }

            decimal[,] resultValues = new decimal[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                {
                    resultValues[i, j] = matrixA.Values[i, j] + matrixB.Values[i, j];
                }
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

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                for (int j = 0; j < matrixB.Values.GetLength(1); j++)
                {
                    var nums = new List<decimal>();
                    for (int k = 0; k < matrixA.Values.GetLength(1); k++)
                    {
                        nums.Add(matrixA.Values[i, k] * matrixB.Values[k, j]);
                    }
                    resultValues[i, j]=KahanSum(nums);
                }
            }

            return new Matrix<decimal>(resultValues);
        }

        public Matrix<decimal> MultiplyByScalar(Matrix<decimal> matrixA, decimal scalar)
        {
            decimal[,] resultValues = new decimal[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                {
                    resultValues[i, j] = matrixA.Values[i, j] * scalar;
                }
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

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                {
                    resultValues[i, j] = matrixA.Values[i, j] - matrixB.Values[i, j];
                }
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
