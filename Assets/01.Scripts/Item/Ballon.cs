using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : BaseItem
{

    public override void EnterEvent(Player player)
    {
        player.TakeBallon();

        PoolManager.Instance.Push(this);
    }

    public override void Reset()
    {
        
    }
}
