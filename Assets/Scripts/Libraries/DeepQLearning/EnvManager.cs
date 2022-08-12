using System.Collections;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using Cysharp.Threading.Tasks;

public class EnvManager : MonoBehaviour
{
    public float TimeScale = 1f;
    public bool Done {get; private set; }= false;
    public int NumActions;
    public CarAgent CarAgent;
    public int NumStateFeatures {get {return CarAgent.NumFeatures;}}
    public float ActionInterval;
    public float LastReward { get; private set;}

    void Awake(){
        NumActions = Enum.GetNames(typeof(CarAgent.Actions)).Length;
    }

    void Update(){
        if(CarAgent.Crashed || CarAgent.ReachedEnd){
            Done = true;
        }

        if(Time.timeScale != TimeScale){
            Time.timeScale = TimeScale;
        }
    }

    public void Reset(){
        CarAgent.Reset();
        Done = false;
    }

    public int NumActionsAvailable(){
        return NumActions;
    }

    public IEnumerator TakeAction(int action){
        CarAgent.StartAction((CarAgent.Actions)action);
        yield return new WaitForSeconds(ActionInterval);
        CarAgent.EndAction();

        float[] newState = CarAgent.GetData();
        LastReward = CalculateReward(newState);
    }

    public float[] GetState(){
        if(Done)
            return Enumerable.Repeat(0f, CarAgent.NumFeatures).ToArray();
        
        return CarAgent.GetData();
    }

    private float CalculateReward(float[] newState){
        float reward = 0;
        float angle = newState[CarAgent.NumRays];
        float speed = newState[CarAgent.NumRays + 1];

        // Angle
        float absAngle = Mathf.Abs(angle);
        float angleR = Map(absAngle, 0, 180, 0, 2);
        float anglePunishment = absAngle > 170 ? Mathf.Abs(absAngle - 360) * -0.03f : 0f;
        reward += angleR + anglePunishment;

        // Centered
        float leftDistance = newState[0];
        float rightDistance = newState[CarAgent.NumRays - 1];
        float centerR = (1 / Mathf.Abs(leftDistance - rightDistance + .1f) * .1f);

        reward +=  centerR;

        // Speed
        float speedR = Map(speed, 0, 13, -0.5f, 5f);
        reward += speedR;

        return reward;
    }
    
    private float Map(float x, float in_min, float in_max, float out_min, float out_max) {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
