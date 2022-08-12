using System;

class InputLayer : Layer {
    // int inputs;

    public InputLayer(){
    }

    public override Matrix Forward(Matrix input){
        Output = input;
        return Output;
    }

    public override Matrix Forward(Matrix input, IActivationFunction activationFunction)
    {
        Output = input;
        return Output;
    }
}