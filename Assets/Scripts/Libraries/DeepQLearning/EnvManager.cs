using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvManager : MonoBehaviour
{
    float[] CurrentState;
    public bool Done {get; private set; }= false;
    public int NumActions;

    public void Reset(){
        throw new System.NotImplementedException();
    }

    public int NumActionsAvailable(){
        return NumActions;
    }

    public float TakeAction(int action){
        throw new System.NotImplementedException();
    }

    public float[] GetState(){
        if(Done)
            return Enumerable.Repeat(0f, CurrentState.Length).ToArray();
        
        return CurrentState;
    }

    public int NumStateFeatures(){
        // return amount of inputs the car has
        throw new System.NotImplementedException();
    }

    void Update(){
        // Get current state
    }
}
