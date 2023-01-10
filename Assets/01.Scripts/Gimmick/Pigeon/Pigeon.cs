using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Pigeon : PoolableMono
{
    [SerializeField] private float speed; // 이동속도

    [SerializeField] private float maxX;
    [SerializeField] private float minX;

    private bool moveCheck = false;

    private void FixedUpdate()
    {
        PigeonMove();
    }

    private void PigeonMove()
    {
        if (GameManager.Instance.Stop) return;

        transform.position = transform.position + transform.right * speed * Time.fixedDeltaTime;
        bool checkMin = transform.position.x <= minX && moveCheck;
        bool checkMax = transform.position.x >= maxX && !moveCheck;
        if (checkMin || checkMax)
            ChangeMoveDir();
        
    }

    private void ChangeMoveDir()
    {
        //
        
        transform.rotation = ( moveCheck ) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        moveCheck = (moveCheck) ? false : true;
    }


    public override void Reset()
    {
        //
    }
}
