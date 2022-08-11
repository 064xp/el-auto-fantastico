using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasts : MonoBehaviour
{
    int angleToMatch = 180;
    int numberOfRays = 5;
    public GameObject pointToLookAt;
    public GameObject player;
    InputState[] inputs; 
    Quaternion q;
    Vector3 d;

    private void Start() {
        inputs = new InputState[numberOfRays];
    }

    private void Update(){
        rayCastsFunction();
    }

    private void rayCastsFunction(){
        float angle = -90;
        for(int i=0; i<numberOfRays; i++){
            Debug.DrawRay(transform.position,  Quaternion.AngleAxis(angle, transform.up) * transform.forward * 20, Color.cyan);
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Quaternion.AngleAxis(angle, transform.up) * transform.forward, out hit, Mathf.Infinity)){
                inputs[i] = new InputState(hit.distance, Vector3.Angle(UbicationController.streetRotation.forward, transform.forward));
                print("Direction:" + inputs[i].hitDirection + " Rotation:" + inputs[i].rotation);
            }
            angle += angleToMatch / (numberOfRays - 1);
        }
    }
}
