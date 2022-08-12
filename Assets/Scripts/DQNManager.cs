using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DQNManager : MonoBehaviour
{
    DQN dqn;
    public EnvManager env;
    public int[] LayerDescriptions;


    // Start is called before the first frame update
    void Start()
    {
        LayerDescriptions[0] = env.NumStateFeatures;
        LayerDescriptions[LayerDescriptions.Length - 1] = env.NumActions;

        dqn = new DQN(env.NumStateFeatures, env, LayerDescriptions, 10);        
    }

    public void StartTraining(){
        // dqn.Train();
        print("star training");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
