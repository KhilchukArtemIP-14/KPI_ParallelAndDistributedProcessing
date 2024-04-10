using Microsoft.Diagnostics.Runtime.Utilities;
using ParallelAndDistributedProcessing_Lab4.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab4.MatrixCalculators
{
    public class MultiThreadMatrixCalculator : IMatrixCalculator
    {
        public Matrix<double> Add(Matrix<double> matrixA, Matrix<double> matrixB)
        {
            if (matrixA.Values.GetLength(0) != matrixB.Values.GetLength(0) ||
                matrixA.Values.GetLength(1) != matrixB.Values.GetLength(1))
            {
                throw new ArgumentException("Matrix dimensions must match for addition.");
            }

            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];

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

            return new Matrix<double>(resultValues);
        }

        public Matrix<double> MultiplyByMatrix(Matrix<double> matrixA, Matrix<double> matrixB)
        {
            if (matrixA.Values.GetLength(1) != matrixB.Values.GetLength(0))
            {
                throw new ArgumentException("Matrix dimensions must match for matrix multiplication.");
            }

            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixB.Values.GetLength(1)];

            int size = CalculateOptimalBlockSize(matrixA.Values.GetLength(0), Environment.ProcessorCount);
            int blockCount = matrixA.Values.GetLength(0) / size;

            List<Thread> threads = new List<Thread>();

            double[,][,] blocksA = SplitIntoBlocks(matrixA.Values, size);
            double[,][,] blocksB = SplitIntoBlocks(matrixB.Values, size);

            for (int i = 0; i < blockCount; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    int row = i;
                    int col = j;

                    Thread thread = new Thread(() =>
                    {
                        double[,] blockResult = MultiplyBlocks(blocksA, blocksB, row, col, size);
                        lock (resultValues)
                        {
                            AppendSubmatrix(resultValues, blockResult, row * size, col * size, size);
                        }
                    });

                    thread.Start();
                    threads.Add(thread);
                }
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return new Matrix<double>(resultValues);
        }

        public Matrix<double> MultiplyByScalar(Matrix<double> matrixA, double scalar)
        {
            double[,] resultValues = new double[matrixA.Values.GetLength(0), matrixA.Values.GetLength(1)];

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
        private double[,] MultiplyBlocks(double[,][,] blocksA, double[,][,] blocksB, int row, int col, int size)
        {
            double[,] result = new double[size, size];

            for (int s = 0; s < blocksA.GetLength(0); s++)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        var products = new List<double>();

                        for (int k = 0; k < size; k++)
                        {
                            products.Add(blocksA[row,(row + s) % blocksA.GetLength(0)][i, k] * blocksB[(row + s) % blocksA.GetLength(0), (col + s) % blocksB.GetLength(0)][k, j]);
                        }

                        products.Add(result[i, j]);
                        result[i, j] = KahanSum(products);
                    }
                }
            }

            return result;
        }

        private int CalculateOptimalBlockSize(int matrixSize, int threadCount)
        {
            int size = matrixSize / threadCount;
            while (size!=0 && matrixSize % size != 0)
            {
                size--;
            }
            return Math.Max(size, 1);
        }

        public void AppendSubmatrix(double[,] original, double[,] submatrix, int offsetRow, int offsetColumn, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    original[offsetRow + i, offsetColumn + j] = submatrix[i, j];
                }
            }
        }

        public double[,][,] SplitIntoBlocks(double[,] matrix, int size)
        {
            var blockCount = matrix.GetLength(0) / size;
            double[,][,] blocks = new double[blockCount, blockCount][,];

            for (int i = 0; i < blockCount; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    blocks[i,j] = new double[size, size];
                    for (int inblockRow = i * size; inblockRow < (i + 1) * size; inblockRow++)
                    {
                        for (int inblockCol = j * size; inblockCol < (j + 1) * size; inblockCol++)
                        {
                            blocks[i, j][inblockRow % size, inblockCol % size] = matrix[inblockRow, inblockCol];
                        }
                    }
                }
            }

            return blocks;
        }
    }
}