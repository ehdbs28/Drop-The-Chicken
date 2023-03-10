using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchTrap : PoolableMono, IObstacle, IBrokenObject
{
    [SerializeField]
    private List<Sprite> branchSprites = new List<Sprite>();
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void ResetBranch()
    {
        _spriteRenderer.sprite = branchSprites[(int)Random.Range(0, branchSprites.Count)];
    }

    public void EnterEvent(Collider2D col)
    {
        Player player = col.GetComponent<Player>();
        player.OnDamage();
    }

    public void StayEvent(Collider2D col)
    {
    }

    public void ExitEvent(Collider2D col)
    {
    }

    public override void Reset()
    {
        ResetBranch();
    }

    public void BrokenEvent()
    {
        PoolingParticle brokenParticle = PoolManager.Instance.Pop("BrokenParticleBranch") as PoolingParticle;

        brokenParticle.SetPosition(transform.position);
        brokenParticle.Play();

        PoolManager.Instance.Push(this);
    }
}
