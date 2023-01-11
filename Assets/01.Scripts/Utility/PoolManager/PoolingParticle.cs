using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingParticle : PoolableMono
{
    private ParticleSystem _particle;

    private void Awake() {
        _particle = GetComponent<ParticleSystem>();
    }

    public void Play(){
        StartCoroutine(PlayCoroutine());
    }

    public void SetPosition(Vector3 position){
        _particle.transform.position = position;
    }

    IEnumerator PlayCoroutine(){
        _particle.Play();

        yield return new WaitForSecondsRealtime(.5f);

        Reset();
        PoolManager.Instance.Push(this);
    }

    public override void Reset()
    {
        _particle.Stop();
    }
}
