using System;

class LinearLayer : Layer {
    int inputs;
    int nodes;

    public LinearLayer(int inputs, int nodes){
        Weights = new Matrix(nodes, inputs);
        Bias = new Matrix(nodes, 1);

        this.inputs = inputs;
        this.nodes = nodes;
    }

    public override Matrix Forward(Matrix input){
        Output = (Weights * input);
        Output += Bias;
        return Output;
    }

    public override Matrix Forward(Matrix input, IActivationFunction activationFunction)
    {
        Output = Forward(input);
        Output = Matrix.Map(Output, activationFunction.Calculate);
        return Output;
    }
}