using System;

class Sigmoid : IActivationFunction
{
    public float Calculate(float x)
    {
        return (float) (1f / (1f + Math.Exp(-x)));
    }

    public float Derivative(float x)
    {
        return x * (1f - x);
    }
}