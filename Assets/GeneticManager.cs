using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using UnityEngine;

public class GeneticManager
{
    public float mutationRate = 0.005f;

    public NeuralNet[] CreateOffSpring(List<Entity> safeEntities, int numberOfEntities)
    {
        NeuralNet[] nextGeneration = new NeuralNet[numberOfEntities];

        CrossOver(nextGeneration, safeEntities);
        Mutate(nextGeneration);

        return nextGeneration;
    }

    private void CrossOver(NeuralNet[] nextGeneration, List<Entity> safeEntities)
    {
        System.Random random = new System.Random();

        int numberOfParents = safeEntities.Count;
        int numberOfChildren = 0;

        int numberOfInputs = safeEntities[0].numberOfInputs;
        int numberOfOutputs = safeEntities[0].numberOfOutputs;
        int numberOfHiddenLayers = safeEntities[0].numberOfLayers;
        int numberOfHiddenNeurons = safeEntities[0].numberOfHiddenNeurons;

        while (numberOfChildren < nextGeneration.Length)
        {
            int firstParent = random.Next(numberOfParents);
            int secondParent;
            do
            {
                secondParent = random.Next(numberOfParents);
            } while (firstParent == secondParent);

            NeuralNet firstChild = new NeuralNet(numberOfInputs, numberOfOutputs);
            NeuralNet secondChild = new NeuralNet(numberOfInputs, numberOfOutputs);

            firstChild.Initialize(numberOfHiddenLayers, numberOfHiddenNeurons);
            secondChild.Initialize(numberOfHiddenLayers, numberOfHiddenNeurons);

            CrossOverWeights(safeEntities, firstParent, secondParent, firstChild, secondChild);
            CrossOverBiases(safeEntities, firstParent, secondParent, firstChild, secondChild);

            nextGeneration[numberOfChildren] = firstChild;
            numberOfChildren++;

            nextGeneration[numberOfChildren] = secondChild;
            numberOfChildren++;
        }
    }

    private void CrossOverWeights(List<Entity> safeEntities, int firstParent, int secondParent, 
                                  NeuralNet firstChild, NeuralNet secondChild)
    {
        for (int i = 0; i < firstChild.weights.Count; i++)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f)
            {
                firstChild.weights[i] = safeEntities[firstParent].neuralNet.weights[i];
                secondChild.weights[i] = safeEntities[secondParent].neuralNet.weights[i];
            }
            else
            {
                secondChild.weights[i] = safeEntities[firstParent].neuralNet.weights[i];
                firstChild.weights[i] = safeEntities[secondParent].neuralNet.weights[i];
            }
        }
    }

    private void CrossOverBiases(List<Entity> safeEntities, int firstParent, int secondParent,
                                  NeuralNet firstChild, NeuralNet secondChild)
    {
        for (int i = 0; i < firstChild.biases.Count; i++)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f)
            {
                firstChild.biases[i] = safeEntities[firstParent].neuralNet.biases[i];
                secondChild.biases[i] = safeEntities[secondParent].neuralNet.biases[i];
            }
            else
            {
                secondChild.biases[i] = safeEntities[firstParent].neuralNet.biases[i];
                firstChild.biases[i] = safeEntities[secondParent].neuralNet.biases[i];
            }
        }
    }

    private void Mutate(NeuralNet[] nextGeneration)
    {
        for (int i = 0; i < nextGeneration.Length; i++)
        {
            for (int w = 0; w < nextGeneration[i].weights.Count; w++)
            {
                if (UnityEngine.Random.Range(0.0f, 1.0f) < mutationRate)
                {
                    nextGeneration[i].weights[w] = MutateWeightMatrix(nextGeneration[i].weights[w]);
                }
            }
        }
    }

    private Matrix<float> MutateWeightMatrix(Matrix<float> weightMatrix)
    {
        Matrix<float> mutatedMatrix = weightMatrix;

        int randomRow = UnityEngine.Random.Range(0, mutatedMatrix.RowCount);
        int randomColumn = UnityEngine.Random.Range(0, mutatedMatrix.ColumnCount);

        mutatedMatrix[randomRow, randomColumn] = Mathf.Clamp(mutatedMatrix[randomRow, randomColumn] + UnityEngine.Random.Range(-1f, 1f), -1.0f, 1.0f);

        return mutatedMatrix;
    }
}
