using ParallelAndDistributedCalculations_Lab1.Models;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ParallelAndDistributedCalculations_Lab1.Matrices;
namespace ParallelAndDistributedCalculations_Lab1.Data
{
    public class DataManager
    {
        public Input GetDataFromFile(string path)
        {
            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);

                var options = new JsonSerializerOptions
                {
                    Converters = { new MatrixConverter<double>() }
                };

                return JsonSerializer.Deserialize<Input>(jsonString,options);
            }

            return null;
        }

        public void SaveDataToFile(Input data, string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new MatrixConverter<double>() }
            };
            string jsonString = JsonSerializer.Serialize(data,options);
            File.WriteAllText(path, jsonString);
        }

        public Input GenerateData(int height, int width)
        {
            Input input = new Input();
            Random random = new Random();
            input.A = new Matrix<double>(GenerateArray(1, width, 15));
            input.MT = new Matrix<double>(GenerateArray(height,width,18));
            input.MB = new Matrix<double>(GenerateArray(height, width,0));
            input.MZ = new Matrix<double>(GenerateArray(height, width,13));
            input.ME = new Matrix<double>(GenerateArray(height, width, 5));
            input.C = new Matrix<double>(GenerateArray(1, width, 8));
            input.a = NextDouble(random, 10);

            return input;
        }
        private double[,] GenerateArray(int height, int width, byte scale)
        {
            var array= new double[height, width];
            Random random = new Random();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    array[i, j] = NextDouble(random, scale);
                }
            }

            return array;
        }

        public double NextDouble(Random rng, byte scale)
        {
            if (-1 > scale || scale > 18)
            {
                throw new ArgumentException("Wrong scale");
            }
            var nextInt = rng.NextInt64((long)Math.Pow(10, scale-1), (long)Math.Pow(10, scale)+1);
            double decimals = rng.NextDouble();
            return nextInt + decimals;
        }
    }
}
