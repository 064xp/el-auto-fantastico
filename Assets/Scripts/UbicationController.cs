using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UbicationController : MonoBehaviour
{
    public static Transform streetRotation = null;

     private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Street"){
            print(other.gameObject.name);
            streetRotation = other.gameObject.transform;
        }
    }
}
