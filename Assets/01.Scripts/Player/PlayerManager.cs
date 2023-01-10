using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerManager : IManager
{
    private Player _player;
    private IObservable<Vector3> playerPosStream;

    float _minSpeed = 2f;
    float _maxSpeed = 7f;
    float _maxSpeedScore = 700;

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                Init();
                break;
            case GameState.INGAME:
                _player.IsPlay = true;   
                GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(ScoreSetSpeed);
                break;
        }
    }

    public void Init(){
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        playerPosStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME).Select(position => _player.transform.position);
        _player.IsPlay = false;
    }

    private void ScoreSetSpeed(int score)
    {
        if (score == 0)
        {
            _player.FallingSpeed = _minSpeed;
            return;
        }
    
        _player.FallingSpeed = Mathf.Lerp(_minSpeed, _maxSpeed, score / _maxSpeedScore);
    }

    public void PlayerPosSubscribe(Action<Vector3> action){
        playerPosStream.Subscribe(action).AddTo(GameManager.Instance);
    }
}
