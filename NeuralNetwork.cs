using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.Rendering.DebugUI.Table;

public class NeuralNetwork : MonoBehaviour
{
    void sigmoid(float[][] layers, int l)
    {
        for(int i = 0; i < layers[l].Length; i++)
        {
            layers[l][i] = 1 / (1 + Mathf.Exp(-layers[l][i]));
        }
    }

    void leaky_relu(float[][] layers, int l)
    {
        for (int i = 0; i < layers[l].Length; i++)
        {
            if (layers[l][i] < 0) layers[l][i] *= 0.02f;
        }
    }

    void tanh(float[][] layers, int l)
    {
        for (int i = 0; i < layers[l].Length; i++)
        {
            float is_negative = layers[l][i] < 0 ? -1.0f : 1.0f;
            layers[l][i] = (1-Mathf.Exp(-2*layers[l][i]))/(1+Mathf.Exp(-2*layers[l][i]));
            if (float.IsNaN(layers[l][i]))
                layers[l][i] = is_negative;
        }
    }

    public void InitializeNetwork(float[][] layers, float[][,] new_weights, float[][] biases, int count_of_hidden_layers)
    {
        //init weights
        for (int n = 0; n < count_of_hidden_layers+1; n++)
        {
            for(int i = 0; i < new_weights[n].GetLength(0); i++)
            {
                for(int j = 0; j < new_weights[n].GetLength(1); j++)
                {
                    new_weights[n][i, j] = Random.Range(-1f, 1f);
                }
            }
        }

        //init layers
        for(int i = 1; i < layers.Length - 1; i++)
        {
            for(int j = 0; j < layers[i].Length; j++)
            {
                layers[i][j] = 0;
                biases[i][j] = Random.Range(-0.00002f, 0.00002f);
            }
        }
    }

    public void ForwardPropagation(float[][] layers, float[][,] new_weights, float[][] biases)
    {
        MakeNeuronsZero(layers);
        for(int i = 1; i < layers.Length; i++)
        {
            for(int j = 0; j < layers[i].Length; j++)
            {
                for (int k = 0; k < new_weights[i - 1].GetLength(0); k++)
                {
                    layers[i][j] += new_weights[i - 1][k, j] * layers[i-1][k];
                }
                layers[i][j] += biases[i][j];
            }
            if(i < layers.Length-1)
                tanh(layers, i);
            // if(i == layers.Length-1)
            //     tanh(layers, i);
            // else
            //     sigmoid(layers, i);
        }
    }
    public void MakeNeuronsZero(float[][] layers)
    {
        if (layers == null)
        {
            Debug.LogError("Layers array is null!");
            return;
        }

        for (int i = 1; i < layers.Length; i++)
        {
            if (layers[i] == null)
            {
                Debug.LogError($"Layer {i} is not initialized!");
                continue;
            }

            for (int j = 0; j < layers[i].Length; j++)
            {
                layers[i][j] = 0;
            }
        }
    }

    public void Mutate(float[][,] new_weights, int count_of_hidden_layers, float[][] biases, int mutation_chance_percantage)
    {
        for (int n = 0; n < count_of_hidden_layers + 1; n++)
        {
            for (int i = 0; i < new_weights[n].GetLength(0); i++)
            {
                for (int j = 0; j < new_weights[n].GetLength(1); j++)
                {
                    if (float.IsNaN(new_weights[n][i, j])) 
                        new_weights[n][i, j] = 0.0f;
                    if (mutation_chance_percantage <= Random.Range(0, 101))
                        new_weights[n][i, j] += Random.Range(-0.005f, 0.005f);
                }
            }
        }

        for (int i = 1; i < biases.Length - 1; i++)
        {
           for (int j = 0; j < biases[i].Length; j++)
           {
                if (mutation_chance_percantage <= Random.Range(0, 101))
                    biases[i][j] += Random.Range(-0.00001f, 0.00001f);
           }
        }
    }
}
