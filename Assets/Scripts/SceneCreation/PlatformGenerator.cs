using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public int platformSize = 100;
    public GameObject basePlatform;
    public float increment = 1;
    Vector3 startPosition;
    void Start(){
        createPlatform(1);
        createPlatform(-1);
    }

    void createPlatform(int direction){
        startPosition = transform.position;
        for(int i = 0; i < platformSize; i++){
            for(int j = 0; j < platformSize; j++){
                Vector3 vectorTemp = new Vector3(i * direction, 0, j);
                GameObject instantiated = Instantiate(basePlatform, startPosition + vectorTemp, Quaternion.identity, transform);
                instantiated.name = i.ToString() + "," + j.ToString();
            }
        }
    }
}
