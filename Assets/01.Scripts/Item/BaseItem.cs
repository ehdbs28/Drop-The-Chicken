using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : PoolableMono, IItem
{
    public virtual void EnterEvent(Player player)
    {   
        
    }
}
