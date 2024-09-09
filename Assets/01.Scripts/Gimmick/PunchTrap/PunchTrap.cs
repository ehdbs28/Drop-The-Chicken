using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchTrap : PoolableMono, IObstacle, IBrokenObject
{
    [SerializeField] float punchForce;
    [SerializeField] float controlTime;
    [SerializeField] bool isLeft; // 왼쪽을 보고 있는가

    public void EnterEvent(Collider2D col)
    {
        float xForce = isLeft ? -punchForce : punchForce;
        Vector2 force = new Vector2(xForce, 0);

        Player player = col.GetComponent<Player>();
        player.AddForce(force, controlTime);
        player.GetHitBlinkFeedback.Play();
        player.GetHitParticlePlayFeedback.Play(player.transform.position);

        GameManager.Instance.GetManager<TimeManager>().Stop(0.05f, 0.25f);
    }

    public void StayEvent(Collider2D col)
    {

    }

    public void ExitEvent(Collider2D col)
    {
    }

    public override void Reset()
    {
        
    }

    public void BrokenEvent()
    {
        PoolingParticle brokenParticle = PoolManager.Instance.Pop("BrokenParticlePunchTrap") as PoolingParticle;

        brokenParticle.SetPosition(transform.position);
        brokenParticle.Play();

        PoolManager.Instance.Push(this);
    }
}
