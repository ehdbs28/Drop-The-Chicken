using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlatform : MonoBehaviour
{
    float maxPosX = 2;
    float minPosX = -2;

    public void SetRandomPos()
    {
        Vector2 pos = transform.position;
        pos.x = Random.Range(minPosX, maxPosX);
        transform.position = pos;
    }
}
