using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent
{
    int currentStep;
    IExplorationStrategy strategy;
    int numActions;

    public Agent(int numActions, IExplorationStrategy strategy)
    {
        this.strategy = strategy;
        this.numActions = numActions;
        this.currentStep = 0;
    }

    public int SelectAction(float[] state, NeuralNetwork policyNet){
        float rate = strategy.GetExplorationRate(currentStep);
        currentStep++;
        if(rate > Random.value)
            return Random.Range(0, numActions);
        
        return NeuralNetwork.ResultsToIndex(policyNet.FeedForward(state));
    }
}
