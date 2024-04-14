using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndDistributedProcessing_Lab6.Matrices
{
    [Serializable]
    public class Matrix<T> : IMatrix<T>  where T : IComparable<T>
    {
        // To serialize
        public Matrix()
        {
        }
        public Matrix(T[,] values)
        {
            this.Values = values;
        }

        public T Max => Values.Cast<T>().Max();

        public T Min => Values.Cast<T>().Min();

        public T[,] Values { get;set;}

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var value in Values)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            var repr = "";
            for (int i = 0; i < this.Values.GetLength(0); i++)
            {
                for (int j = 0; j < this.Values.GetLength(1); j++)
                {
                    repr = string.Concat(repr, String.Format("{0:F}", Values[i, j])," ");
                }
                repr = string.Concat(repr, "\n");
            }
            return repr;
        }
    }
}
