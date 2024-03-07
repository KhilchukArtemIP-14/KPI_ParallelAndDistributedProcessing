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
        public Matrix<double> A {  get; set; }
        public double a {  get; set; }
        public Matrix<double> MB { get; set; }
        public Matrix<double> C { get; set; }
        public Matrix<double> MT { get; set; }
        public Matrix<double> MZ { get; set; }
        public Matrix<double> ME { get; set; }
    }
}
