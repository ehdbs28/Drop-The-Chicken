using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchTrap : PoolableMono
{
    [SerializeField]
    private List<Sprite> branchSprites = new List<Sprite>();
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void ResetBranch()
    {
        _spriteRenderer.sprite = branchSprites[(int)Random.Range(0, branchSprites.Count)];
    }

    public override void Reset()
    {
        //
        ResetBranch();
    }
}
