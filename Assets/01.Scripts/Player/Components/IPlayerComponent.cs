using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayerComponent : IManager
{
    protected Player player;

    public IPlayerComponent(Player player){
        this.player = player;
    }

    public abstract void UpdateState(GameState state);
}
