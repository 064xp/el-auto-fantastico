using System.Collections.Generic;
using System.Linq;

public class QValues{
    public static float[] GetCurrent(NeuralNetwork network, float[][] states, int[] actions){
        List<float> qValues = new List<float>();

        foreach(float[] state in states){
            float[] result = network.FeedForward(state);

            qValues.Add(NeuralNetwork.ResultsToIndex(result));
        }
        return qValues.ToArray();
    }

    public static float[] GetNext(NeuralNetwork network, float[][] nextStates){
        List<int> nonFinalStateLocations = new List<int>();

        for(int i=0; i<nextStates.Length; i++){
            float[] state = nextStates[i];
            if(!state.All(x => x == 0)){
                nonFinalStateLocations.Add(i);
            }
        }

        int batchSize = nextStates.Length;
        float[] values = nextStates.Select((state, index) =>
        {
            if (nonFinalStateLocations.Contains(index))
            {
                return NeuralNetwork.LargestResult(network.FeedForward(state));
            }
            return 0f;
        }
        ).ToArray();

        return values;
    }
}