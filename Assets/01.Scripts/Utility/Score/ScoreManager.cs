using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ScoreManager : IManager
{
    private int _currentScore = 0;
    private int _bestSocre {
        get => GameManager.Instance.GetManager<DataManager>().User.BestScore; 
        set => GameManager.Instance.GetManager<DataManager>().User.BestScore = value;
    }

    private Vector3 playerStartPos = Vector3.zero;

    private IObservable<int> upScoreStream;

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                Init();
                break;
        }
    }

    private void Init(){
        upScoreStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME).Select(score => _currentScore);

        GameManager.Instance.GetManager<PlayerManager>().PlayerPosSubscribe(PlayerMoveEvent);
    }

    private void PlayerMoveEvent(Vector3 playerPos){
        if(playerStartPos == Vector3.zero) playerStartPos = playerPos;

        _currentScore = (int)Mathf.Abs(playerStartPos.y - playerPos.y);

        if(_currentScore > _bestSocre){
            _bestSocre = _currentScore;
        }
    }

    public void ScoreSubscribe(Action<int> action){
        upScoreStream.Subscribe(action);
    }
}
