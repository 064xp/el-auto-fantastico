using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Actions = CarAgent.Actions;

public class DQNManager : MonoBehaviour
{
    DQN dqn;
    public EnvManager env;
    public int[] LayerDescriptions;
    bool doneTraining = false;


    // Start is called before the first frame update
    void Start()
    {
        LayerDescriptions[0] = env.NumStateFeatures;
        LayerDescriptions[LayerDescriptions.Length - 1] = env.NumActions;

        dqn = new DQN(env.NumStateFeatures, env, LayerDescriptions, 10_000, learningRate: 0.1f);        
        
    }

    public void StartTraining(){
        StartCoroutine(dqn.Train());
    }

    public void OnGoalReached(){
        if(doneTraining)
            return;
        doneTraining = true;
        StopAllCoroutines();
        StartCoroutine(Move());
    }

    IEnumerator Move(){
        print("start move");
        env.Reset();
        yield return new WaitForSeconds(1);
        while(true){
            Actions action = dqn.GetAction(env.CarAgent.GetData());
            env.CarAgent.StartAction(action);
            yield return new WaitForSeconds(env.ActionInterval);
            env.CarAgent.EndAction();

            if(env.Done){
                env.Reset();
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
