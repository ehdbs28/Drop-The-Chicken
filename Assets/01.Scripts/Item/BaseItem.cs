using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : PoolableMono, IItem, IBrokenObject
{
    public virtual void BrokenEvent()
    {
        string poolingParticleName = $"BrokenParticle{GetType()}";
        PoolingParticle brokenParticle = PoolManager.Instance.Pop(poolingParticleName) as PoolingParticle;

        if(brokenParticle != null )
        {
            brokenParticle.SetPosition(transform.position);
            brokenParticle.Play();
        }
        else
        {
            Debug.LogError($"{poolingParticleName}이 존재하지 않는다.");
        }

        PoolManager.Instance.Push(this);
    }

    public abstract void EnterEvent(Player player);
}
