using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    [SerializeField] public List<Vector3> position = new List<Vector3>();
    [SerializeField] public List<Quaternion> rotation = new List<Quaternion>();
    [SerializeField] public List<int> prefabIndex = new List<int>();

    public void addData(Vector3 position, Quaternion rotation, int prefabIndex){
        this.position.Add(position);
        this.rotation.Add(rotation);
        this.prefabIndex.Add(prefabIndex);
    }

    public void clearData(){
        this.position.Clear();
        this.rotation.Clear();
        this.prefabIndex.Clear();
    }
}
