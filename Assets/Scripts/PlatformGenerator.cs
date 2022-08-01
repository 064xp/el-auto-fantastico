using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    int platformSize = 100;
    public GameObject basePlatform;
    float increment = 11.925769f;
    Vector3 startPosition;
    void Start(){
        for(int i = 0; i < platformSize; i++){
            startPosition = transform.position;
            startPosition.x += increment * i;
            for(int j = 0; j < platformSize; j++){
                startPosition.z += increment;
                GameObject instantiated = Instantiate(basePlatform, startPosition, Quaternion.identity, transform);
                instantiated.name = i.ToString() + "," + j.ToString();
            }
        }
    }
}
