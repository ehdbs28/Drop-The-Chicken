using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudTrap : PoolableMono
{
    private ParticleSystem[] _cloudParticle;

    public override void Reset()
    {
        //
    }

    private void Awake() {
        _cloudParticle = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            _cloudParticle[0].Play();
            other.GetComponent<Player>().IsFast = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            _cloudParticle[1].Play();
        }
    }
}
