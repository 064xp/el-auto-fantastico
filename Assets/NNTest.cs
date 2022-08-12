using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNTest : MonoBehaviour
{
    public int[] LayerDescriptions;
    NeuralNetwork net;
    // Start is called before the first frame update
    void Start()
    {
        LayerDescriptions = new int[]{
            3, 10, 16, 6, 3
        };

       net = new NeuralNetwork(LayerDescriptions, 0.5f, new Sigmoid());

        float[][] inputs = new float[][]{
            new float[]{0, 0, 0},
            new float[]{0, 0, 1},
            new float[]{0, 1, 0},
            new float[]{1, 1, 1},
        };
        float[][] outputs = new float[][]{
            new float[]{0, 0.5f, 1f},
            new float[]{1f, 0.5f, 0f},
            new float[]{0f, 0.5f, 0f},
            new float[]{1f, 1f, 1f},
        };

        net.Train(inputs, outputs, 10_000);

        float[] input;
        float[] output;
        input = new float[]{0, 0, 0};
        output = net.FeedForward(input);
        PrintOutput(output);

        input = new float[]{0, 0, 1};
        output = net.FeedForward(input);
        PrintOutput(output);

        input = new float[]{0, 1, 0};
        output = net.FeedForward(input);
        PrintOutput(output);

        input = new float[]{1, 1, 1};
        output = net.FeedForward(input);
        PrintOutput(output);

    }

    void PrintOutput(float[] output){
        string s = "";

        for(int i=0; i<output.Length; i++){
            s += output[i].ToString("0.00") + " ";
        }

        print(s);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
