using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

public class MatrixToColorMapper
{
    public (int, int, int) ComputeWeightColor(List<Matrix<float>> weights)
    {
        int totalValues = 0;
        float totalSum = 0;

        foreach (var matrix in weights)
        {
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    float value = matrix[i, j];
                    totalSum += value;
                    totalValues++;
                }
            }
        }

        // Calculate the average value
        float averageValue = totalValues > 0 ? totalSum / totalValues : 0;

        // Map the average value to an RGB color
        return MapValueToColor(averageValue);
    }

    public (int, int, int) ComputeBiasColor(List<float> biases)
    {
        int totalValues = biases.Count;
        float totalSum = 0;

        foreach (var element in biases)
        {
            totalSum += element;
        }

        float averageValue = totalValues > 0 ? totalSum / totalValues : 0;

        return MapValueToColor(averageValue);
    }

    private (int, int, int) MapValueToColor(float value)
    {
        int colorValue = (int)((value + 1) * 0.5f * 255);
        colorValue = Math.Max(0, Math.Min(255, colorValue));

        int red = (colorValue * 11) % 255;
        int green = (colorValue * 5) % 255;
        int blue = (colorValue * 7) % 255;

        return (red, green, blue);
    }
}
