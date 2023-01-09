using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : PoolableMono
{
    [SerializeField] private Transform _dragonBodyParent;
    private DragonBody[] _dragonBodys;

    private float _speed = 20f;


    private void Awake()
    {
        _dragonBodys = _dragonBodyParent.GetComponentsInChildren<DragonBody>();

        SetDragonBody();
    }

    private void FixedUpdate()
    {
        DragonMove();
    }

    private void DragonMove()
    {
        if (GameManager.Instance.Stop) return;

        transform.position = transform.position + Vector3.up * _speed * Time.fixedDeltaTime;
    }

    private void SetDragonBody()
    {
        for (int i = 0; i < _dragonBodys.Length; i++)
        {
            _dragonBodys[i].SetBody(i);
            _dragonBodys[i].ShakeBody();
        }
    }
    

    public override void Reset()
    {
        //
        SetDragonBody();
    }
}
