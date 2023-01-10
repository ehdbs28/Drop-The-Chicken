using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerManager : IManager
{
    private Player _player;
    private IObservable<Vector3> playerPosStream;

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
        playerPosStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME).Select(position => _player.transform.position);
        _player.IsPlay = false;
    }

    public void PlayerPosSubscribe(Action<Vector3> action){
        playerPosStream.Subscribe(action).AddTo(GameManager.Instance);
    }
}
