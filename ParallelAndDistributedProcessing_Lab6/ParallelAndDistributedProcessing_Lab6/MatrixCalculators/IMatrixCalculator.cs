using ParallelAndDistributedProcessing_Lab6.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab6.MatrixCalculators
{
    public interface IMatrixCalculator
    {
        public Matrix<double> MultiplyByMatrix(Matrix<double> matrixA, Matrix<double> matrixB);
        public Matrix<double> MultiplyByScalar(Matrix<double> matrixA, double scalar);
        public Matrix<double> Add(Matrix<double> matrixA, Matrix<double> matrixB);
        public Matrix<double> Substract(Matrix<double> matrixA, Matrix<double> matrixB);
    }
}
