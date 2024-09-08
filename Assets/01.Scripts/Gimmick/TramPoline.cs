using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramPoline : PoolableMono, IObstacle, IBrokenObject
{
    Animator animator;
    [SerializeField] float risingSpeed;
    [SerializeField] float risingTime;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void EnterEvent(Collider2D col)
    {
        animator.SetBool("Enter", true);
        col.GetComponent<Player>().Rising(risingSpeed, risingTime);
    }

    public void EndAnimationEvent() => animator.SetBool("Enter", false);
    public void ExitEvent(Collider2D col)
    {
        
    }

    public override void Reset()
    {
        
    }

    public void StayEvent(Collider2D col)
    {
        
    }
    public void BrokenEvent()
    {
        PoolingParticle brokenParticle = PoolManager.Instance.Pop("BrokenParticleTrampoline") as PoolingParticle;

        brokenParticle.SetPosition(transform.position);
        brokenParticle.Play();

        PoolManager.Instance.Push(this);
    }

}
