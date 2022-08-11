using System;

class ReLu : IActivationFunction
{
    public float Calculate(float x)
    {
        return Math.Max(0, x);
    }

    public float Derivative(float x)
    {
        return x > 0 ? 1 : 0;
    }
}