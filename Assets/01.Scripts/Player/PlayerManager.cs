using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class PlayerManager : IManager
{
    private Player _player;
    private IObservable<Vector3> playerPosStream;
    private IObservable<List<bool>> playerFeverStream;

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

        playerFeverStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME).Select(list => _player.Fevers);
        playerPosStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME).Select(position => _player.transform.position);
        
        _player.IsPlay = false;
        _player.Fevers = Enumerable.Repeat(false, 5).ToList();
    }

    public void PlayerFeverSubscribe(Action<List<bool>> action){
        playerFeverStream.Subscribe(action).AddTo(GameManager.Instance);
    }

    public void PlayerPosSubscribe(Action<Vector3> action){
        playerPosStream.Subscribe(action).AddTo(GameManager.Instance);
    }
}
