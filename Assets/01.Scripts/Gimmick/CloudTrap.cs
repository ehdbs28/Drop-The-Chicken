using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudTrap : PoolableMono, IObstacle
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
}
