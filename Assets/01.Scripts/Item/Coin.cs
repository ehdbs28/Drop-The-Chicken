using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : BaseItem
{
    public override void EnterEvent(Player player)
    {
        Debug.Log("Coin È¹µæ");
        GameManager.Instance.GetManager<CashManager>().AddCash(1);

        PoolManager.Instance.Push(this);
    }

    public override void Reset()
    {

    }
}
