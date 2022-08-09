using System.Collections;
using System.Collections.Generic;

public class NeuralNetwork
{
    public List<Layer> Layers;
    public IActivationFunction ActivationFunction;
    public float LearningRate { get; set; }
    public NeuralNetwork(int[] layerDescriptions, float learningRate, IActivationFunction activationFunction){
        Layers = new List<Layer>();
        LearningRate = learningRate;
        ActivationFunction = activationFunction;

        if(layerDescriptions.Length < 2){
            throw new System.Exception("Neural network must have at least 2 layers");
        }

        for(int i=1; i<layerDescriptions.Length; i++){
            Layers.Add(new LinearLayer(layerDescriptions[i - 1], layerDescriptions[i]));
        }
    }

    public float[] FeedForward(float[] inputs){
        Matrix outputs = Matrix.FromArray(inputs);

        for(int i=0; i<Layers.Count; i++){
            Layer layer = Layers[i];
            outputs = layer.Forward(outputs, ActivationFunction);
        }
        return outputs.ToArray();
    }

    public void Train(float[][] inputs, float[][] targets, int iterations){
        for(int i=0; i<iterations; i++){
            for(int j=0; j<inputs.Length; j++){
                float[] input = inputs[j];
                float[] target = targets[j];
                FeedForward(input);
                BackPropagate(target);
            }
        }
    }

    private void BackPropagate(float[] target){
        Matrix targetMatrix = Matrix.FromArray(target);
        Matrix error = default;

        for(int i=Layers.Count -1; i>=1; i--){
            Layer layer = Layers[i];
            // If this is the output layer
            if(i == Layers.Count - 1){
                // error = target - output
                error = targetMatrix - layer.Output;
            }
            else
                // Otherwise, error = weight_T * error_next layer
                error = Matrix.Transpose(Layers[i+1].Weights) * error;

            // Calculate gradient of current layer
            Matrix gradient = Matrix.Map(layer.Output, ActivationFunction.Derivative);
            gradient = Matrix.Hadamard(gradient, error);
            gradient *= LearningRate;

            // Calculate deltas of previous layer weights 
            // (weights of connections coming into this layer)
            Layer prevLayer = Layers[i-1];
            Matrix prevLayerT = Matrix.Transpose(prevLayer.Output);
            Matrix weightDelta = gradient * prevLayerT;

            layer.Weights += weightDelta;
            layer.Bias += gradient;
        }
    }
}
