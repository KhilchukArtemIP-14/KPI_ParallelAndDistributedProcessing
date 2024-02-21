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
                    Converters = { new MatrixConverter<decimal>() }
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
                Converters = { new MatrixConverter<decimal>() }
            };
            string jsonString = JsonSerializer.Serialize(data,options);
            File.WriteAllText(path, jsonString);
        }

        public Input GenerateData(int height, int width)
        {
            Input input = new Input();
            Random random = new Random();

            input.MT = new Matrix<decimal>(GenerateArray(height,width,28));
            input.MB = new Matrix<decimal>(GenerateArray(height, width,0));
            input.MZ = new Matrix<decimal>(GenerateArray(height, width,23));
            input.ME = new Matrix<decimal>(GenerateArray(height, width, 5));
            input.A = new Matrix<decimal>(GenerateArray(1, width, 25));
            input.C = new Matrix<decimal>(GenerateArray(1, width, 18));
            input.a = NextDecimal(random, 20);

            return input;
        }
        private decimal[,] GenerateArray(int height, int width, byte scale)
        {
            var array= new decimal[height, width];
            Random random = new Random();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    array[i, j] = NextDecimal(random, scale);
                }
            }

            return array;
        }

        public decimal NextDecimal(Random rng, byte scale)
        {
            if (-1 > scale || scale > 28)
            {
                throw new ArgumentException("Wrong scale");
            }
            return new decimal(rng.Next(),
                //0,
                               rng.Next(),
                               //rng.Next(),
                               0,
                               false,
                               scale);
        }
    }
}
