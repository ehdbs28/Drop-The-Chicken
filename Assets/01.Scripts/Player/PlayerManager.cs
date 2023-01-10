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
                GameManager.Instance.GetManager<CameraManager>().CamSizeSubscribe(PlayerMoveLimit);
                break;
        }
    }

    public void Init(){
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        playerFeverStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME).Select(list => _player.Fevers);
        playerPosStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME && !_player.IsDie).Select(position => _player.transform.position);
        
        _player.IsPlay = false;
        _player.Fevers = Enumerable.Repeat(false, 5).ToList();
    }

    private void PlayerMoveLimit(float camSize){
        _player.transform.position = new Vector3(Mathf.Clamp(_player.transform.position.x, -camSize + .2f, camSize - .2f), _player.transform.position.y);
    }

    public void PlayerFeverSubscribe(Action<List<bool>> action){
        playerFeverStream.Subscribe(action).AddTo(GameManager.Instance);
    }

    public void PlayerPosSubscribe(Action<Vector3> action){
        playerPosStream.Subscribe(action).AddTo(GameManager.Instance);
    }
}
