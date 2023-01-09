using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : IManager
{
    private Player _player;

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                Init();
                break;
            case GameState.INGAME:
                _player.IsPlay = true;    
                break;
        }
    }

    public void Init(){
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _player.IsPlay = false;
    }
}
