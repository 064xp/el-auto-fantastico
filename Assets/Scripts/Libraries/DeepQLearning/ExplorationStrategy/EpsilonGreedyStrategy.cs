using System;

public class EpsilonGreedyStrategy : IExplorationStrategy
{
    float EpsilonStart;
    float EpsilonEnd;
    float EpsilonDecay;
    public EpsilonGreedyStrategy(float epsilonStart, float epsilonEnd, float epsilonDecay)
    {
        EpsilonStart = epsilonStart;
        EpsilonEnd = epsilonEnd;
        EpsilonDecay = epsilonDecay;
    }

    public float GetExplorationRate(int currentStep)
    {
        return (float) (EpsilonEnd + (EpsilonStart - EpsilonEnd) * Math.Exp(-1 * EpsilonDecay * currentStep));
    }
}
