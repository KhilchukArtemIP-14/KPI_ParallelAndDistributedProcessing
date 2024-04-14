using ParallelAndDistributedProcessing_Lab6.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab6.MatrixCalculators
{
    internal class SingleThreadMatrixCalculator : IMatrixCalculator
    {
        public Matrix<double> Add(Matrix<double> matrixA, Matrix<double> matrixB)
        {
            if (matrixA.Values.GetLength(0) != matrixB.Values.GetLength(0) ||
                matrixA.Values.GetLength(1) != matrixB.Values.GetLength(1))
            {
                throw new ArgumentException("Matrix dimensions must match for addition.");
            }

            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                {
                    resultValues[i, j] = matrixA.Values[i, j] + matrixB.Values[i, j];
                }
            }

            return new Matrix<double>(resultValues);
        }

        public Matrix<double> MultiplyByMatrix(Matrix<double> matrixA, Matrix<double> matrixB)
        {
            if (matrixA.Values.GetLength(1) != matrixB.Values.GetLength(0))
            {
                throw new ArgumentException("Matrix dimensions must match for matrix multiplication.");
            }

            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixB.Values.GetLength(1)];

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                for (int j = 0; j < matrixB.Values.GetLength(1); j++)
                {
                    var nums = new List<double>();
                    for (int k = 0; k < matrixA.Values.GetLength(1); k++)
                    {
                        nums.Add(matrixA.Values[i, k] * matrixB.Values[k, j]);
                    }
                    resultValues[i, j]=KahanSum(nums);
                }
            }

            return new Matrix<double>(resultValues);
        }

        public Matrix<double> MultiplyByScalar(Matrix<double> matrixA, double scalar)
        {
            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                {
                    resultValues[i, j] = matrixA.Values[i, j] * scalar;
                }
            }

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

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                {
                    resultValues[i, j] = matrixA.Values[i, j] - matrixB.Values[i, j];
                }
            }

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
