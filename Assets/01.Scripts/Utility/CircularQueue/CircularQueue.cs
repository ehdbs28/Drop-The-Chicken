using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CircularQueue<T>
{
    public List<T> queue;

    public void Enqueue(T param){
        queue.Add(param);
    }

    public T Dequeue(){
        T value = default(T);

        queue.Add(queue[0]);
        value = queue[0];
        queue.RemoveAt(0);

        return value;
    }
}    
