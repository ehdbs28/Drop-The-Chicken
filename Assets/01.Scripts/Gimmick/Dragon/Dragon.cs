using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : PoolableMono
{
    [SerializeField] private ParticleSystem _dangerParticle;

    [SerializeField] private Transform _dragonBodyParent;
    private DragonBody[] _dragonBodys;

    private float _speed = 20f;

    private float _delay = 1.5f;
    private bool _move = false;

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
        if (GameManager.Instance.Stop || !_move) return;
        

        _dragonBodyParent.position = _dragonBodyParent.position + Vector3.up * _speed * Time.fixedDeltaTime;
    }

    private void SetDragonBody()
    {
        for (int i = 0; i < _dragonBodys.Length; i++)
        {
            _dragonBodys[i].SetBody(i, transform.position);
            _dragonBodys[i].ShakeBody();
        }
    }

    IEnumerator DragonSetting()
    {
        _dangerParticle.Play();
        SetDragonBody();
        _move = false;
        yield return new WaitForSeconds(_delay);
        _dangerParticle.Stop();
        _move = true;
    }
    

    public override void Reset()
    {
        //
        StartCoroutine("DragonSetting");
    }
}
