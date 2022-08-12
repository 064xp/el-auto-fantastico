using System.Collections;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
// using Cysharp.Threading.Tasks;

public class EnvManager : MonoBehaviour
{
    [Range(1f, 100f)]
    public float TimeScale = 1f;
    public bool Done {get; private set; }= false;
    public int NumActions;
    public CarAgent CarAgent;
    public int NumStateFeatures {get {return CarAgent.NumFeatures;}}
    public float ActionInterval;
    public float LastReward { get; private set;}
    public Vector3 LastAgentPos;
    public UnityEvent OnGoalReached;

    public int action = 0;

    void Awake(){
        NumActions = Enum.GetNames(typeof(CarAgent.Actions)).Length;
    }

    void Start(){
        LastAgentPos = CarAgent.startPosition;
    }

    void Update(){
        if(CarAgent.Crashed || CarAgent.ReachedEnd){
            Done = true;
            if(CarAgent.ReachedEnd)
                OnGoalReached.Invoke();

        }

        if(Time.timeScale != TimeScale){
            Time.timeScale = TimeScale;
        }
    }

    public void Reset(){
        CarAgent.Reset();
        Done = false;
        LastAgentPos = CarAgent.startPosition;
    }

    public int NumActionsAvailable(){
        return NumActions;
    }

    public IEnumerator TakeAction(int action){
        float[] prevState = CarAgent.GetData();
        Vector3 startPosition = CarAgent.transform.position;
        CarAgent.StartAction((CarAgent.Actions)action);
        yield return new WaitForSeconds(ActionInterval);
        CarAgent.EndAction();

        float[] newState = CarAgent.GetData();
        // LastReward = CalculateReward(newState);
        LastReward = CalculateRewardNew(prevState, action, newState);
    }

    public float[] GetState(){
        if(Done)
            return Enumerable.Repeat(0f, CarAgent.NumFeatures).ToArray();
        
        return CarAgent.GetData();
    }

    float CalculateRewardNew(float[] state, int action, float[] newState){
        float reward = 0f;
        float[] rays = newState.Take(CarAgent.NumRays).ToArray();

        int maxRayIndex = Array.IndexOf(rays, rays.Max());
        int minRayIndex = Array.IndexOf(rays, rays.Min());
        int middleRay = Mathf.FloorToInt(CarAgent.NumRays / 2);

        // right
        if(
            action == (int)CarAgent.Actions.ForwardRight
            || action == (int)CarAgent.Actions.Right
        ){
            if(maxRayIndex > middleRay)
                reward += 10f;
            else 
                reward -= 1f;
            
            if(minRayIndex > middleRay)
                reward -= 4f;
        }

        // left
        if(
            action == (int)CarAgent.Actions.ForwardLeft
            || action == (int)CarAgent.Actions.Left
        ){
            if(maxRayIndex < middleRay)
                reward += 10f;
            else 
                reward -= 1f;
            
            if(minRayIndex < middleRay)
                reward -= 4f;
        }

        // center
        if(
            action == (int)CarAgent.Actions.Forward
        ){
            if(maxRayIndex == middleRay)
                reward += 10f;
            else 
                reward -= 1f;
            
            if(minRayIndex == middleRay)
                reward -= 4f;
        }

        float angle = newState[CarAgent.NumRays];
        float absAngle = Mathf.Abs(angle);
        // float angleR = Map(absAngle, 0, 180, 0, 0.5f);
        float anglePunishment = absAngle > 170 ? Mathf.Abs(absAngle - 360) * -0.03f : 0f;
        // reward += angleR + anglePunishment;
        reward += anglePunishment;


        float speed = newState[CarAgent.NumRays + 1];
        float speedR = Map(speed, 0, 13, -0.5f, 1f);
        reward += speedR;
        return reward;

    }

    private float CalculateReward(float[] newState){
        float reward = 0;
        float angle = newState[CarAgent.NumRays];
        float speed = newState[CarAgent.NumRays + 1];

        // Angle
        float absAngle = Mathf.Abs(angle);
        // float angleR = Map(absAngle, 0, 180, 0, 0.5f);
        float anglePunishment = absAngle > 170 ? Mathf.Abs(absAngle - 360) * -0.03f : 0f;
        // reward += angleR + anglePunishment;
        reward += anglePunishment;

        // Centered
        float leftDistance = newState[0];
        float rightDistance = newState[CarAgent.NumRays - 1];
        float centerR = (1 / Mathf.Abs(leftDistance - rightDistance + .1f) * .1f);

        reward +=  centerR;

        // Speed
        float speedR = Map(speed, 0, 13, -0.5f, 5f);
        reward += speedR;

        // position
        if(LastAgentPos != Vector3.zero){
            Vector3 movement = CarAgent.transform.position - LastAgentPos;
            float distanceTraveled = Vector3.Dot(CarAgent.RoadForward, movement);
            reward += distanceTraveled * 2f;

            Debug.Log($"Distance Traveled {distanceTraveled}");
        }

        LastAgentPos = CarAgent.transform.position;

        if(CarAgent.ReachedEnd)
            reward += 5f;

        return reward;
    }
    
    private float Map(float x, float in_min, float in_max, float out_min, float out_max) {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
