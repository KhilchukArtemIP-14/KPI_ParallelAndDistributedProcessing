using ParallelAndDistributedCalculations_Lab1.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedCalculations_Lab1.Models
{
    [Serializable]
    public class Input
    {
        public Matrix<decimal> A {  get; set; }
        public decimal a {  get; set; }
        public Matrix<decimal> MB { get; set; }
        public Matrix<decimal> C { get; set; }
        public Matrix<decimal> MT { get; set; }
        public Matrix<decimal> MZ { get; set; }
        public Matrix<decimal> ME { get; set; }
    }
}
