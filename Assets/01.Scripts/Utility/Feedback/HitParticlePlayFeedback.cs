using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticlePlayFeedback : MonoBehaviour
{
    [SerializeField] PoolableMono particle;

    public void Play(Vector3 pos)
    {
        HitParticle particle = PoolManager.Instance.Pop("HitParticle") as HitParticle;
        particle.transform.position = pos;
        particle.Play();
    }
}
