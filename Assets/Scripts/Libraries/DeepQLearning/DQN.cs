using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DQN 
{
    int NumStateFeatures;
    int BatchSize;
    float Gamma;
    float EpsilonStart;
    float EpsilonEnd;
    float EpsilonDecay;
    int TargetUpdate;
    int MemorySize;
    float LearningRate;
    int NumEpisodes;
    int TrainigIterations;
    EnvManager Env;

    Agent agent;
    ReplayMemory memory;
    NeuralNetwork policyNet;
    NeuralNetwork targetNet;
    // Ammount of time between each action taken
    public float actionInterval = 0.4f;

    public DQN(
        int numStateFeatures,
        EnvManager env,
        int[] layerDescriptions,
        int trainingIterations,
        int batchSize = 256, 
        float gamma = 0.99f, 
        float epsilonStart = 1, float epsilonEnd = 0.1f, float epsilonDecay = 0.001f,
        int targetUpdate = 10,
        int memorySize = 100_000,
        float learningRate = 0.001f,
        int numEpisodes = 1_000
    ){
        NumStateFeatures = numStateFeatures;
        BatchSize = batchSize;
        Gamma = gamma;
        EpsilonStart = epsilonStart;
        EpsilonEnd = epsilonEnd;
        EpsilonDecay = epsilonDecay;
        TargetUpdate = targetUpdate;
        MemorySize = memorySize;
        LearningRate = learningRate;
        NumEpisodes = numEpisodes;
        Env = env;
        TrainigIterations = trainingIterations;


        // Initialize 
        agent = new Agent(NumStateFeatures, new EpsilonGreedyStrategy(EpsilonStart, EpsilonEnd, EpsilonDecay));
        memory = new ReplayMemory(MemorySize);
        policyNet = new NeuralNetwork(layerDescriptions, learningRate, new ReLu());
        targetNet = new NeuralNetwork(layerDescriptions, learningRate, new ReLu());

        targetNet.CopyNetwork(policyNet);
    }

    private (float[][], int[], float[], float[][]) ExtractTensors(Experience[] experiences){
        float[][] states = new float[experiences.Length][];
        int[] actions = new int[experiences.Length];
        float[] rewards = new float[experiences.Length];
        float[][] nextStates = new float[experiences.Length][];
        for(int i = 0; i < experiences.Length; i++){
            states[i] = experiences[i].State;
            actions[i] = experiences[i].Action;
            rewards[i] = experiences[i].Reward;
            nextStates[i] = experiences[i].NextState;
        }
        return (states, actions, rewards, nextStates);
    }

    public float[] GenerateTargetValue(float[] result, int action, float targetQValue){
        float[] target = result.Clone() as float[];
        target[action] = targetQValue;

        return target;
    }

    public IEnumerator Train(){
        for(int episode=0; episode<NumEpisodes; episode++){
            Env.Reset();
            float[] state = Env.GetState();

            for(int timeStep=0; ; timeStep++){
                yield return new WaitForEndOfFrame();
                int action = agent.SelectAction(state, policyNet);
                float reward = Env.TakeAction(action);
                // maybe wait for x seconds before next state?
                float[] nextState = Env.GetState();
                memory.Push(new Experience(state, action, reward, nextState));
                state = nextState;

                if(memory.CanProvideSample(BatchSize)){
                    Experience[] experiences = memory.Sample(BatchSize);
                    var (states, actions, rewards, nextStates) = ExtractTensors(experiences);

                    Matrix MRewards = Matrix.FromArray(rewards); // 256x1
                    // Matrix currentQValues = Matrix.FromArray(QValues.GetCurrent(policyNet, states, actions));
                    Matrix nextQValues = Matrix.FromArray(QValues.GetNext(targetNet, nextStates)); // 256 x 1

                    float[] targetQValues = ((nextQValues * Gamma) + MRewards).ToArray(); // 256 x 1

                    policyNet.Train(states, 1, (float[] input, float[] output, int index) => 
                        GenerateTargetValue(output, actions[index], targetQValues[index])
                    );

                    yield return new WaitForSeconds(actionInterval);
                    if(Env.Done){
                        break;
                    }
                }

                if(episode % TargetUpdate == 0){
                    targetNet.CopyNetwork(policyNet);
                }
            }
        }
    }


}
