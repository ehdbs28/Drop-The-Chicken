using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : BaseItem
{
    public override void EnterEvent(Player player)
    {
        player.TakeMushRoom();

        PoolManager.Instance.Push(this);
    }

    public override void Reset()
    {

    }
}
