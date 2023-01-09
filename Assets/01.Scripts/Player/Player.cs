using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _fallingSpeed = 1f;
    [SerializeField] private float _moveSpeed = 3f;

    private void Update() {
        Debug.Log(Input.touchCount);
    }

    private void PlayerMove(){
        
    }
}
