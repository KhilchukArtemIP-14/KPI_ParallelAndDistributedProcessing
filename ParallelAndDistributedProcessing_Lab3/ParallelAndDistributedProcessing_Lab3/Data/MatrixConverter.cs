using ParallelAndDistributedCalculations_Lab1.Matrices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ParallelAndDistributedCalculations_Lab1.Data
{
    public class MatrixConverter<T> : JsonConverter<Matrix<T>> where T : IComparable<T>
    {
        public override Matrix<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            List<List<T>> valuesList = new List<List<T>>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException();
                }

                List<T> row = new List<T>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        break;
                    }

                    T value = JsonSerializer.Deserialize<T>(ref reader, options);
                    row.Add(value);
                }

                valuesList.Add(row);
            }

            T[,] values = new T[valuesList.Count, valuesList[0].Count];

            for (int i = 0; i < valuesList.Count; i++)
            {
                for (int j = 0; j < valuesList[0].Count; j++)
                {
                    values[i, j] = valuesList[i][j];
                }
            }

            return new Matrix<T>(values);
        }

        public override void Write(Utf8JsonWriter writer, Matrix<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            for (int i = 0; i < value.Values.GetLength(0); i++)
            {
                writer.WriteStartArray();
                for (int j = 0; j < value.Values.GetLength(1); j++)
                {
                    JsonSerializer.Serialize(writer,  value.Values[i, j], options);
                }
                writer.WriteEndArray();
            }

            writer.WriteEndArray();
        }
    }
}
