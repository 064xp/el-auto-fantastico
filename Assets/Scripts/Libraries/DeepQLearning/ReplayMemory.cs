using System.Collections;
using System.Collections.Generic;

public class ReplayMemory{
    int Capacity;
    List<Experience> Memory;
    int PushCount;
    public ReplayMemory(int capacity){
        Capacity = capacity;
        Memory = new List<Experience>();
        PushCount = 0;
    }

    public void Push(Experience experience){
        if(Memory.Count < Capacity)
            Memory.Add(experience);
        else
            Memory[PushCount % Capacity] = experience;
        PushCount++;
    }

    public Experience[] Sample(int batchSize){
        List<int> indices = new List<int>();
        List<Experience> batch = new List<Experience>();

        for(int i = 0; i < batchSize; i++){
            while(true){
                int index = UnityEngine.Random.Range(0, Memory.Count);
                if(!indices.Contains(index)){
                    indices.Add(index);
                    batch.Add(Memory[index]);
                    break;
                }
            }
        }
        return batch.ToArray();
    }

    public bool CanProvideSample(int batchSize){
        return Memory.Count >= batchSize;
    }
}