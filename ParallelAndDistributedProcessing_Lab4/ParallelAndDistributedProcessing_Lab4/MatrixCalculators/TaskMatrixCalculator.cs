using ParallelAndDistributedProcessing_Lab4.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab4.MatrixCalculators
{
    public class TaskMatrixCalculator: IMatrixCalculator
    {
        public Matrix<double> Add(Matrix<double> matrixA, Matrix<double> matrixB)
        {
            if (matrixA.Values.GetLength(0) != matrixB.Values.GetLength(0) ||
                matrixA.Values.GetLength(1) != matrixB.Values.GetLength(1))
            {
                throw new ArgumentException("Matrix dimensions must match for addition.");
            }

            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];

            List<Task<double[]>> tasks = new List<Task<double[]>>();

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                int row = i;
                tasks.Add(Task.Run(() =>
                {
                    double[] rowResult = new double[matrixA.Values.GetLength(1)];
                    for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                    {
                        rowResult[j] = matrixA.Values[row, j] + matrixB.Values[row, j];
                    }
                    return rowResult;
                })); 
            }

            Task.WaitAll(tasks.ToArray());

            for (int i = 0; i < tasks.Count; i++)
            {
                double[] rowResult = tasks[i].Result;
                for (int j = 0; j < rowResult.Length; j++)
                {
                    resultValues[i, j] = rowResult[j];
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

            List<Task<double[]>> tasks = new List<Task<double[]>>();

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                int row = i;
                tasks.Add(Task.Run(() =>
                {
                    double[] rowResult = new double[matrixB.Values.GetLength(1)];
                    for (int j = 0; j < matrixB.Values.GetLength(1); j++)
                    {
                        var nums = new List<double>();
                        for (int k = 0; k < matrixA.Values.GetLength(1); k++)
                        {
                            nums.Add(matrixA.Values[row, k] * matrixB.Values[k, j]);
                        }
                        rowResult[j] = KahanSum(nums);
                    }
                    return rowResult;
                }));
            }

            Task.WaitAll(tasks.ToArray());

            for (int i = 0; i < tasks.Count; i++)
            {
                double[] rowResult = tasks[i].Result;
                for (int j = 0; j < rowResult.Length; j++)
                {
                    resultValues[i, j] = rowResult[j];
                }
            }

            return new Matrix<double>(resultValues);
        }

        public Matrix<double> MultiplyByScalar(Matrix<double> matrixA, double scalar)
        {
            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];

            List<Task<double[]>> tasks = new List<Task<double[]>>();

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                int row = i;
                tasks.Add(Task.Run(() =>
                {
                    double[] rowResult = new double[matrixA.Values.GetLength(1)];
                    for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                    {
                        rowResult[j] = matrixA.Values[row, j] * scalar;
                    }
                    return rowResult;
                }));
            }

            Task.WaitAll(tasks.ToArray());

            for (int i = 0; i < tasks.Count; i++)
            {
                double[] rowResult = tasks[i].Result;
                for (int j = 0; j < rowResult.Length; j++)
                {
                    resultValues[i, j] = rowResult[j];
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

            List<Task<double[]>> tasks = new List<Task<double[]>>();

            for (int i = 0; i < matrixA.Values.GetLength(0); i++)
            {
                int row = i;
                tasks.Add(Task.Run(() =>
                {
                    double[] rowResult = new double[matrixA.Values.GetLength(1)];
                    for (int j = 0; j < matrixA.Values.GetLength(1); j++)
                    {
                        rowResult[j] = matrixA.Values[row, j] - matrixB.Values[row, j];
                    }
                    return rowResult;
                }));
            }

            Task.WaitAll(tasks.ToArray());

            for (int i = 0; i < tasks.Count; i++)
            {
                double[] rowResult = tasks[i].Result;
                for (int j = 0; j < rowResult.Length; j++)
                {
                    resultValues[i, j] = rowResult[j];
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
