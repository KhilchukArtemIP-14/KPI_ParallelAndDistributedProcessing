using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab6.Matrices
{
        public interface IMatrix<T>:IEnumerable<T> where T : IComparable<T>
        {
            public T Max { get;}
            public T Min { get;}
            public T[,] Values { get; set; }
        }
    }
