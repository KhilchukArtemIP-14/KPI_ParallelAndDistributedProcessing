using ParallelAndDistributedCalculations_Lab1.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedCalculations_Lab1.MatrixCalculators
{
    public interface IMatrixCalculator
    {
        public Matrix<decimal> MultiplyByMatrix(Matrix<decimal> matrixA, Matrix<decimal> matrixB);
        public Matrix<decimal> MultiplyByScalar(Matrix<decimal> matrixA, decimal scalar);
        public Matrix<decimal> Add(Matrix<decimal> matrixA, Matrix<decimal> matrixB);
        public Matrix<decimal> Substract(Matrix<decimal> matrixA, Matrix<decimal> matrixB);
    }
}
