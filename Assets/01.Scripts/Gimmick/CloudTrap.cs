using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudTrap : PoolableMono, IObstacle, IBrokenObject
{
    private ParticleSystem[] _cloudParticle;

    private void Awake() {
        _cloudParticle = GetComponentsInChildren<ParticleSystem>();
    }
    
    public void EnterEvent(Collider2D col)
    {
        _cloudParticle[0].Play();
        col.GetComponent<Player>().IsFast = true;
    }

    public void StayEvent(Collider2D col)
    {
        
    }

    public void ExitEvent(Collider2D col)
    {
        _cloudParticle[1].Play();
    }

    public override void Reset()
    {

    }

    public void BrokenEvent()
    {
        PoolingParticle brokenParticle = PoolManager.Instance.Pop("BrokenParticleCloud") as PoolingParticle;

        brokenParticle.SetPosition(transform.position);
        brokenParticle.Play();

        PoolManager.Instance.Push(this);
    }
}
