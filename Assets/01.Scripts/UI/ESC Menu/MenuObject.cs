using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuObject : MonoBehaviour
{
    public float ObjectPosition;
    public abstract void ObjectEvent();

    private void Awake() {
        ObjectPosition = transform.position.y;
    }
}
