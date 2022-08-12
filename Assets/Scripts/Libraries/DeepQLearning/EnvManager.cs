using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvManager : MonoBehaviour
{
    public bool Done {get; private set; }= false;
    public int NumActions;
    public CarAgent CarAgent;
    public bool ReadyToStart = false;
    public int NumStateFeatures {get {return CarAgent.NumFeatures;}}

    void Awake(){
        NumActions = Enum.GetNames(typeof(CarAgent.Actions)).Length;
    }

    public void Reset(){
        throw new System.NotImplementedException();
    }

    public int NumActionsAvailable(){
        return NumActions;
    }

    public float TakeAction(int action){
        CarAgent.TakeAction((CarAgent.Actions)action);
        // wait a little bit for the car to move
        float[] newState = CarAgent.GetData();

        return CalculateReward(newState);
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
        reward += Map(Mathf.Abs(angle), 0, 180, -1, 1);

        // Centered
        float leftDistance = newState[0];
        float rightDistance = newState[CarAgent.NumRays - 1];

        reward +=  1 / Mathf.Abs(leftDistance - rightDistance + .1f) * .1f;

        // Speed
        reward += Map(speed, 0, 20, 0, 1);

        return reward;
    }
    
    private float Map(float x, float in_min, float in_max, float out_min, float out_max) {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
