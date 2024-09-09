using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : PoolableMono
{
    private new ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    
    }

    void Start()
    {
        var main = particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        PoolManager.Instance.Push(this);
    }

    public void Play()
    {
        particleSystem.Play();

    }

    public override void Reset()
    {

    }
}
