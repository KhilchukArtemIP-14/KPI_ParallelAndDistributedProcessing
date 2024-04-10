using ParallelAndDistributedProcessing_Lab4.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab4.MatrixCalculators
{
    public class ForkJoinMatrixCalculator: IMatrixCalculator
    {
        private int _forkThreshold;
        public ForkJoinMatrixCalculator SetSizeThreshold(int forkThreshold)
        {
            _forkThreshold = forkThreshold;
            return this;
        }
        public ForkJoinMatrixCalculator(int forkThreshold)
        {
            _forkThreshold = forkThreshold;
        }
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

            List<Thread> threads = new List<Thread>();

            int size = CalculateOptimalBlockSize(matrixA.Values.GetLength(0), Environment.ProcessorCount);
            int blockCount = matrixA.Values.GetLength(0) / size;

            var tasks = new Task<double[,]>[blockCount * blockCount];

            double[,][,] blocksA = SplitIntoBlocks(matrixA.Values, size);
            double[,][,] blocksB = SplitIntoBlocks(matrixB.Values, size);

            for (int i = 0; i < blockCount; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    int row = i;
                    int col = j;

                    tasks[i * blockCount + j] = Task.Run(() =>
                    {
                        return MultiplyBlocks(blocksA, blocksB, row, col, size);
                    });
                }
            }

            Task.WaitAll(tasks);

            double[,] result = new double[matrixA.Values.GetLength(0), matrixB.Values.GetLength(1)];

            for (int i = 0; i < blockCount; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    AppendSubmatrix(result, tasks[i * blockCount + j].Result, i * size, j * size, size);
                }
            }

            return new Matrix<double>(result);

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
        private double[,] MultiplyBlocks(double[,][,] blocksA, double[,][,] blocksB, int row, int col, int size)
        {
            double[,] result = new double[size, size];

            for (int s = 0; s < blocksA.GetLength(0); s++)
            {
                var matrixA = blocksA[row,(row + s) % blocksA.GetLength(0)];
                var matrixB = blocksB[(row + s) % blocksA.GetLength(0), (col + s) % blocksB.GetLength(0)];

                if (matrixA.GetLength(0) > _forkThreshold)
                {
                    var multiplicationResult = MultiplyByMatrix(new Matrix<double>(matrixA), new Matrix<double>(matrixB));
                    result = Add( new Matrix<double>(result), multiplicationResult).Values;
                    continue;
                }

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        var products = new List<double>();

                        for (int k = 0; k < size; k++)
                        {
                            products.Add(matrixA[i, k] * matrixB[k, j]);
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
            int size = (int)Math.Sqrt(matrixSize*matrixSize / threadCount);
            while (size != 0 && matrixSize % size != 0)
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
