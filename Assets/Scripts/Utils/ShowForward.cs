using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowForward : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10.0f, Color.red);
    }
}
