using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState : MonoBehaviour
{
    public float hitDirection;
    public float rotation;

    public InputState(float hitDirection, float rotation)
    {
        this.hitDirection = hitDirection;
        this.rotation = rotation;
    }
}
