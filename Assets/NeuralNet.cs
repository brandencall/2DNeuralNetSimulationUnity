using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using System;

public class NeuralNet
{
    private int numberOfInputs;
    private int numberOfOutputs;

    public Matrix<float> inputLayer;
    public List<Matrix<float>> hiddenLayers = new List<Matrix<float>>();
    public Matrix<float> outputLayer;
    public List<Matrix<float>> weights = new List<Matrix<float>>();
    public List<float> biases = new List<float>();

    public NeuralNet(int numberOfInputs = 1, int numberOfOutputs = 1)
    {
        this.numberOfInputs = numberOfInputs;
        this.numberOfOutputs = numberOfOutputs;

        inputLayer = Matrix<float>.Build.Dense(1, numberOfInputs);
        outputLayer = Matrix<float>.Build.Dense(1, numberOfOutputs);
    }

    public void Initialize(int hiddenLayerCount, int hiddenNeuronCount)
    {
        inputLayer.Clear();
        hiddenLayers.Clear();
        outputLayer.Clear();
        weights.Clear();
        biases.Clear();

        for (int i = 0; i < hiddenLayerCount + 1; i++)
        {
            Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronCount);
            hiddenLayers.Add(f);

            biases.Add(UnityEngine.Random.Range(-1f, 1f));

            //Adding the weights
            if (i == 0)
            {
                Matrix<float> inputToH1 = Matrix<float>.Build.Dense(numberOfInputs, hiddenNeuronCount);
                weights.Add(inputToH1);
            }
            else
            {
                Matrix<float> hiddenToHidden = Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount);
                weights.Add(hiddenToHidden);
            }

        }

        Matrix<float> outputWeight = Matrix<float>.Build.Dense(hiddenNeuronCount, numberOfOutputs);
        weights.Add(outputWeight);
        biases.Add(UnityEngine.Random.Range(-1f, 1f));

        RandomizeWeights();

    }

    private void RandomizeWeights()
    {
        for (int i = 0; i < weights.Count; i++)
        {
            for (int x = 0; x < weights[i].RowCount; x++)
            {
                for (int y = 0; y < weights[i].ColumnCount; y++)
                {
                    weights[i][x, y] = UnityEngine.Random.Range(-4f, 4f);
                }
            }
        }
    }

    public List<float> RunNetwork(List<float> inputs)
    {
        List<float> result = new List<float>();

        for(int i = 0; i < inputs.Count; i++)
        {
            //inputLayer[0, i] = inputs[i];
            inputLayer[0, i] = Sigmoid(inputs[i]);
        }

        //inputLayer = inputLayer.PointwiseTanh();

        hiddenLayers[0] = (inputLayer * weights[0] + biases[0]).PointwiseTanh();

        for (int i = 1; i < hiddenLayers.Count; i++)
        {
            hiddenLayers[i] = (hiddenLayers[i - 1] * weights[i] + biases[i]).PointwiseTanh();
        }

        outputLayer = (hiddenLayers[^1] * weights[^1] + biases[^1]).PointwiseTanh();

        for (int i = 0; i < numberOfOutputs; i++)
        {
            result.Add(outputLayer[0, i]);
        }

        return result;
    }

    private float Sigmoid(float x)
    {
        return (1 / (1 + MathF.Exp(-x)));
    }

}
